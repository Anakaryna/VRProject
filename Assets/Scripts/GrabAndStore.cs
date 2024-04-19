using System.Collections.Generic;
using UnityEngine;
using importedFunctions;

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

    public FixedJoint Grab(Rigidbody body, out bool makeTransfer)
    {
        
        if (GrabbedFixedJoint)
        {
            makeTransfer = true;
            return null;
        }

        Transform closest = GrabAndStorage.getClosestPosition(snapPoints, body.transform);

        GrabbedFixedJoint = GrabAndStorage.grabAutomaticFullSnapPoint(this.body, body, closest, closest, new Vector3(1,1,1), excludingGrabLayerMask);
        Invoke(nameof(putMass), 0.1f);
        makeTransfer = true;
        return GrabbedFixedJoint;
    }

    public GameObject Release(FixedJoint fixedJoint, Vector3 handsPosition, out bool stored)
    {
        Collider[] nearbyColliders = Physics.OverlapSphere(handsPosition, 0.2f, storageLayer);
        if (nearbyColliders.Length > 0 && nearbyColliders[0].CompareTag("BodyStorage"))
        {
            print(nearbyColliders[0]);
            stored = Store(transform.InverseTransformPoint(handsPosition), nearbyColliders[0].gameObject);
        }
        else
        {
            body.automaticCenterOfMass = true;
            body.excludeLayers = 0;
            body.mass = 1;
            stored = false;
        }

        return gameObject;
    }
    
}