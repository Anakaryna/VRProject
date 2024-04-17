using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using importedFunctions;

public class PistolScript : MonoBehaviour, IGrabbable, IStorable
{


    public Rigidbody mybody;
    public Transform snapPosition;
    public Transform snapRotation;
    public Transform pocketSnapRotation;
    
    public LayerMask storageLayer;
    public LayerMask excludingGrabLayerMask;
    
    public void putMass()
    {
        mybody.mass = 0.2f;
    }
    
    public FixedJoint GrabbedFixedJoint { get; set; }

    public FixedJoint Grab(Rigidbody body, out bool makeTransfer)
    {
        if (GrabbedFixedJoint)
        {
            makeTransfer = false;
            return null;
        }
        
        GrabbedFixedJoint = GrabAndStorage.grabAutomaticFullSnapPoint(mybody, body, snapPosition, snapRotation, new Vector3(1,1,1), excludingGrabLayerMask);
        Invoke(nameof(putMass), 0.1f);
        makeTransfer = true;
        return GrabbedFixedJoint;
    }

    public void Release(FixedJoint fixedJoint, Vector3 handsPosition, out bool stored)
    {
        
        Collider[] nearbyColliders = Physics.OverlapSphere(handsPosition, 0.2f, storageLayer);
        if (nearbyColliders.Length > 0 && nearbyColliders[0].CompareTag("BodyStorage"))
        {
            stored = Store(transform.InverseTransformPoint(handsPosition), nearbyColliders[0].gameObject);
        }
        else
        {
            mybody.automaticCenterOfMass = true;
            mybody.excludeLayers = 0;
            mybody.mass = 1;
            stored = false;
        }
    }
    
    
    public FixedJoint Stored { get; set; }

    public FixedJoint Store(Vector3 releasePoint, GameObject storage)
    {
        Stored = GrabAndStorage.storeAutomaticFullSnapPoint(mybody, storage, snapPosition, pocketSnapRotation, new Vector3(1,1,1), excludingGrabLayerMask);
        return Stored;
    }

    public bool StorageRelease()
    {
        Destroy(Stored);
        return true;
    }
}