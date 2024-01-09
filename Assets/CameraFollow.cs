using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform boatTransform; // The boat's transform
    public float smoothSpeed = 0.125f; // How smoothly the camera catches up to its target position
    public Vector3 offset; // Offset distance between the camera and the boat

    void LateUpdate()
    {
        Vector3 desiredPosition = new Vector3(boatTransform.position.x + offset.x, transform.position.y, offset.z);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

    }
}