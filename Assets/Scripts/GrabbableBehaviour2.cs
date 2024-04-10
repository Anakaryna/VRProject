using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using importedFunctions;

public class GrabbableBehaviour2 : MonoBehaviour, IGrabbable
{
    
    public FixedJoint GrabbedFixedJoint { get; set; }

    public Rigidbody body;
    
    public AudioClip audioClip;

    public Transform snapPointA;
    public Transform snapPointB;

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
        fixedJoint.connectedAnchor = transform.InverseTransformPoint(ClosestPointToLine.getClosestPointToLine(snapPointA.position, snapPointB.position, body.position));
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
