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
    public Transform pocketRotation;

    public Vector3 rotationMask;

    public LayerMask storageLayer;
    public LayerMask excludingGrabLayerMask;
    
    public FixedJoint Stored { get; set; }

    public FixedJoint Store(Vector3 releasePoint, GameObject storage)
    {
        body.excludeLayers = excludingGrabLayerMask;
        body.automaticCenterOfMass = false;
        body.centerOfMass = snapPointA.localPosition;
        body.mass = 0f;
        var fixedJoint = storage.AddComponent<FixedJoint>();
        fixedJoint.autoConfigureConnectedAnchor = false;
        transform.rotation = SnapRotation.getSnapRotation(pocketRotation.localRotation, transform.rotation,
            storage.transform.rotation, new Vector3(1, 1, 1));
        fixedJoint.connectedAnchor = snapPointA.localPosition;
        fixedJoint.connectedBody = body;
        Stored = fixedJoint;
        return fixedJoint;
    }

    public bool StorageRelease()
    {
        Destroy(Stored);
        return true;
    }

    public FixedJoint Grab(Rigidbody body)
    {
        if (GrabbedFixedJoint)
        {
            return null;
        }

        this.body.excludeLayers = excludingGrabLayerMask;
        this.body.automaticCenterOfMass = false;
        this.body.centerOfMass = snapPointA.localPosition;
        this.body.mass = 0.1f;
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

    public void Release(FixedJoint fixedJoint, Vector3 handsPosition)
    {
        Collider[] nearbyColliders = Physics.OverlapSphere(handsPosition, 0.2f, storageLayer);
        if (nearbyColliders.Length > 0 && nearbyColliders[0].CompareTag("BodyStorage"))
        {
            print(nearbyColliders[0]);
            Store(transform.InverseTransformPoint(handsPosition), nearbyColliders[0].gameObject);
        }
        else
        {
            body.automaticCenterOfMass = true;
            body.excludeLayers = 0;
            body.mass = 1;
        }
    }
    
}
