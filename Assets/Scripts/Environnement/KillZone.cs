using UnityEngine;

public sealed class KillZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<PlayerController>(out var player))
        {
            return;
        }

        Debug.Log("Player died");
    }
}