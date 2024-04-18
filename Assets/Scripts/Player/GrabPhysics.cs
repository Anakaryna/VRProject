using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GrabPhysics : MonoBehaviour
{
    public Rigidbody body;
    [Header("INPUT ACTION")]
    public InputActionProperty grabInputSource;
    [Header("INPUT ACTION")]
    public InputActionProperty triggerInputSource;
    [Header("RADIUS")]
    public float radius = 0.1f;
    [Header("LAYER")]
    public LayerMask grabLayer;

    private FixedJoint fixedJoint;
    private bool isGrabbing = false;
    private IGrabbable grabedThing; // hey maybe you could do a recall mod thanks to that ?

    private bool isTriggering = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        bool isGrabButtonPressed = grabInputSource.action.ReadValue<float>() > 0.1f;
        bool isTriggerPressed = triggerInputSource.action.ReadValue<float>() > 0.1f;

        if(isGrabButtonPressed && !isGrabbing)
        {
            Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, radius, grabLayer);

            bool stored;

            if (nearbyColliders.Length > 0 && nearbyColliders[0].gameObject.TryGetComponent(out IStorable storable))
            {

                if (storable.Stored)
                {
                    stored = !storable.StorageRelease();
                }
                else
                {
                    stored = false;
                }
                
                
            }
            else
            {
                stored = false;
            }
            
            if(nearbyColliders.Length > 0 && !stored && nearbyColliders[0].gameObject.TryGetComponent(out IGrabbable grabbable))
            {
                
                Rigidbody nearbyRigidbody = nearbyColliders[0].attachedRigidbody;

                fixedJoint = grabbable.Grab(body, out bool makeTransfer);

                if (makeTransfer)
                {
                    DontDestroyOnLoad(fixedJoint.connectedBody.gameObject);
                }

                isGrabbing = fixedJoint;

                grabedThing = grabbable;

            }
        }
        else if(!isGrabButtonPressed && isGrabbing)
        {
            isGrabbing = false;

            if(fixedJoint)
            {
                grabedThing.Release(fixedJoint, transform.position, out bool stored);
                if (!stored)
                {
                    SceneManager.MoveGameObjectToScene(fixedJoint.connectedBody.gameObject, SceneManager.GetActiveScene());
                }
                Destroy(fixedJoint);
            }
        }

        if (isTriggerPressed && !isTriggering)
        {
            isTriggering = true;
            if (fixedJoint.connectedBody.gameObject.TryGetComponent(out IGun gun))
            {
                gun.Fire();
            }
        }
        else if(!isTriggerPressed && isTriggering)
        {
            isTriggering = false;
        }
    }
}
