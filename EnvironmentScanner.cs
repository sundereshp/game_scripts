using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentScanner : MonoBehaviour
{
    [SerializeField] Vector3 forwardRayOffset = new Vector3(0, .25f, 0);
    [SerializeField] float forwardRayLength = 0.8f;
    [SerializeField] float heightRayLength = 5;
    [SerializeField] LayerMask obstacleLayer;

    public ObstacleHitData ObstacleCheck()
    {
        var hitData = new ObstacleHitData();

        // Forward Raycast
        var forwardOrigin = transform.position + forwardRayOffset;
        hitData.forwardHitFound = Physics.Raycast(forwardOrigin, transform.forward, out hitData.forwardHit, forwardRayLength, obstacleLayer);

        // Visualize the forward ray in the Scene view
        Debug.DrawRay(forwardOrigin, transform.forward * forwardRayLength, (hitData.forwardHitFound) ? Color.red : Color.white);
        Debug.Log("Forward Ray Hit: " + hitData.forwardHit.collider.name);

        // Print the object name if the forward ray hits something
        if (hitData.forwardHitFound)
        {
            

            // Height Raycast (downward)
            var heightOrigin = hitData.forwardHit.point + Vector3.up * heightRayLength;
            hitData.heightHitFound = Physics.Raycast(heightOrigin, Vector3.down, out hitData.heightHit, heightRayLength, obstacleLayer);

            // Visualize the height ray in the Scene view
            Debug.DrawRay(heightOrigin, Vector3.down * heightRayLength, (hitData.heightHitFound) ? Color.red : Color.white);

            // Print the object name if the height ray hits something
            if (hitData.heightHitFound)
            {
                Debug.Log("Height Ray Hit: " + hitData.heightHit.collider.name);
            }
        }
        return hitData;
    }
}

public struct ObstacleHitData
{
    public bool forwardHitFound;
    public bool heightHitFound;
    public RaycastHit forwardHit;
    public RaycastHit heightHit;
}
