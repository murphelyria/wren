using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform target; // The character that the camera should follow
    public float xMin = -5.0f; // The minimum x value that the camera should be at
    public float xMax = 5.0f; // The maximum x value that the camera should be at
    public float yMin = -5.0f; // The minimum y value that the camera should be at
    public float yMax = 5.0f; // The maximum y value that the camera should be at
    public float followSpeed = 1.0f; // The speed at which the camera should follow the character

    private Vector3 smoothVelocity = Vector3.zero; // The velocity of the camera's movement
    private float smoothTime = 0.3f; // The time it takes for the camera to reach its target position

    void Update()
    {
        // Get the current position of the character
        Vector3 targetPos = target.position;

        // Clamp the x and y values of the character's position so that the camera stays within the desired bounds
        targetPos.x = Mathf.Clamp(targetPos.x, xMin, xMax);
        targetPos.y = Mathf.Clamp(targetPos.y, yMin, yMax);
        targetPos.z = -10;

        // Smoothly move the camera towards the target position
        Vector3 smoothPos = Vector3.SmoothDamp(transform.position, targetPos, ref smoothVelocity, smoothTime);

        // Interpolate between the current position of the camera and the smooth position
        Vector3 newPos = Vector3.Lerp(transform.position, smoothPos, followSpeed * Time.deltaTime);

        // Set the position of the camera to the interpolated position
        transform.position = newPos;
    }
}