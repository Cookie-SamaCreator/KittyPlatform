using UnityEngine;

public sealed class PlayerPlatformDetector : MonoBehaviour
{
    [SerializeField]
    private PlayerPlatformAttachment attachment;

    private void Awake()
    {
        attachment ??= GetComponentInParent<PlayerPlatformAttachment>();
    }

    private void OnTriggerEnter(Collider other)
    {
        MovingObject platform = other.GetComponentInParent<MovingObject>();

        if (platform != null && attachment != null)
        {
            attachment.Attach(platform.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        MovingObject platform = other.GetComponentInParent<MovingObject>();

        if (platform != null && attachment != null)
        {
            // Detach only from the platform that was actually left.
            attachment.Detach(platform.transform);
        }
    }
}