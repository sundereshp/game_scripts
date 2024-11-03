using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Parkour System/New parkour action")]
public class ParkourAction : ScriptableObject
{
    [SerializeField] string animName;
    [SerializeField] string obstacleTag;

    [SerializeField] float minHeight;
    [SerializeField] float maxHeight;

    [SerializeField] bool rotateToObstacle;
    [SerializeField] float postActionDelay;

    [Header("Target Matching")]
    [SerializeField] bool enableTargetMatching = true;
    [SerializeField] AvatarTarget matchBodyPart;
    [SerializeField] float matchStartTime;
    [SerializeField] float matchTargetTime;
    [SerializeField] Vector3 matchPosWeight = new Vector3(0, 1, 0);
    [SerializeField] float normalHeight = 2f;
    [SerializeField] Vector3 normalCenter = new Vector3(0, 1f, 0);
    [SerializeField] float jumpHeight = 3f; // Added height for jump
    [SerializeField] Vector3 jumpCenter = new Vector3(0, 1.5f, 0); // Adjusted center for jump
    public Quaternion TargetRotation { get; set; }
    public Vector3 MatchPos { get; set; }

    private CharacterController characterController;

    public void Initialize(CharacterController controller)
    {
        characterController = controller; // Initialize the character controller
    }

    public bool CheckIfPossible(ObstacleHitData hitData, Transform player)
    {
        // Check Tag
        if (!string.IsNullOrEmpty(obstacleTag) && hitData.forwardHit.transform.tag != obstacleTag)
            return false;

        // Height Check
        float height = hitData.heightHit.point.y - player.position.y;
        if (height < minHeight || height > maxHeight)
            return false;

        if (rotateToObstacle)
            TargetRotation = Quaternion.LookRotation(-hitData.forwardHit.normal);

        if (enableTargetMatching)
            MatchPos = hitData.heightHit.point;

        // Adjust the character controller for the jump
        PerformJump();

        return true;
    }

    private void PerformJump()
    {
        if (characterController != null)
        {
            characterController.height = jumpHeight;
            characterController.center = jumpCenter;
            // You can also play the jump animation here
        }
    }

    public void ResetCharacterController()
    {
        if (characterController != null)
        {
            characterController.height = normalHeight;
            characterController.center = normalCenter;
        }
    }

    public string AnimName => animName;
    public bool RotateToObstacle => rotateToObstacle;
    public float PostActionDelay => postActionDelay;

    public bool EnableTargetMatching => enableTargetMatching;
    public AvatarTarget MatchBodyPart => matchBodyPart;
    public float MatchStartTime => matchStartTime;
    public float MatchTargetTime => matchTargetTime;
    public Vector3 MatchPosWeight => matchPosWeight;
}
