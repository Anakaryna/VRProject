
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
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
        public static Quaternion getSnapRotation(Quaternion rotationTarget, Quaternion objectRotation, Quaternion grabberRotation, Vector3 rotationMask)
        {
            Vector3 targetRotation = (grabberRotation * rotationTarget).eulerAngles;
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
        public static FixedJoint grabAutomaticFullSnapPoint(Rigidbody grabbedBody, Rigidbody grabbingBody, Transform snapPosition, Transform snapRotation, Vector3 rotationFilter, LayerMask excludingGrabLayerMask)
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

        public static FixedJoint grabAutoFullSnapLine(Rigidbody grabbedBody, Rigidbody grabbingBody,
            Transform snapPositionA, Transform snapPositionB, Transform snapRotation, Transform snapRotationInverse, Vector3 rotationFilter,
            LayerMask excludingGrabLayerMask)
        {
            
            
            Vector3 pos = ClosestPointToLine.getClosestPointToLine(snapPositionA.localPosition, snapPositionB.localPosition,
                grabbedBody.transform.InverseTransformPoint(grabbingBody.position));
            grabbedBody.excludeLayers = excludingGrabLayerMask;
            grabbedBody.automaticCenterOfMass = false;
            grabbedBody.centerOfMass = pos;
            grabbedBody.mass = 0f;
            var fixedJoint = grabbingBody.gameObject.AddComponent<FixedJoint>();
            fixedJoint.autoConfigureConnectedAnchor = false;
            grabbedBody.isKinematic = true;

            if (Quaternion.Angle(snapRotation.rotation, grabbingBody.rotation) >
                Quaternion.Angle(snapRotationInverse.rotation, grabbingBody.rotation))
            {
                grabbedBody.transform.rotation = SnapRotation.getSnapRotation(snapRotation.localRotation, grabbedBody.transform.rotation, grabbingBody.rotation,
                    rotationFilter);
            }
            else
            {
                grabbedBody.transform.rotation = SnapRotation.getSnapRotation(snapRotationInverse.localRotation, grabbedBody.transform.rotation, grabbingBody.rotation,
                    rotationFilter);
            }
        
            fixedJoint.connectedBody = grabbedBody;
            fixedJoint.connectedAnchor = pos;
        
            grabbedBody.isKinematic = false;
            return fixedJoint;
        }

        public static Transform getClosestPosition(List<Transform> points, Transform p)
        {
            float distance = Vector3.Distance(points[0].position, p.position);
            Transform shortest = points[0];
            foreach (var point in points)
            {
                float d = Vector3.Distance(point.position, p.position);
                if (d < distance)
                {
                    distance = d;
                    shortest = point;
                }
            }

            return shortest;
        }

        public static FixedJoint storeAutomaticFullSnapPoint(Rigidbody grabbedBody, GameObject storage, Transform snapPosition, Transform pocketSnapRotation, Vector3 rotationFilter, LayerMask excludingGrabLayerMask)
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
