using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using importedFunctions;
using Unity.Mathematics;
using Unity.VisualScripting;

public class GrabAndStore : MonoBehaviour, IGrabbable, IStorable
{
    
    public FixedJoint GrabbedFixedJoint { get; set; }

    public Rigidbody body;

    public List<Transform> snapPoints;
    public Transform pocketRotation;

    public Vector3 rotationMask;

    public LayerMask storageLayer;
    public LayerMask excludingGrabLayerMask;

    public void putMass()
    {
        body.mass = 0.2f;
    }
    
    public FixedJoint Stored { get; set; }

    public FixedJoint Store(Vector3 releasePoint, GameObject storage)
    {
        Stored = GrabAndStorage.storeAutomaticFullSnapPoint(body, storage, snapPoints[0], pocketRotation, new Vector3(1,1,1), excludingGrabLayerMask);
        return Stored;
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

        Transform closest = GrabAndStorage.getClosestPosition(snapPoints, body.transform);

        GrabbedFixedJoint = GrabAndStorage.grabAutomaticFullSnapPoint(this.body, body, closest, closest, new Vector3(1,1,1), excludingGrabLayerMask);
        Invoke(nameof(putMass), 0.1f);
        return GrabbedFixedJoint;
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