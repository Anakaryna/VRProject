using System;
using UnityEngine;
using importedFunctions;
using Unity.Mathematics;

public class TwoHandedGrabAndStorage : MonoBehaviour, IGrabbable, IStorable
{
    
    public FixedJoint GrabbedFixedJoint { get; set; }

    public Rigidbody body;

    public Transform snapPointA;
    public Transform snapPointB;

    public Vector3 rotationMask;

    public LayerMask storageLayer;
    
    public FixedJoint Stored { get; set; }

    public FixedJoint Store(Vector3 releasePoint, GameObject storage)
    {
        body.excludeLayers = ~0;
        body.mass = 0;
        var fixedJoint = storage.AddComponent<FixedJoint>();
        fixedJoint.massScale = 0;
        fixedJoint.autoConfigureConnectedAnchor = false;
        fixedJoint.connectedAnchor = storage.transform.position;
        fixedJoint.connectedBody = body;
        Stored = fixedJoint;
        return fixedJoint;
    }

    public bool StorageRelease()
    {
        body.mass = 1;
        Destroy(Stored);
        body.excludeLayers = 0;
        return true;
    }

    public FixedJoint Grab(Rigidbody body)
    {
        if (GrabbedFixedJoint)
        {
            return null;
        }
        var fixedJoint = body.gameObject.AddComponent<FixedJoint>();
        fixedJoint.autoConfigureConnectedAnchor = false;
        this.body.isKinematic = true;
        var distA = Vector3.Distance(body.position, snapPointA.position);
        var distB = Vector3.Distance(body.position, snapPointB.position);
        Vector3 pos;
        if (distA < distB)
        {
            pos = transform.InverseTransformPoint(snapPointA.position);
            transform.rotation = SnapRotation.getSnapRotation(snapPointA.localRotation, transform.rotation, body.rotation,
                new Vector3(1, 1, 1));
        }
        else
        {
            pos = transform.InverseTransformPoint(snapPointB.position);
            transform.rotation = SnapRotation.getSnapRotation(snapPointB.localRotation, transform.rotation, body.rotation,
                new Vector3(1, 1, 1));
        }
        
        
        fixedJoint.connectedBody = this.body;
        fixedJoint.connectedAnchor = pos;
        
        this.body.isKinematic = false;
        GrabbedFixedJoint = fixedJoint;
        return fixedJoint;
    }

    public void Release(FixedJoint fixedJoint)
    {
        Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, 5, storageLayer);
        if (nearbyColliders.Length > 0 && nearbyColliders[0].CompareTag("BodyStorage"))
        {
            print(nearbyColliders[0]);
            Store(fixedJoint.connectedAnchor, nearbyColliders[0].gameObject);
        }
    }
    
}
