using UnityEngine;
using importedFunctions;
public class GrabbableBehaviour1 : MonoBehaviour, IGrabbable
{
    
    public FixedJoint GrabbedFixedJoint { get; set; }

    public Rigidbody body;
    
    public AudioClip audioClip;

    public Transform snapPoint;
    public LayerMask excludingGrabLayerMask;
    
    public void putMass()
    {
        body.mass = 0.2f;
    }

    public FixedJoint Grab(Rigidbody body, out bool makeTransfer)
    {
        if (GrabbedFixedJoint)
        {
            makeTransfer = true;
            return null;
        }
        
        GrabbedFixedJoint = GrabAndStorage.grabAutomaticFullSnapPoint(this.body, body, snapPoint, snapPoint, new Vector3(0,0,0), excludingGrabLayerMask);
        Invoke(nameof(putMass), 0.1f);
        AudioSource.PlayClipAtPoint(audioClip, transform.position, 1);
        makeTransfer = true;
        return GrabbedFixedJoint;
    }

    public GameObject Release(FixedJoint fixedJoint, Vector3 handsPosition, out bool stored)
    {
        body.automaticCenterOfMass = true;
        body.excludeLayers = 0;
        body.mass = 1;
        stored = false;
        return gameObject;
    }
}
