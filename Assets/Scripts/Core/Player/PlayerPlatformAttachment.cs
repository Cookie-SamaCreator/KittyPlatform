using UnityEngine;

public sealed class PlayerPlatformAttachment : MonoBehaviour
{
    private Transform originalParent;
    private Transform currentPlatform;

    private void Awake()
    {
        originalParent = transform.parent;
    }

    public void Attach(Transform platform)
    {
        if (platform == null || currentPlatform == platform)
        {
            return;
        }

        currentPlatform = platform;
        // Preserve the player's world position when changing parent.
        transform.SetParent(platform, true);
    }

    public void Detach()
    {
        Detach(currentPlatform);
    }

    public void Detach(Transform platform)
    {
        if (platform == null || currentPlatform != platform)
        {
            return;
        }

        currentPlatform = null;
        transform.SetParent(originalParent, true);
    }
}