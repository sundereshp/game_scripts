using System.Collections;
using UnityEngine;
using TMPro;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform planeTarget; // Initial target for top-down view
    [SerializeField] private Transform playerTarget; // Target to follow after initial view
    [SerializeField] private float initialViewDuration = 5f; // Duration for top-down view

    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float minVerticalAngle = -45f;
    [SerializeField] private float maxVerticalAngle = 45f;
    [SerializeField] private Vector2 framingOffset;
    [SerializeField] private bool invertX;
    [SerializeField] private bool invertY;

    [Header("Zoom Settings")]
    [SerializeField] private float zoomInDistance = 2f;
    [SerializeField] private float zoomOutDistance = 15f;
    [SerializeField] private float zoomSpeed = 2f; // Speed of zoom in/out

    private PlayerController playerController;
    private float targetDistance;
    private float currentDistance;
    private Coroutine verticalAdjustmentCoroutine;

    private float rotationX;
    private float rotationY;
    private float invertXVal;
    private float invertYVal;
    private bool isFollowingPlayer = false;

    [SerializeField] private TextMeshProUGUI completionText;

    private void Start()
    {
        if (completionText != null)
        {
            completionText.gameObject.SetActive(false); // Hide the completion message at the start
        }

        playerController = FindObjectOfType<PlayerController>();
        currentDistance = zoomOutDistance; // Set initial distance for zoom out

        // Set the initial camera position to (0, 15, 0) relative to the plane and look downwards
        transform.position = planeTarget.position + new Vector3(0, 200, 0);
        transform.rotation = Quaternion.Euler(90, 0, 0); // Look directly downwards

        StartCoroutine(SwitchToPlayerTargetAfterDelay());
    }

    private void Update()
    {
        if (!isFollowingPlayer) return;

        // Calculate inverted values for rotation
        invertXVal = invertX ? -1 : 1;
        invertYVal = invertY ? -1 : 1;

        // Update rotation based on input
        rotationX += Input.GetAxis("Mouse Y") * invertYVal * rotationSpeed;
        rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);
        rotationY += Input.GetAxis("Mouse X") * invertXVal * rotationSpeed;

        Quaternion targetRotation = Quaternion.Euler(rotationX, rotationY, 0);

        // Set target distance for smooth zoom
        targetDistance = playerController.IsSliding ? zoomInDistance : zoomOutDistance;

        // Gradually adjust current distance for smooth zoom effect
        currentDistance = Mathf.Lerp(currentDistance, targetDistance, Time.deltaTime * zoomSpeed);

        // Update camera position and rotation
        Vector3 targetPosition = playerTarget.position + new Vector3(framingOffset.x, framingOffset.y);
        transform.position = targetPosition - targetRotation * new Vector3(0, 0, currentDistance);
        transform.rotation = targetRotation;
    }

    private IEnumerator SwitchToPlayerTargetAfterDelay()
    {
        yield return new WaitForSeconds(initialViewDuration);

        // After the initial view duration, start following the player
        isFollowingPlayer = true;
    }

    public void ShowCompletionMessage()
    {
        if (completionText != null)
        {
            completionText.gameObject.SetActive(true); // Show the message
            completionText.text = "You Completed!";
        }
    }

    // Added PlanarRotation property
    public Quaternion PlanarRotation => Quaternion.Euler(0, rotationY, 0);
}
