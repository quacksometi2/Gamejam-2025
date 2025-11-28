using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Input")]
    public InputActionReference moveRef;
    public InputActionReference jumpRef;
    public InputActionReference sprintRef;

    [Header("Movement")]
    public float walkSpeed = 10f;
    public float sprintSpeed = 15f;
    public float acceleration = 20f;

    [Header("Jump & Gravity")]
    public float jumpHeight = 1.5f;
    public float gravity = -20f;
    public float groundedGravity = -2f;

    [Header("Wall Run")]
    public float wallRunSpeed = 9f;
    public float wallRunGravity = -5f;
    public float wallCheckDistance = 0.7f;
    public float wallRunDuration = 2.0f;
    public float wallExitCooldown = 0.05f;   // kort cooldown så man kan hoppe fra mur til mur
    public LayerMask wallMask;

    [Header("Camera Tilt")]
    public Transform cameraTransform;
    public float maxTilt = 12f;
    public float tiltSpeed = 6f;

    private CharacterController controller;
    private Vector3 velocity;
    private Vector3 inputDir;
    private float currentSpeed;

    private bool isGrounded;
    private bool isSprinting;
    private bool isWallRunning;
    private bool wallOnLeft;
    private bool wallOnRight;

    private float wallRunTimer;
    private float wallExitTimer;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction sprintAction;

    private float tiltVelocity;

    // Ny variabel til at huske sidste væg
    private Vector3 lastWallNormal;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        currentSpeed = walkSpeed;

        moveAction = moveRef ? moveRef.action : null;
        jumpAction = jumpRef ? jumpRef.action : null;
        sprintAction = sprintRef ? sprintRef.action : null;

        if (moveAction == null || jumpAction == null || sprintAction == null)
        {
            enabled = false;
            return;
        }
    }

    void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
        sprintAction.Enable();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnDisable()
    {
        moveAction?.Disable();
        jumpAction?.Disable();
        sprintAction?.Disable();
    }

    void Update()
    {
        ReadInput();
        GroundCheck();

        if (CanStartWallRun())
            TryStartWallRun();
        else if (isWallRunning && !CanContinueWallRun())
            StopWallRun();

        MovePlayer();
        HandleJumpAndGravity();
        ApplyCameraTilt();
    }

    void ReadInput()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        inputDir = (transform.right * input.x + transform.forward * input.y).normalized;
        isSprinting = sprintAction.ReadValue<float>() > 0f;
    }

    void GroundCheck()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = groundedGravity;
            lastWallNormal = Vector3.zero; // reset når vi lander
        }
    }

    bool WallCheck(out Vector3 wallNormal)
    {
        wallNormal = Vector3.zero;
        wallOnLeft = Physics.Raycast(transform.position, -transform.right, out RaycastHit hitL, wallCheckDistance, wallMask);
        wallOnRight = Physics.Raycast(transform.position, transform.right, out RaycastHit hitR, wallCheckDistance, wallMask);

        if (wallOnLeft) { wallNormal = hitL.normal; return true; }
        if (wallOnRight) { wallNormal = hitR.normal; return true; }
        return false;
    }

    bool CanStartWallRun()
    {
        if (isGrounded) return false;
        if (wallExitTimer > 0f) { wallExitTimer -= Time.deltaTime; return false; }
        if (inputDir.magnitude < 0.1f) return false;

        if (WallCheck(out Vector3 wallNormal))
        {
            // Blokér hvis det er samme væg som sidst
            if (Vector3.Dot(wallNormal, lastWallNormal) > 0.9f)
                return false;

            // Tillad lidt bevægelse ind mod væggen, men ikke direkte ind
            float dot = Vector3.Dot(inputDir, wallNormal);
            if (dot > -0.5f && dot < 0.3f)
                return true;
        }
        return false;
    }

    bool CanContinueWallRun()
    {
        if (isGrounded) return false;
        if (!WallCheck(out _)) return false;
        return wallRunTimer > 0f;
    }

    void TryStartWallRun()
    {
        if (isWallRunning) return;
        isWallRunning = true;
        wallRunTimer = wallRunDuration;

        Vector3 horizontalVel = new Vector3(velocity.x, 0f, velocity.z);
        if (horizontalVel.magnitude < wallRunSpeed)
            horizontalVel = transform.forward * wallRunSpeed;
        velocity.x = horizontalVel.x;
        velocity.z = horizontalVel.z;
    }

    void StopWallRun()
    {
        isWallRunning = false;
        wallExitTimer = wallExitCooldown;
    }

    void MovePlayer()
    {
        float targetSpeed = isWallRunning ? wallRunSpeed : (isSprinting ? sprintSpeed : walkSpeed);
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);

        Vector3 move = inputDir * currentSpeed;
        controller.Move((move + new Vector3(0f, velocity.y, 0f)) * Time.deltaTime);
    }

    void HandleJumpAndGravity()
    {
        if (isWallRunning)
        {
            wallRunTimer -= Time.deltaTime;
            velocity.y += wallRunGravity * Time.deltaTime;

            if (jumpAction.triggered)
            {
                if (WallCheck(out Vector3 wallNormal))
                {
                    // Blokér hop hvis input peger direkte ind i væggen
                    if (Vector3.Dot(inputDir, wallNormal) > 0.4f)
                        return;

                    // Hop væk fra væggen med reduceret opad kraft
                    Vector3 jumpDir = wallNormal * 1.5f + Vector3.up * 0.3f;
                    velocity = jumpDir.normalized * Mathf.Sqrt(2f * Mathf.Abs(gravity) * jumpHeight);

                    lastWallNormal = wallNormal; // gem væggen vi hoppede fra
                    StopWallRun();
                }
            }
            return;
        }

        if (isGrounded && jumpAction.triggered)
        {
            velocity.y = Mathf.Sqrt(2f * Mathf.Abs(gravity) * jumpHeight);
        }

        velocity.y += gravity * Time.deltaTime;
    }

    void ApplyCameraTilt()
    {
        if (!cameraTransform) return;

        float targetTilt = 0f;
        if (isWallRunning)
        {
            if (wallOnLeft) targetTilt = -maxTilt;
            else if (wallOnRight) targetTilt = maxTilt;
        }

        float currentZ = cameraTransform.localEulerAngles.z;
        if (currentZ > 180f) currentZ -= 360f;

        float newZ = Mathf.SmoothDampAngle(currentZ, targetTilt, ref tiltVelocity, 1f / tiltSpeed);
        cameraTransform.localEulerAngles = new Vector3(
            cameraTransform.localEulerAngles.x,
            cameraTransform.localEulerAngles.y,
            newZ
        );
    }
}



