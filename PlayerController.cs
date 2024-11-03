using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Add this for scene management

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] public float RotationSpeed = 500f;

    [Header("Ground Check Settings")]
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] Vector3 groundCheckOffset;
    [SerializeField] LayerMask groundLayer;

    [Header("Character Controller Settings")]
    [SerializeField] float normalHeight = 2f;
    [SerializeField] Vector3 normalCenter = new Vector3(0, 1f, 0);
    [SerializeField] float slideHeight = 0.1f;
    [SerializeField] Vector3 slideCenter = new Vector3(0, 0.5f, 0);

    [Header("Next Level UI")]
    [SerializeField] GameObject nextLevelUI; // Reference to the UI GameObject
    [SerializeField] TextMeshProUGUI countDownText; // Reference to the countdown text
    [SerializeField] levelManager levelManager; // Reference to the LevelManager

    bool isGrounded;
    bool hasControl = true;
    float ySpeed;
    Quaternion targetRotation;
    bool isSliding;
    Animator animator;
    CharacterController characterController;
    CameraController cameraController;
    public AlienSoldierAnimationAI alienSoldierAnimationAI; // Reference to AlienSoldierAnimationAI
    public bool IsSliding => isSliding;
    public bool isDead = false;
    private float deathAnimationDuration = 2f;
    [SerializeField] float boxingDurationThreshold = 1.6f; // Required boxing duration to trigger death

    private float collisionTime = 0f;
    private bool isBoxingWithEnemy = false;
    private bool isDying = false;

    private void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>(); // Reference the CameraController script
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        alienSoldierAnimationAI = GetComponent<AlienSoldierAnimationAI>(); // Initialize the reference
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("finishline"))
        {
            cameraController.ShowCompletionMessage(); // Show the completion message
            levelManager levelManagerScript = FindObjectOfType<levelManager>(); // Find the levelManager in the scene
            if (levelManagerScript != null)
            {
                levelManagerScript.ShowNextLevelUI(); // Call the LoadNextLevel method
            }
        }
    }

    private void Update()
    {
        if (!hasControl)
        {
            return;
        }
        HandleMovement();
        HandleSlide();
    }

    void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Create move direction relative to the camera
        Vector3 moveInput = new Vector3(h, 0, v).normalized; // Normalize to avoid faster diagonal movement
        Vector3 moveDir = cameraController.PlanarRotation * moveInput; // Adjust movement direction based on camera

        if (!hasControl) return;

        GroundCheck();

        // Handle gravity
        if (isGrounded)
        {
            ySpeed = -0.5f; // Small downward force to keep grounded
        }
        else
        {
            ySpeed += Physics.gravity.y * Time.deltaTime; // Apply gravity
        }

        // Combine movement direction with vertical speed
        Vector3 velocity = moveDir * moveSpeed; // Calculate horizontal velocity
        velocity.y = ySpeed; // Add vertical velocity

        characterController.Move(velocity * Time.deltaTime); // Move the character controller

        // Update rotation
        if (moveInput.magnitude > 0)
        {
            targetRotation = Quaternion.LookRotation(moveDir); // Set the target rotation
        }

        // Smoothly rotate towards the target direction
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);

        // Update the animation parameter for movement
        animator.SetFloat("moveAmount", moveInput.magnitude, 0.2f, Time.deltaTime);
    }

    void HandleSlide()
    {
        if (Input.GetKey(KeyCode.E))
        {
            if (!isSliding)
            {
                isSliding = true;
                characterController.height = slideHeight;
                characterController.center = slideCenter;
                animator.SetTrigger("slide");
            }
        }
        else
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            bool isSlideAnimationPlaying = stateInfo.IsName("slide");
            bool isSlideAnimationFinished = stateInfo.normalizedTime >= 1.0f;

            if (isSliding && isSlideAnimationFinished)
            {
                isSliding = false;
                characterController.height = normalHeight;
                characterController.center = normalCenter;
                animator.SetTrigger("sliderun");
            }
        }
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundLayer);
    }

    public void SetControl(bool hasControl)
    {
        this.hasControl = hasControl;
        characterController.enabled = hasControl;
        if (!hasControl)
        {
            animator.SetFloat("moveAmount", 0f);
            targetRotation = transform.rotation;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius);
    }


    public void PlayDyingAnimation()
    {
        animator.SetTrigger("Die"); // Trigger the dying animation
    }
}
