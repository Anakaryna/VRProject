using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableBehaviour : MonoBehaviour, IGrabbable
{
    
    public bool Grabbed { get; set; }

    public Rigidbody body;

    public bool Grab(FixedJoint fixedJoint)
    {
        fixedJoint.connectedBody = body;
        fixedJoint.connectedAnchor = body.transform.InverseTransformPoint(transform.position);
        return true;
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
