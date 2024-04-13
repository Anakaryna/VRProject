using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class GrabPhysics : MonoBehaviour
{
    public Rigidbody body;
    [Header("INPUT ACTION")]
    public InputActionProperty grabInputSource;
    [Header("RADIUS")]
    public float radius = 0.1f;
    [Header("LAYER")]
    public LayerMask grabLayer;

    private FixedJoint fixedJoint;
    private bool isGrabbing = false;
    private IGrabbable grabedThing; // hey maybe you could do a recall mod thanks to that ?

    // Update is called once per frame
    void FixedUpdate()
    {
        bool isGrabButtonPressed = grabInputSource.action.ReadValue<float>() > 0.1f;

        if(isGrabButtonPressed && !isGrabbing)
        {
            Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, radius, grabLayer);

            if(nearbyColliders.Length > 0 && nearbyColliders[0].gameObject.TryGetComponent(out IGrabbable grabbable))
            {
                
                Rigidbody nearbyRigidbody = nearbyColliders[0].attachedRigidbody;

                fixedJoint = grabbable.Grab(body);

                isGrabbing = fixedJoint;

                grabedThing = grabbable;

            }
        }
        else if(!isGrabButtonPressed && isGrabbing)
        {
            isGrabbing = false;

            if(fixedJoint)
            {
                grabedThing.Release(fixedJoint);
                Destroy(fixedJoint);
            }
        }
    }
}
