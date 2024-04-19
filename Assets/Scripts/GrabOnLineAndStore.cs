using UnityEngine;
using importedFunctions;

public class GrabOnLineAndStore : MonoBehaviour, IGrabbable, IStorable
{
    
    public FixedJoint GrabbedFixedJoint { get; set; }

    public Rigidbody body;

    public Transform snapPointA;
    public Transform snapPointB;
    public Transform rotationPoint;
    public Transform inverseRotationPoint;
    public Transform pocketSnapRotation;
    
    public LayerMask storageLayer;

    public Vector3 rotationMask;
    public LayerMask excludingGrabLayerMask;
    
    public void putMass()
    {
        body.mass = 0.2f;
    }
    
    public FixedJoint Stored { get; set; }

    public FixedJoint Store(Vector3 releasePoint, GameObject storage)
    {
        Stored = GrabAndStorage.storeAutomaticFullSnapPoint(body, storage, snapPointA, pocketSnapRotation, rotationMask, excludingGrabLayerMask);
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

        GrabbedFixedJoint = GrabAndStorage.grabAutoFullSnapLine(this.body, body, snapPointA, snapPointB, rotationPoint, inverseRotationPoint, rotationMask,
            excludingGrabLayerMask);
        Invoke(nameof(putMass), 0.1f);
        makeTransfer = true;
        return GrabbedFixedJoint;
    }

    public GameObject Release(FixedJoint fixedJoint, Vector3 handsPosition, out bool stored)
    {
        Collider[] nearbyColliders = Physics.OverlapSphere(handsPosition, 0.2f, storageLayer);
        if (nearbyColliders.Length > 0 && nearbyColliders[0].CompareTag("BodyStorage"))
        {
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
