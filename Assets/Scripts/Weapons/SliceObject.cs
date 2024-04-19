using UnityEngine;
using EzySlice;

public class SliceObject : MonoBehaviour
{
    public Transform startSlicePoint;
    public Transform endSlicePoint;
    public VelocityEstimator velocityEstimator;
    public LayerMask sliceableLayer;
    public Material crossSectionMaterial;
    public AudioSource audioSource; // Référence à l'AudioSource

    public float cutForce = 2;
    public float minCutVelocity = 5; 

    void FixedUpdate()
    {
        bool hasHit = Physics.Linecast(startSlicePoint.position, endSlicePoint.position, out RaycastHit hit, sliceableLayer);
        if (hasHit)
        {
            GameObject target = hit.transform.gameObject;
            Slice(target);
        }
    }

    public void Slice(GameObject target)
    {
        Vector3 velocity = velocityEstimator.GetVelocityEstimate();
        if (velocity.magnitude >= minCutVelocity)
        {
            Vector3 planeNormal = Vector3.Cross(endSlicePoint.position - startSlicePoint.position, velocity);
            planeNormal.Normalize();

            SlicedHull hull = target.Slice(endSlicePoint.position, planeNormal);
            if (hull != null)
            {
                GameObject upperHull = hull.CreateUpperHull(target, crossSectionMaterial);
                SetupSlicedComponent(upperHull, target);  
                upperHull.layer = target.layer;

                GameObject lowerHull = hull.CreateLowerHull(target, crossSectionMaterial);
                SetupSlicedComponent(lowerHull, target);  
                lowerHull.layer = target.layer;

                audioSource.Play(); // Jouer le son
                Destroy(target);
            }
        }
    }

    public void SetupSlicedComponent(GameObject slicedObject, GameObject original)
    {
        Rigidbody rb = slicedObject.AddComponent<Rigidbody>();
        MeshCollider collider = slicedObject.AddComponent<MeshCollider>();
        collider.convex = true;
        rb.AddExplosionForce(cutForce, slicedObject.transform.position, 1);

        MeshDestructor originalScript = original.GetComponent<MeshDestructor>();
        if (originalScript != null)
        {
            MeshDestructor newScript = slicedObject.AddComponent<MeshDestructor>();
        }
    }
}
