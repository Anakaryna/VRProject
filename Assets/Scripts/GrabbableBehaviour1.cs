using UnityEngine;
using importedFunctions;
public class GrabbableBehaviour1 : MonoBehaviour, IGrabbable
{
    
    public FixedJoint GrabbedFixedJoint { get; set; }

    public Rigidbody body;
    
    public AudioClip audioClip;

    public Transform snapPoint;
    public LayerMask excludingGrabLayerMask;

    public FixedJoint Grab(Rigidbody body)
    {
        if (GrabbedFixedJoint)
        {
            return null;
        }
        
        GrabbedFixedJoint = GrabAndStorage.grabAutomaticFullSnapPoint(this.body, body, snapPoint, snapPoint, new Vector3(0,0,0), excludingGrabLayerMask);
        
        AudioSource.PlayClipAtPoint(audioClip, transform.position, 1);
        return GrabbedFixedJoint;
    }

    public void Release(FixedJoint fixedJoint, Vector3 handsPosition)
    {
        body.automaticCenterOfMass = true;
        body.excludeLayers = 0;
        body.mass = 1;
    }
}
