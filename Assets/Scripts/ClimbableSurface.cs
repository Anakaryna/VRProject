using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbableSurface : MonoBehaviour, IGrabbable
{
    
    public FixedJoint GrabbedFixedJoint { get; set; }

    public Rigidbody body;
    
    public FixedJoint Grab(Rigidbody body)
    {
        if (GrabbedFixedJoint)
        {
            return null;
        }
        var fixedJoint = body.gameObject.AddComponent<FixedJoint>();
        fixedJoint.autoConfigureConnectedAnchor = false;
        fixedJoint.connectedBody = this.body;
        fixedJoint.connectedAnchor = transform.InverseTransformPoint(body.position);
        GrabbedFixedJoint = fixedJoint;
        return fixedJoint;
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
