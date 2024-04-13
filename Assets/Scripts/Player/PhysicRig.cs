using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PhysicRig : MonoBehaviour
{
    [Header("TRANSFORM")]
    public Transform playerHead;
    public Transform leftController;
    public Transform rightController;
    public Transform cameraPosition;
    [Header("CONFIGURABLEJOINT")]
    public ConfigurableJoint headJoint;
    public ConfigurableJoint leftHandJoint;
    public ConfigurableJoint rightHandJoint;
    [Header("COLLIDER")]
    public CapsuleCollider bodyCollider;
    [Header("SIZE")]
    public float bodyHeightMin = 0.5f;
    public float bodyHeightMax = 2;
    [Header("POCKETS")]
    public GameObject parentPockets;
    public GameObject pocket1;
    public GameObject pocket2;
    public GameObject pocket3;
    public GameObject pocket4;

    [Header("Pockets Settings")]
    [Range(0,1)] public float sideHeightMultiplier;
    [Range(0,5)] public float sideSpacing;
    [Range(0,1)] public float shoulderMultiplier;
    [Range(0,5)] public float shoulderSpacing;
    public float lag;

    // Update is called once per frame
    void FixedUpdate()
    {
        bodyCollider.height = Mathf.Clamp(playerHead.localPosition.y, bodyHeightMin, bodyHeightMax);
        bodyCollider.center = new Vector3(playerHead.localPosition.x, bodyCollider.height / 2, playerHead.localPosition.z);

        leftHandJoint.targetPosition = leftController.localPosition;
        leftHandJoint.targetRotation = leftController.localRotation;
        
        rightHandJoint.targetPosition = rightController.localPosition;
        rightHandJoint.targetRotation = rightController.localRotation;

        headJoint.targetPosition = playerHead.localPosition;

        pocket1.transform.localPosition = new Vector3(cameraPosition.localPosition.x + sideSpacing, cameraPosition.localPosition.y*sideHeightMultiplier, cameraPosition.localPosition.z);
        pocket2.transform.localPosition = new Vector3(cameraPosition.localPosition.x - sideSpacing, cameraPosition.localPosition.y*sideHeightMultiplier, cameraPosition.localPosition.z);
        pocket3.transform.localPosition = new Vector3(cameraPosition.localPosition.x + shoulderSpacing, cameraPosition.localPosition.y*shoulderMultiplier, cameraPosition.localPosition.z);
        pocket4.transform.localPosition = new Vector3(cameraPosition.localPosition.x - shoulderSpacing, cameraPosition.localPosition.y*shoulderMultiplier, cameraPosition.localPosition.z);


        Quaternion head = Quaternion.Euler(0, cameraPosition.rotation.eulerAngles.y, 0);
        Quaternion pockets = Quaternion.Euler(0, parentPockets.transform.rotation.eulerAngles.y, 0);

        float distance = Quaternion.Angle(head, pockets);

        if (distance > lag)
        {
            if (lag < 2)
            {
                parentPockets.transform.rotation = Quaternion.RotateTowards(head, pockets, 1);
            }
            else
            {
                parentPockets.transform.rotation = Quaternion.RotateTowards(head, pockets, lag-1);
            }
            
        }
    }
}
