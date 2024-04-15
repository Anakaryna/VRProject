using UnityEngine;

public class GrabbableBehaviour1 : MonoBehaviour, IGrabbable
{
    
    public FixedJoint GrabbedFixedJoint { get; set; }

    public Rigidbody body;
    
    public AudioClip audioClip;

    public Transform snapPoint;
    public LayerMask excludingGrabLayerMask;

    public FixedJoint Grab(Rigidbody body)
    {
        if (GrabbedFixedJoint)
        {
            return null;
        }
        this.body.excludeLayers = excludingGrabLayerMask;
        this.body.automaticCenterOfMass = false;
        Vector3 pos = transform.InverseTransformPoint(snapPoint.position);
        this.body.centerOfMass = pos;
        this.body.mass = 0.1f;
        var fixedJoint = body.gameObject.AddComponent<FixedJoint>();
        fixedJoint.autoConfigureConnectedAnchor = false;
        this.body.isKinematic = true;
        fixedJoint.connectedBody = this.body;
        
        fixedJoint.connectedAnchor = pos;
        
        this.body.isKinematic = false;
        AudioSource.PlayClipAtPoint(audioClip, transform.position, 1);
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
