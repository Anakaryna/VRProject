using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using importedFunctions;

public class GrabbableBehaviour2 : MonoBehaviour, IGrabbable
{
    
    public bool Grabbed { get; set; }

    public Rigidbody body;
    
    public AudioClip audioClip;

    public Transform snapPointA;
    public Transform snapPointB;

    public bool Grab(FixedJoint fixedJoint, Vector3 handsPosition)
    {
        body.isKinematic = true;
        fixedJoint.connectedBody = body;
        fixedJoint.connectedAnchor = body.transform.InverseTransformPoint(ClosestPointToLine.getClosestPointToLine(snapPointA.position, snapPointB.position, handsPosition));
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
