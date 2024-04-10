using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbableSurface : MonoBehaviour, IGrabbable
{
    
    public ConfigurableJoint GrabbedFixedJoint { get; set; }

    public Rigidbody body;
    
    public ConfigurableJoint Grab(Rigidbody body)
    {
        var fixedJoint = body.gameObject.AddComponent<ConfigurableJoint>();
        fixedJoint.autoConfigureConnectedAnchor = false;
        fixedJoint.connectedBody = this.body;
        fixedJoint.connectedAnchor = transform.InverseTransformPoint(body.position);
        return fixedJoint;
    }

    public void Release(ConfigurableJoint fixedJoint)
    {
        
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
