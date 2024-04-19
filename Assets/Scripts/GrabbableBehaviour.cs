using UnityEngine;

public class GrabbableBehaviour : MonoBehaviour, IGrabbable
{
    
    public FixedJoint GrabbedFixedJoint { get; set; }

    public Rigidbody body;
    public LayerMask excludingGrabLayerMask;

    public FixedJoint Grab(Rigidbody body, out bool makeTransfer)
    {
        if (GrabbedFixedJoint)
        {
            makeTransfer = true;
            return null;
        }
        this.body.excludeLayers = excludingGrabLayerMask;
        this.body.automaticCenterOfMass = false;
        Vector3 pos = transform.InverseTransformPoint(transform.position);
        this.body.centerOfMass = pos;
        this.body.mass = 0.1f;
        var fixedJoint = body.gameObject.AddComponent<FixedJoint>();
        fixedJoint.autoConfigureConnectedAnchor = false;
        fixedJoint.connectedBody = this.body;
        fixedJoint.connectedAnchor = pos;
        GrabbedFixedJoint = fixedJoint;
        makeTransfer = true;
        return fixedJoint;
    }

    public GameObject Release(FixedJoint fixedJoint, Vector3 handsPosition, out bool stored)
    {
        body.automaticCenterOfMass = true;
        body.excludeLayers = 0;
        body.mass = 1;
        stored = false;
        return gameObject;
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
