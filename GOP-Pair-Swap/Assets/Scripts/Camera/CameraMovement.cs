using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Vector3 offset = new Vector3(0f, 1f, -10f); // Camera offset (Z = -10 for 2D)
    private float smoothMovementSpeed = 5f;       // Higher = faster camera snap

    void LateUpdate()
    {
        if (player == null) return;

        // Target camera position
        Vector3 desiredPosition = new Vector3(
            transform.position.x,               // Set to player.position.x if following x movement is desired
            player.position.y + offset.y,       // Smooth follow on Y
            offset.z                            // Keep Z at -10 for 2D
        );

        // Smoothly move the camera
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothMovementSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}