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
        body.automaticCenterOfMass = true;
        body.excludeLayers = 0;
        body.mass = 1;
        stored = false;
        return gameObject;
    }
}
