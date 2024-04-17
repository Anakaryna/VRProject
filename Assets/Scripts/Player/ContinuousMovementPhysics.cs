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

    private PhysicRig ph;
    private Vector2 inputMoveAxis;
    private float inputTurnAxis;
    public bool isGrounded;
    public bool wallrunning;

    // Update is called once per frame
    void Update()
    {
        inputMoveAxis = moveInputSource.action.ReadValue<Vector2>();
        inputTurnAxis = turnInputSource.action.ReadValue<Vector2>().x;
        ph = GetComponent<PhysicRig>();

        bool inputJump = jumpInputSource.action.WasPressedThisFrame();

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

            // Compute the average upward hand velocity, ignoring downward motion
            float leftHandUpwardVelocity = Mathf.Max(0, leftHandRB.velocity.y - rb.velocity.y);
            float rightHandUpwardVelocity = Mathf.Max(0, rightHandRB.velocity.y - rb.velocity.y);
            float averageUpwardHandVelocity = (leftHandUpwardVelocity + rightHandUpwardVelocity) / 2;

            // Check if both hands are contributing to the upward motion
            bool bothHandsMovingUp = leftHandUpwardVelocity > 0 && rightHandUpwardVelocity > 0;

            if (inputJumpPressed && isGrounded && bothHandsMovingUp && averageUpwardHandVelocity > minJumpWithHandSpeed)
            {
                float jumpVelocity = Mathf.Clamp(averageUpwardHandVelocity, minJumpWithHandSpeed, maxJumpWithHandSpeed);
                rb.velocity = Vector3.up * jumpVelocity;
            }
        }
    }

    private void FixedUpdate()
    {
        isGrounded = CheckIfGrounded();

        if (!onlyMoveIfGrounded && !wallrunning || (onlyMoveIfGrounded && isGrounded))
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
            ph.bodyHeightMax = 0.5f;
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
}
