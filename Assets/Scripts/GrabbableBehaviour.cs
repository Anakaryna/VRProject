using UnityEngine;

public class GrabbableBehaviour : MonoBehaviour, IGrabbable
{
    
    public FixedJoint GrabbedFixedJoint { get; set; }

    public Rigidbody body;
    public LayerMask excludingGrabLayerMask;

    public FixedJoint Grab(Rigidbody body)
    {
        if (GrabbedFixedJoint)
        {
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
