using UnityEngine;

public sealed class PhysicsSynchronisator : MonoBehaviour
{
    private void LateUpdate()
    {
        Physics.SyncTransforms();
    }
}