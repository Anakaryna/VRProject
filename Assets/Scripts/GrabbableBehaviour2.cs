using UnityEngine;
using importedFunctions;

public class GrabbableBehaviour2 : MonoBehaviour, IGrabbable
{
    
    public FixedJoint GrabbedFixedJoint { get; set; }

    public Rigidbody body;

    public Transform snapPointA;
    public Transform snapPointB;
    public Transform rotationPoint;
    public Transform inverseRotationPoint;

    public Vector3 rotationMask;
    public LayerMask excludingGrabLayerMask;
    
    public void putMass()
    {
        body.mass = 0.2f;
    }
    
    public FixedJoint Grab(Rigidbody body)
    {
        if (GrabbedFixedJoint)
        {
            return null;
        }

        GrabbedFixedJoint = GrabAndStorage.grabAutoFullSnapLine(this.body, body, snapPointA, snapPointB, rotationPoint, inverseRotationPoint, rotationMask,
            excludingGrabLayerMask);
        Invoke(nameof(putMass), 0.1f);
        return GrabbedFixedJoint;
    }

    public void Release(FixedJoint fixedJoint, Vector3 handsPosition)
    {
        body.automaticCenterOfMass = true;
        body.excludeLayers = 0;
        body.mass = 1;
    }
}
