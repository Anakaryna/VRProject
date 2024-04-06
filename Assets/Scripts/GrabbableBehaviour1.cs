using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableBehaviour1 : MonoBehaviour, IGrabbable
{
    
    public bool Grabbed { get; set; }

    public Rigidbody body;
    
    public AudioClip audioClip;

    public Transform snapPoint;

    public bool Grab(FixedJoint fixedJoint)
    {
        body.isKinematic = true;
        fixedJoint.connectedBody = body;
        fixedJoint.connectedAnchor = body.transform.InverseTransformPoint(snapPoint.position);
        body.isKinematic = false;
        AudioSource.PlayClipAtPoint(audioClip, transform.position, 1);
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
