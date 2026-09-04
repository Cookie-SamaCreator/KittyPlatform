using UnityEngine;

public sealed class MovingLoopObject : MovingObject
{
    [Tooltip("The duration of a full loop in seconds.")]
    [SerializeField, Min(0.01f)]
    private float loopDuration = 2f;

    private float elapsedTime;

    protected override void Update()
    {
        Vector3 currentPosition = transform.position;

        if (loopDuration <= 0f || offset.sqrMagnitude <= Mathf.Epsilon)
        {
            MovementDelta = Vector3.zero;
            return;
        }

        elapsedTime = Mathf.Repeat(elapsedTime + Time.deltaTime, loopDuration);

        // Convert the loop time into a back-and-forth interpolation phase.
        float t = Mathf.PingPong(elapsedTime / loopDuration * 2f, 1f);
        Vector3 nextPosition = Vector3.Lerp(
            startPosition,
            destination,
            t
        );

        transform.position = nextPosition;
        MovementDelta = nextPosition - currentPosition;
    }
}