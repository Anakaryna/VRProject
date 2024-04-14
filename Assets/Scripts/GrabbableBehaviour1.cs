using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableBehaviour1 : MonoBehaviour, IGrabbable
{
    
    public FixedJoint GrabbedFixedJoint { get; set; }

    public Rigidbody body;
    
    public AudioClip audioClip;

    public Transform snapPoint;

    public FixedJoint Grab(Rigidbody body)
    {
        if (GrabbedFixedJoint)
        {
            return null;
        }
        var fixedJoint = body.gameObject.AddComponent<FixedJoint>();
        fixedJoint.autoConfigureConnectedAnchor = false;
        this.body.isKinematic = true;
        fixedJoint.connectedBody = this.body;
        
        fixedJoint.connectedAnchor = transform.InverseTransformPoint(snapPoint.position);
        
        this.body.isKinematic = false;
        AudioSource.PlayClipAtPoint(audioClip, transform.position, 1);
        GrabbedFixedJoint = fixedJoint;
        return fixedJoint;
    }

    public void Release(FixedJoint fixedJoint)
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
