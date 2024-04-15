using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbableSurface : MonoBehaviour, IGrabbable
{
    
    public FixedJoint GrabbedFixedJoint { get; set; }

    public Rigidbody body;
    
    public FixedJoint Grab(Rigidbody body)
    {
        var fixedJoint = body.gameObject.AddComponent<FixedJoint>();
        fixedJoint.autoConfigureConnectedAnchor = false;
        fixedJoint.connectedAnchor = transform.position;
        return fixedJoint;
    }

    public void Release(FixedJoint fixedJoint, Vector3 handsPosition)
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
