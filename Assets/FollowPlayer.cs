using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform target;  // Reference to the player's Transform
    public Vector3 offset = new Vector3(0f, 2f, -5f);  // Adjust this to set the offset from the player

    void Update()
    {
        if (target != null)
        {
            // Calculate the desired position for the camera
            Vector3 desiredPosition = target.position + offset;

            // Use SmoothDamp to smoothly interpolate between the current position and the desired position
            //Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, 0.1f);

            // Set the position of the camera
            transform.position = desiredPosition;

            // Make the camera look at the player
            transform.LookAt(target);
        }
    }
}
