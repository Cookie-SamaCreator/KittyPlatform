using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Destructive : MonoBehaviour
{
    private Rigidbody destructiveBody;

    private void Awake()
    {
        destructiveBody = GetComponent<Rigidbody>();

        // The trigger volume should not be affected by physics.
        destructiveBody.isKinematic = true;
        destructiveBody.useGravity = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        var destructible = other.GetComponentInParent<Destructible>();
        if (destructible == null)
        {
            return;
        }

        destructible.Destruct();
    }
}
