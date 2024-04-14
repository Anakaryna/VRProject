using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ContinuousMovementPhysics : MonoBehaviour
{
    [Header("PERSO")]
   public float speed = 1;
    public float turnSpeed = 60;
    public float jumpHeight = 1.5f;

    private float jumpVelocity = 2;
  
    public bool onlyMoveIfGrounded = false;
    public bool jumpWithHand = true;

    public float minJumpWithHandSpeed = 2;
    public float maxJumpWithHandSpeed = 7;
    
    [Header("INPUT ACTION")]
    public InputActionProperty moveInputSource;
    public InputActionProperty turnInputSource;
    public InputActionProperty jumpInputSource;
    
    [Header("RIGID BODY")]
    public Rigidbody rb;
    public Rigidbody leftHandRB;
    public Rigidbody rightHandRB;
    public float heightWhenJumping = 2;
    
    [Header("LAYER")]
    public LayerMask groundLayer;
    
    [Header("CAMERA")]
    public Transform directionSource;
    public Transform turnSource;
    
    [Header("COLLIDER")]
    public CapsuleCollider bodyCollider;
    
    [Header("RUN")]
    public bool runWithHands = true;
    public float minRunWithHandsSpeed = 1;
    public float maxRunWithHandsSpeed = 5;
    public float runMultiplier = 2; 

    private bool isRunning = false;
    private float baseSpeed;

    private PhysicRig ph;
    private Vector2 inputMoveAxis;
    private float inputTurnAxis;
    private bool isGrounded;
    
    private void Start()
    {
        baseSpeed = speed; 
    }

    // Update is called once per frame
    void Update()
    {
        inputMoveAxis = moveInputSource.action.ReadValue<Vector2>();
        inputTurnAxis = turnInputSource.action.ReadValue<Vector2>().x;
        ph = GetComponent<PhysicRig>();

        bool inputJump = jumpInputSource.action.WasPressedThisFrame();
        
        if (runWithHands)
        {
            RunWithHandsCheck();
        }

        if (!jumpWithHand)
        {
            if (inputJump && isGrounded)
            {
                jumpVelocity = Mathf.Sqrt(2 * -Physics.gravity.y * jumpHeight);
                rb.velocity += Vector3.up * jumpVelocity;
            }
        }
        else
        {
            bool inputJumpPressed = jumpInputSource.action.IsPressed();

            float handSpeed = ((leftHandRB.velocity - rb.velocity).magnitude + (rightHandRB.velocity - rb.velocity).magnitude) / 2;

            if (inputJumpPressed && isGrounded && handSpeed > minJumpWithHandSpeed)
            {
                rb.velocity = Vector3.up * Mathf.Clamp(handSpeed, minJumpWithHandSpeed, maxJumpWithHandSpeed);
            }
        }
    }

    private void FixedUpdate()
    {
        isGrounded = CheckIfGrounded();

        if (!onlyMoveIfGrounded || (onlyMoveIfGrounded && isGrounded))
        {
            Quaternion yaw = Quaternion.Euler(0, directionSource.eulerAngles.y, 0);
            Vector3 direction = yaw * new Vector3(inputMoveAxis.x, 0, inputMoveAxis.y);

            Vector3 targetMovePosition = rb.position + direction * Time.fixedDeltaTime * speed;

            Vector3 axis = Vector3.up;
            float angle = turnSpeed * Time.fixedDeltaTime * inputTurnAxis;

            Quaternion q = Quaternion.AngleAxis(angle, axis);

            rb.MoveRotation(rb.rotation * q);

            Vector3 newPosition = q * (targetMovePosition - turnSource.position) + turnSource.position;

            rb.MovePosition(newPosition);
        }

        if (!isGrounded)
        {
            
            ph.bodyHeightMax = 1;
        }
        else
        {
            ph.bodyHeightMax = heightWhenJumping;
        }
    }

    public bool CheckIfGrounded()
    {
        Vector3 start = bodyCollider.transform.TransformPoint(bodyCollider.center);
        float rayLength = bodyCollider.height / 2 - bodyCollider.radius + 0.05f;

        bool hasHit = Physics.SphereCast(start, bodyCollider.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);

        return hasHit;
    }
    
     void RunWithHandsCheck()
    {
        float leftHandVerticalSpeed = leftHandRB.velocity.y - rb.velocity.y;
        float rightHandVerticalSpeed = rightHandRB.velocity.y - rb.velocity.y;

        bool handsMovingAlternately = (leftHandVerticalSpeed > 0 && rightHandVerticalSpeed < 0) || (leftHandVerticalSpeed < 0 && rightHandVerticalSpeed > 0);
        float averageVerticalHandSpeed = (Mathf.Abs(leftHandVerticalSpeed) + Mathf.Abs(rightHandVerticalSpeed)) / 2;

        bool wasRunning = isRunning; 
        isRunning = handsMovingAlternately && averageVerticalHandSpeed > minRunWithHandsSpeed;

        if (isRunning != wasRunning) 
        {
            if (isRunning)
            {
                speed = baseSpeed * runMultiplier; 
            }
            else
            {
                speed = baseSpeed; 
            }
        }
    }

}
