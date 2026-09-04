using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [SerializeField]
    protected Vector3 offset = new(0f, 0.2f, 0f);

    [SerializeField, Min(0f)]
    protected float speed = 1f;

    protected Vector3 startPosition;
    protected Vector3 destination;

    public Vector3 MovementDelta { get; protected set; }

    private void Awake()
    {
        startPosition = transform.position;
        destination = startPosition + offset;
    }

    protected virtual void Update()
    {
        Vector3 currentPosition = transform.position;

        if (speed <= 0f || currentPosition == destination)
        {
            MovementDelta = Vector3.zero;
            return;
        }

        Vector3 nextPosition = Vector3.MoveTowards(
            currentPosition,
            destination,
            speed * Time.deltaTime
        );

        transform.position = nextPosition;

        // The player uses this frame delta to follow the platform.
        MovementDelta = nextPosition - currentPosition;

    }
}
