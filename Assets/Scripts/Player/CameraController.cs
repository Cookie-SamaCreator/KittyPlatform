using UnityEngine;
using UnityEngine.InputSystem;

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
        yaw += lookInput.x * sensitivity;
        pitch -= lookInput.y * sensitivity;

        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        cameraTarget.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}
