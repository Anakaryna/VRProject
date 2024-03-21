using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody playerBody;

	 // The normal of the ground the player is currently on
	public Vector3 groundNormal = Vector3.up;

    [Header("Running")]
	public float runSpeed = 10f;

    [Header("Sliding")]
    public float _slideVelocity;
    private bool _isSliding;

    [Header("Movement")] 
    public float moveSpeed;
    public float groundDrag;
    private Vector3 _playerMovementInput;

    [Header("Jumping")] 
    public float jumpForce;
    public float airMultiplier;
    private bool _readyToJump;
    
    [Header("WallRun")]
    private float Wallrun_Speed;
    public enum MovementState
    {
        Wallrunning,
    } 
    public MovementState state;
    public bool wallrunning;


    [Header("Ground Check")] 
    public float playerHeight;
    public LayerMask groundLayer;
    public bool _isGrounded;
	
	  void Start()
    {
        // Set the playerBody field to the Rigidbody component of the player game object
        playerBody = GetComponent<Rigidbody>();
    }

   private void Update()
{
    //Make the player quit if he press escape
    if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
        // Input
    _playerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

     if (wallrunning)state = MovementState.Wallrunning;

    if (Input.GetKey(KeyCode.Space) && _isGrounded && !_readyToJump)
    {
        _readyToJump = true;
    }

   if (Input.GetKey(KeyCode.LeftControl))
    {
        _isSliding = true;
    }
    else
    {
        _isSliding = false;
    }

}

   private void FixedUpdate()
{
    // Ground check
    // _isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayer);
    _isGrounded = Physics.CheckBox(new Vector3(transform.position.x, transform.position.y - playerHeight/2, transform.position.z), new Vector3(0.45f, 0.1f, 0.45f), transform.rotation, groundLayer);

    if (_isGrounded)
    {
        playerBody.drag = groundDrag;

        // Cast a ray down from the player's position to check for the ground normal
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 2, groundLayer))
        {	
            groundNormal = hit.normal;
        }
    }
    else
    {
        playerBody.drag = 0;
        groundNormal = Vector3.up;
    }
    
    MovePlayer();

    if (_readyToJump)
    {
        Jump();
        _readyToJump = false;
    }
}
   private void MovePlayer()
{	
    // Movement direction
    Vector3 moveVector = transform.TransformDirection(_playerMovementInput) * moveSpeed;

		if (Input.GetKey(KeyCode.LeftShift) && (wallrunning || _isGrounded))
	{
    	moveVector *= runSpeed;
	}

    if (_isSliding)
    {
        // Check if the player is on a slope
        float slopeAngle = Vector3.Angle(groundNormal, Vector3.up);
        if (slopeAngle > 10f)
        {
            // Make the movement direction conform to the slope
            moveVector = Vector3.ProjectOnPlane(moveVector, groundNormal);

            // Cast a ray from the player's position towards the ground
            Ray ray = new Ray(transform.position, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, 10, groundLayer))
            {
               // Add force to the player to make them slide along the slope
            playerBody.AddForce(moveVector, ForceMode.VelocityChange);
            }

            // Apply the slide velocity when sliding along a slope
            moveVector = groundNormal * _slideVelocity;
        }
        else
        {
            // Reduce the velocity when sliding on flat ground
            moveVector *= 0.5f;
        }
    }
    else if (!_isGrounded)
    {
        moveVector *= airMultiplier;
    }

    // Apply movement by changing rigidBody velocity
    playerBody.velocity = new Vector3(moveVector.x, playerBody.velocity.y, moveVector.z);
}

 private void Jump()
    {
        playerBody.velocity = new Vector3(playerBody.velocity.x, jumpForce * (Convert.ToInt32(_isSliding) + 1), playerBody.velocity.z);
    }
}