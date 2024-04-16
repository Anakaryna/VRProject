
using UnityEngine;

namespace importedFunctions
{
    public class ClosestPointToLine
    {
        public static Vector3 getClosestPointToLine(Vector3 a, Vector3 b, Vector3 p)
        {
            // WTF WAS I THINKING ?!
            var max = Vector3.Max(a, b);
            var min = Vector3.Min(a, b);
            Vector3 now = new Vector3();
            if (p.x > max.x)
            {
                now.x = max.x;
            } else if (p.x < min.x)
            {
                now.x = min.x;
            }
            else
            {
                now.x = p.x;
            }
            if (p.y > max.y)
            {
                now.y = max.y;
            } else if (p.y < min.y)
            {
                now.y = min.y;
            }
            else
            {
                now.y = p.y;
            }
            if (p.z > max.z)
            {
                now.z = max.z;
            } else if (p.z < min.z)
            {
                now.z = min.z;
            }
            else
            {
                now.z = p.z;
            }

            return now;

        } 
    }

    public class SnapRotation
    {
        public static Quaternion getSnapRotation(Quaternion rotationTarget, Quaternion objectRotation, Quaternion handRotation, Vector3 rotationMask)
        {
            Vector3 targetRotation = (handRotation * rotationTarget).eulerAngles;
            if (rotationMask.x == 0)
            {
                targetRotation.x = objectRotation.eulerAngles.x;
            }
            if (rotationMask.y == 0)
            {
                targetRotation.y = objectRotation.eulerAngles.y;
            }
            if (rotationMask.z == 0)
            {
                targetRotation.z = objectRotation.eulerAngles.z;
            }
            return Quaternion.Euler(targetRotation);
        }
    }

    public class GrabAndStorage
    {
        public static FixedJoint grabAutomaticFullSnapPoint(Rigidbody grabbedBody, float grabbedMass, Rigidbody grabbingBody, Transform snapPosition, Transform snapRotation, Vector3 rotationFilter, LayerMask excludingGrabLayerMask)
        {
            grabbedBody.excludeLayers = excludingGrabLayerMask;
            grabbedBody.automaticCenterOfMass = false;
            grabbedBody.centerOfMass = snapPosition.localPosition;
            grabbedBody.mass = 0f;
            var fixedJoint = grabbingBody.gameObject.AddComponent<FixedJoint>();
            fixedJoint.autoConfigureConnectedAnchor = false;
            grabbedBody.isKinematic = true;
        
            Vector3 pos = grabbedBody.transform.InverseTransformPoint(snapPosition.position);
            grabbedBody.transform.rotation = SnapRotation.getSnapRotation(snapRotation.localRotation, grabbedBody.transform.rotation, grabbingBody.rotation,
                rotationFilter);
        
            fixedJoint.connectedBody = grabbedBody;
            fixedJoint.connectedAnchor = pos;
        
            grabbedBody.isKinematic = false;
            return fixedJoint;
        }

        public static FixedJoint storeAutomaticFullSnapPoint(Rigidbody grabbedBody, float grabbedMass, GameObject storage, Transform snapPosition, Transform pocketSnapRotation, Vector3 rotationFilter, LayerMask excludingGrabLayerMask)
        {
            grabbedBody.excludeLayers = excludingGrabLayerMask;
            grabbedBody.automaticCenterOfMass = false;
            grabbedBody.centerOfMass = snapPosition.localPosition;
            grabbedBody.mass = 0f;
            var fixedJoint = storage.AddComponent<FixedJoint>();
            fixedJoint.autoConfigureConnectedAnchor = false;
            grabbedBody.transform.rotation = SnapRotation.getSnapRotation(pocketSnapRotation.localRotation, grabbedBody.transform.rotation,
                storage.transform.rotation, rotationFilter);
            fixedJoint.connectedAnchor = snapPosition.localPosition;
            fixedJoint.connectedBody = grabbedBody;
            return fixedJoint;
        }
        
        
    }
    
}
