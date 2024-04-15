using UnityEngine;
using importedFunctions;

public class GrabbableBehaviour2 : MonoBehaviour, IGrabbable
{
    
    public FixedJoint GrabbedFixedJoint { get; set; }

    public Rigidbody body;

    public Transform snapPointA;
    public Transform snapPointB;
    public Transform rotationPoint;

    public Vector3 rotationMask;
    public LayerMask excludingGrabLayerMask;
    
    public FixedJoint Grab(Rigidbody body)
    {
        if (GrabbedFixedJoint)
        {
            return null;
        }

        this.body.excludeLayers = excludingGrabLayerMask;
        this.body.automaticCenterOfMass = false;
        Vector3 pos = ClosestPointToLine.getClosestPointToLine(snapPointA.localPosition, snapPointB.localPosition,
            transform.InverseTransformPoint(body.position));
        this.body.centerOfMass = pos;
        this.body.mass = 0.1f;
        var fixedJoint = body.gameObject.AddComponent<FixedJoint>();
        fixedJoint.autoConfigureConnectedAnchor = false;
        this.body.isKinematic = true;
        
        transform.rotation = SnapRotation.getSnapRotation(rotationPoint.localRotation, transform.rotation, body.rotation, rotationMask);
        fixedJoint.connectedBody = this.body;
        fixedJoint.connectedAnchor = pos;
        
        this.body.isKinematic = false;
        GrabbedFixedJoint = fixedJoint;
        return fixedJoint;
    }

    public void Release(FixedJoint fixedJoint, Vector3 handsPosition)
    {
        body.automaticCenterOfMass = true;
        body.excludeLayers = 0;
        body.mass = 1;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
