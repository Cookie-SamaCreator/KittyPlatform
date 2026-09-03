using UnityEngine;

public sealed class MovingLoopObject : MovingObject
{
    [Tooltip("The duration of a full loop in seconds.")]
    [SerializeField, Min(0.01f)]
    private float loopDuration = 2f;

    private float elapsedTime;

    protected override void Update()
    {
        Vector3 previousPosition = transform.position;

        elapsedTime += Time.deltaTime;

        float t = Mathf.PingPong(
            elapsedTime / (loopDuration * 0.5f),
            1f
        );

        transform.position = Vector3.Lerp(
            startPosition,
            destination,
            t
        );

        MovementDelta = transform.position - previousPosition;
    }
}