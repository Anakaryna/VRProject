using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDestructor : MonoBehaviour
{
    private bool isEdgeDefined = false;  // Indicates if an edge has been defined
    private Vector3 edgePosition = Vector3.zero;  // Position of the edge vertex
    private Vector2 edgeUVCoordinates = Vector2.zero;  // UV coordinates of the edge
    private Plane slicingPlane = new Plane();  // Plane used for slicing the mesh
    public float requiredImpactVelocity = 5;  // Minimum velocity required for mesh destruction

    public int slicingSubdivisions = 3;  // Number of subdivisions for slicing
    public float explosionForce = 5;  // Explosion force after destruction

    // Start is called before the first frame update, used for initialization
    void Start()
    {
    }

    // Update is called once per frame, checks for user input to trigger mesh destruction
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DestroyMesh();
        }
    }

    // Handles collision events to check if the collider is a 'Weapon' and exceeds the required velocity for destruction
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Weapon" && collision.relativeVelocity.magnitude > requiredImpactVelocity)
        {
            DestroyMesh();
        }
    }

    // Main function to handle the destruction of the mesh
    private void DestroyMesh()
    {
        var originalMesh = GetComponent<MeshFilter>().mesh;
        originalMesh.RecalculateBounds();
        var meshParts = new List<MeshPart>();
        var subMeshParts = new List<MeshPart>();

        var primaryMeshPart = new MeshPart()
        {
            UV = originalMesh.uv,
            Vertices = originalMesh.vertices,
            Normals = originalMesh.normals,
            Triangles = new int[originalMesh.subMeshCount][],
            Bounds = originalMesh.bounds
        };

        for (int i = 0; i < originalMesh.subMeshCount; i++)
        {
            primaryMeshPart.Triangles[i] = originalMesh.GetTriangles(i);
        }

        meshParts.Add(primaryMeshPart);

        for (var c = 0; c < slicingSubdivisions; c++)
        {
            foreach (var part in meshParts)
            {
                var expandedBounds = part.Bounds;
                expandedBounds.Expand(0.5f);

                var randomPlane = new Plane(UnityEngine.Random.onUnitSphere, new Vector3(
                    UnityEngine.Random.Range(expandedBounds.min.x, expandedBounds.max.x),
                    UnityEngine.Random.Range(expandedBounds.min.y, expandedBounds.max.y),
                    UnityEngine.Random.Range(expandedBounds.min.z, expandedBounds.max.z)));

                subMeshParts.Add(CreateMeshPart(part, randomPlane, true));
                subMeshParts.Add(CreateMeshPart(part, randomPlane, false));
            }
            meshParts = new List<MeshPart>(subMeshParts);
            subMeshParts.Clear();
        }

        foreach (var part in meshParts)
        {
            part.CreateGameObject(this);
            part.GameObject.GetComponent<Rigidbody>().AddForceAtPosition(part.Bounds.center * explosionForce, transform.position);
        }

        Destroy(gameObject); // Destroys the current object after the mesh has been split into parts
    }

    // Creates a new mesh part by slicing an existing mesh part with a defined plane
    private MeshPart CreateMeshPart(MeshPart original, Plane plane, bool isLeftSide)
    {
        var newMeshPart = new MeshPart() { };
        var ray1 = new Ray();
        var ray2 = new Ray();

        for (var i = 0; i < original.Triangles.Length; i++)
        {
            var triangles = original.Triangles[i];
            isEdgeDefined = false;

            for (var j = 0; j < triangles.Length; j += 3)
            {
                var sideA = plane.GetSide(original.Vertices[triangles[j]]) == isLeftSide;
                var sideB = plane.GetSide(original.Vertices[triangles[j + 1]]) == isLeftSide;
                var sideC = plane.GetSide(original.Vertices[triangles[j + 2]]) == isLeftSide;

                var sideCount = (sideA ? 1 : 0) +
                                (sideB ? 1 : 0) +
                                (sideC ? 1 : 0);
                if (sideCount == 0)
                {
                    continue;
                }
                if (sideCount == 3)
                {
                    newMeshPart.AddTriangle(i,
                                         original.Vertices[triangles[j]], original.Vertices[triangles[j + 1]], original.Vertices[triangles[j + 2]],
                                         original.Normals[triangles[j]], original.Normals[triangles[j + 1]], original.Normals[triangles[j + 2]],
                                         original.UV[triangles[j]], original.UV[triangles[j + 1]], original.UV[triangles[j + 2]]);
                    continue;
                }
                
                var singleIndex = sideB == sideC ? 0 : sideA == sideC ? 1 : 2;

                ray1.origin = original.Vertices[triangles[j + singleIndex]];
                var dir1 = original.Vertices[triangles[j + ((singleIndex + 1) % 3)]] - original.Vertices[triangles[j + singleIndex]];
                ray1.direction = dir1;
                plane.Raycast(ray1, out var enter1);
                var lerp1 = enter1 / dir1.magnitude;

                ray2.origin = original.Vertices[triangles[j + singleIndex]];
                var dir2 = original.Vertices[triangles[j + ((singleIndex + 2) % 3)]] - original.Vertices[triangles[j + singleIndex]];
                ray2.direction = dir2;
                plane.Raycast(ray2, out var enter2);
                var lerp2 = enter2 / dir2.magnitude;
                
                AddEdge(i,
                        newMeshPart,
                        isLeftSide ? plane.normal * -1f : plane.normal,
                        ray1.origin + ray1.direction.normalized * enter1,
                        ray2.origin + ray2.direction.normalized * enter2,
                        Vector2.Lerp(original.UV[triangles[j + singleIndex]], original.UV[triangles[j + ((singleIndex + 1) % 3)]], lerp1),
                        Vector2.Lerp(original.UV[triangles[j + singleIndex]], original.UV[triangles[j + ((singleIndex + 2) % 3)]], lerp2));

                if (sideCount == 1)
                {
                    newMeshPart.AddTriangle(i,
                                        original.Vertices[triangles[j + singleIndex]],
                                        ray1.origin + ray1.direction.normalized * enter1,
                                        ray2.origin + ray2.direction.normalized * enter2,
                                        original.Normals[triangles[j + singleIndex]],
                                        Vector3.Lerp(original.Normals[triangles[j + singleIndex]], original.Normals[triangles[j + ((singleIndex + 1) % 3)]], lerp1),
                                        Vector3.Lerp(original.Normals[triangles[j + singleIndex]], original.Normals[triangles[j + ((singleIndex + 2) % 3)]], lerp2),
                                        original.UV[triangles[j + singleIndex]],
                                        Vector2.Lerp(original.UV[triangles[j + singleIndex]], original.UV[triangles[j + ((singleIndex + 1) % 3)]], lerp1),
                                        Vector2.Lerp(original.UV[triangles[j + singleIndex]], original.UV[triangles[j + ((singleIndex + 2) % 3)]], lerp2));
                    
                    continue;
                }

                if (sideCount == 2)
                {
                    newMeshPart.AddTriangle(i,
                                        ray1.origin + ray1.direction.normalized * enter1,
                                        original.Vertices[triangles[j + ((singleIndex + 1) % 3)]],
                                        original.Vertices[triangles[j + ((singleIndex + 2) % 3)]],
                                        Vector3.Lerp(original.Normals[triangles[j + singleIndex]], original.Normals[triangles[j + ((singleIndex + 1) % 3)]], lerp1),
                                        original.Normals[triangles[j + ((singleIndex + 1) % 3)]],
                                        original.Normals[triangles[j + ((singleIndex + 2) % 3)]],
                                        Vector2.Lerp(original.UV[triangles[j + singleIndex]], original.UV[triangles[j + ((singleIndex + 1) % 3)]], lerp1),
                                        original.UV[triangles[j + ((singleIndex + 1) % 3)]],
                                        original.UV[triangles[j + ((singleIndex + 2) % 3)]]);
                    newMeshPart.AddTriangle(i,
                                        ray1.origin + ray1.direction.normalized * enter1,
                                        original.Vertices[triangles[j + ((singleIndex + 2) % 3)]],
                                        ray2.origin + ray2.direction.normalized * enter2,
                                        Vector3.Lerp(original.Normals[triangles[j + singleIndex]], original.Normals[triangles[j + ((singleIndex + 1) % 3)]], lerp1),
                                        original.Normals[triangles[j + ((singleIndex + 2) % 3)]],
                                        Vector3.Lerp(original.Normals[triangles[j + singleIndex]], original.Normals[triangles[j + ((singleIndex + 2) % 3)]], lerp2),
                                        Vector2.Lerp(original.UV[triangles[j + singleIndex]], original.UV[triangles[j + ((singleIndex + 1) % 3)]], lerp1),
                                        original.UV[triangles[j + ((singleIndex + 2) % 3)]],
                                        Vector2.Lerp(original.UV[triangles[j + singleIndex]], original.UV[triangles[j + ((singleIndex + 2) % 3)]], lerp2));
                    continue;
                }
            }
        }

        newMeshPart.FillArrays();

        return newMeshPart; // Returns the generated mesh part
    }

    // Adds an edge between two vertices during the mesh slicing process
    private void AddEdge(int subMesh, MeshPart newMeshPart, Vector3 normal, Vector3 vertex1, Vector3 vertex2, Vector2 uv1, Vector2 uv2)
    {
        if (!isEdgeDefined)
        {
            isEdgeDefined = true;
            edgePosition = vertex1;
            edgeUVCoordinates = uv1;
        }
        else
        {
            slicingPlane.Set3Points(edgePosition, vertex1, vertex2);

            newMeshPart.AddTriangle(subMesh,
                                edgePosition,
                                slicingPlane.GetSide(edgePosition + normal) ? vertex1 : vertex2,
                                slicingPlane.GetSide(edgePosition + normal) ? vertex2 : vertex1,
                                normal,
                                normal,
                                normal,
                                edgeUVCoordinates,
                                uv1,
                                uv2);
        }
    }

    public class MeshPart
    {
        private List<Vector3> _Vertices = new List<Vector3>();
        private List<Vector3> _Normals = new List<Vector3>();
        private List<List<int>> _Triangles = new List<List<int>>();
        private List<Vector2> _UVs = new List<Vector2>();
        public Vector3[] Vertices;
        public Vector3[] Normals;
        public int[][] Triangles;
        public Vector2[] UV;
        public GameObject GameObject;
        public Bounds Bounds = new Bounds();

        // Adds a triangle to the mesh part, updating vertex, normal, and UV lists
        public void AddTriangle(int submesh, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, Vector3 normal1, Vector3 normal2, Vector3 normal3, Vector2 uv1, Vector2 uv2, Vector2 uv3)
        {
            if (_Triangles.Count - 1 < submesh)
                _Triangles.Add(new List<int>());

            _Triangles[submesh].Add(_Vertices.Count);
            _Vertices.Add(vertex1);
            _Triangles[submesh].Add(_Vertices.Count);
            _Vertices.Add(vertex2);
            _Triangles[submesh].Add(_Vertices.Count);
            _Vertices.Add(vertex3);
            _Normals.Add(normal1);
            _Normals.Add(normal2);
            _Normals.Add(normal3);
            _UVs.Add(uv1);
            _UVs.Add(uv2);
            _UVs.Add(uv3);

            Bounds.min = Vector3.Min(Bounds.min, vertex1);
            Bounds.min = Vector3.Min(Bounds.min, vertex2);
            Bounds.min = Vector3.Min(Bounds.min, vertex3);
            Bounds.max = Vector3.Max(Bounds.max, vertex1);
            Bounds.max = Vector3.Max(Bounds.max, vertex2);
            Bounds.max = Vector3.Max(Bounds.max, vertex3);
        }

        // Compiles the arrays for vertices, normals, UVs, and triangles for the mesh part
        public void FillArrays()
        {
            Vertices = _Vertices.ToArray();
            Normals = _Normals.ToArray();
            UV = _UVs.ToArray();
            Triangles = new int[_Triangles.Count][];
            for (var i = 0; i < _Triangles.Count; i++)
                Triangles[i] = _Triangles[i].ToArray();
        }

        // Creates a GameObject from this mesh part
        public void CreateGameObject(MeshDestructor original)
        {
            GameObject = new GameObject(original.name);
            GameObject.transform.position = original.transform.position;
            GameObject.transform.rotation = original.transform.rotation;
            GameObject.transform.localScale = original.transform.localScale;
            
            GameObject.layer = original.gameObject.layer;  

            var mesh = new Mesh();
            mesh.name = original.GetComponent<MeshFilter>().mesh.name;

            mesh.vertices = Vertices;
            mesh.normals = Normals;
            mesh.uv = UV;
            for(var i = 0; i < Triangles.Length; i++)
                mesh.SetTriangles(Triangles[i], i, true);
            Bounds = mesh.bounds;
    
            var renderer = GameObject.AddComponent<MeshRenderer>();
            renderer.materials = original.GetComponent<MeshRenderer>().materials;

            var filter = GameObject.AddComponent<MeshFilter>();
            filter.mesh = mesh;

            var collider = GameObject.AddComponent<MeshCollider>();
            collider.convex = true;

            var rigidbody = GameObject.AddComponent<Rigidbody>();
            var meshDestructor = GameObject.AddComponent<MeshDestructor>();
            meshDestructor.slicingSubdivisions = original.slicingSubdivisions;
            meshDestructor.explosionForce = original.explosionForce;
        }
    }
}
