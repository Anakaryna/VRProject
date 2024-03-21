using UnityEngine;

public class WallRun : MonoBehaviour
{


    [Header("camera")] 
    [SerializeField]private Camera cam;
    [SerializeField]private float fov;
    [SerializeField]private float wallrun_fov;
    [SerializeField]private float wallrun_fov_time;
    [SerializeField]private float cam_tilt;
    [SerializeField]private float cam_tilt_time;
                    public float tilt;
    
    [Header("detect")]
    [SerializeField]private float walljumpforce_hor;
    [SerializeField]private float walljumpforce_ver;
    [SerializeField]private float Wall_Dist;
    [SerializeField]private float Wallrun_check;
    [SerializeField]private float min_jump;
                    private float Horizontal;
                    private float Vertical;
   

    [Header("WallRunning")] 
    [SerializeField]private float Wallrun_Velocity;
    [SerializeField]private float gravity_force;
    [SerializeField]private float Wallrun_Time;
    [SerializeField]private float exit_time;
                    private float Wallrun_Timer;
                    private float exit_timer;
                    [SerializeField]private bool gravity;
                    private bool exit_wall;
                    private bool Wallright;
                    private bool WallLeft;
                    private RaycastHit wallleftHit; 
                    private RaycastHit wallrightHit;
                    public KeyCode jump = KeyCode.Space;


    [Header("Ref")] 
    [SerializeField]private LayerMask wall;
    [SerializeField]private Transform _transform;
                    private Rigidbody rb;
                    private PlayerMovement pm;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
        // cam = GetComponent<Camera>();
    }
    void FOV()
    {
        if(pm.wallrunning)
        {
         cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wallrun_fov, wallrun_fov_time * Time.deltaTime);
            if (WallLeft) tilt = Mathf.Lerp(tilt, -cam_tilt, cam_tilt_time * Time.deltaTime);
            if (Wallright) tilt = Mathf.Lerp(tilt, cam_tilt, cam_tilt_time * Time.deltaTime);
        }
        if(!pm.wallrunning)
        {
            tilt = Mathf.Lerp(tilt, 0, cam_tilt_time * Time.deltaTime);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, wallrun_fov_time * Time.deltaTime);
        }
    
    }
    void CheckForWall()
    {
        Wallright = Physics.Raycast(transform.position, _transform.right, out wallrightHit, Wall_Dist, wall);
        WallLeft = Physics.Raycast(transform.position, -_transform.right, out wallleftHit, Wall_Dist, wall);
        
    }

     // bool CheckForGround()
     // {
     //     return !Physics.Raycast(transform.position, Vector3.down, min_jump, ground);
     // }

     void wallrun_jump()
     {
         exit_wall = true;
         exit_timer = exit_time;
         
         Vector3 Wall_Normal = Wallright ? wallrightHit.normal : wallleftHit.normal;
         Vector3 jumpForce = transform.up * walljumpforce_hor + Wall_Normal * walljumpforce_ver;
         
         rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
         
         rb.AddForce(jumpForce,ForceMode.Impulse);

     }

     void Wallrun_Start()
     {
         pm.wallrunning = true;
         Wallrun_Timer = Wallrun_Time;
         rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
     }
     
     void Wallrun_Stop()
     {
         pm.wallrunning = false;


     }
     void Wallrun()
     {
         rb.useGravity = gravity;

         Vector3 Wall_Normal = Wallright ? wallrightHit.normal : wallleftHit.normal;
         Vector3 Wall_Forward = Vector3.Cross(Wall_Normal, transform.up);
         
         if ((_transform.forward - Wall_Forward).magnitude > (_transform.forward - -Wall_Forward).magnitude)Wall_Forward = -Wall_Forward;

         rb.AddForce(Wall_Forward * Wallrun_Velocity,ForceMode.Force);
         rb.AddForce(transform.up * gravity_force, ForceMode.Force);
     }

     void state()
     {
         Horizontal = Input.GetAxisRaw("Horizontal");
         Vertical = Input.GetAxisRaw("Vertical");

         if ((Wallright || WallLeft) && Vertical > 0 && !pm._isGrounded && !exit_wall)
         {
             if (!pm.wallrunning)Wallrun_Start();
             
             if (Wallrun_Timer > 0)Wallrun_Timer -= Time.deltaTime;

             if (Wallrun_Timer <= 0)
             {
                 exit_wall = true;
                 exit_timer = exit_time;
             }
             
             if (Input.GetKeyDown(jump)) wallrun_jump();
             
         }
         else if (exit_wall)
         {
             if (pm.wallrunning)Wallrun_Stop();
             
             if (exit_timer > 0) exit_timer -= Time.deltaTime;
             
             if (exit_timer <= 0) exit_wall = false;

         }
         else if(pm.wallrunning)
         {
             Wallrun_Stop();
         }
     }

    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        CheckForWall();
        state();
        FOV();
    }

    private void FixedUpdate()
    {
        if (pm.wallrunning)Wallrun();
    }
}