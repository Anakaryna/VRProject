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
    
    public FixedJoint GrabbedFixedJoint { get; set; }

    public FixedJoint Grab(Rigidbody body)
    {
        if (GrabbedFixedJoint)
        {
            return null;
        }
        
        GrabbedFixedJoint = GrabAndStorage.grabAutomaticFullSnapPoint(mybody, 0.1f, body, snapPosition, snapRotation, new Vector3(1,1,1), excludingGrabLayerMask);
        return GrabbedFixedJoint;
    }

    public void Release(FixedJoint fixedJoint, Vector3 handsPosition)
    {
        
        Collider[] nearbyColliders = Physics.OverlapSphere(handsPosition, 0.2f, storageLayer);
        if (nearbyColliders.Length > 0 && nearbyColliders[0].CompareTag("BodyStorage"))
        {
            Store(transform.InverseTransformPoint(handsPosition), nearbyColliders[0].gameObject);
        }
        else
        {
            mybody.automaticCenterOfMass = true;
            mybody.excludeLayers = 0;
            mybody.mass = 1;
        }
    }
    
    
    public FixedJoint Stored { get; set; }

    public FixedJoint Store(Vector3 releasePoint, GameObject storage)
    {
        Stored = GrabAndStorage.storeAutomaticFullSnapPoint(mybody, 0f, storage, snapPosition, pocketSnapRotation, new Vector3(1,1,1), excludingGrabLayerMask);
        return Stored;
    }

    public bool StorageRelease()
    {
        Destroy(Stored);
        return true;
    }
}
