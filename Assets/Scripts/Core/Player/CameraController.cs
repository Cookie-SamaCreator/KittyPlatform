using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Transform cameraTarget;

    [Header("Look")]
    [SerializeField, Min(0f)]
    private float sensitivity = 0.1f;

    [SerializeField]
    private float minPitch = -30f;

    [SerializeField]
    private float maxPitch = 70f;

    private float yaw;
    private float pitch;

    public void Look(Vector2 lookInput)
    {
        if (cameraTarget == null || lookInput == Vector2.zero)
        {
            return;
        }

        yaw += lookInput.x * sensitivity;

        // Invert vertical look so moving the mouse up looks up.
        pitch -= lookInput.y * sensitivity;

        // Keep the camera from rotating beyond the configured vertical limits.
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        cameraTarget.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}
