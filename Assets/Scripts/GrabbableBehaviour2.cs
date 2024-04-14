using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using importedFunctions;
using Unity.Mathematics;

using importedFunctions;
using UnityEditor.SceneManagement;

public class GrabbableBehaviour2 : MonoBehaviour, IGrabbable
{
    
    public FixedJoint GrabbedFixedJoint { get; set; }

    public Rigidbody body;

    public Transform snapPointA;
    public Transform snapPointB;
    public Transform rotationPoint;

    public Vector3 rotationMask;

    public FixedJoint Grab(Rigidbody body)
    {
        if (GrabbedFixedJoint)
        {
            return null;
        }
        var fixedJoint = body.gameObject.AddComponent<FixedJoint>();
        fixedJoint.autoConfigureConnectedAnchor = false;
        this.body.isKinematic = true;
        print(snapPointA.position);
        print(snapPointB.position);
        print(body.position);
        Vector3 pos = ClosestPointToLine.getClosestPointToLine(snapPointA.localPosition, snapPointB.localPosition,
            transform.InverseTransformPoint(body.position));
        print(pos);
        //print(transform.InverseTransformPoint(ClosestPointToLine.getClosestPointToLine(snapPointA.position, snapPointB.position, body.position)));
        transform.rotation = SnapRotation.getSnapRotation(rotationPoint.localRotation, transform.rotation, body.rotation, rotationMask);
        fixedJoint.connectedBody = this.body;
        fixedJoint.connectedAnchor = pos;
        //print(transform.InverseTransformPoint(ClosestPointToLine.getClosestPointToLine(snapPointA.position, snapPointB.position, body.position)));
        this.body.isKinematic = false;
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
