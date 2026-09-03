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
        Vector3 previousPosition = transform.position;

        transform.position = Vector3.MoveTowards(
            transform.position,
            destination,
            speed * Time.deltaTime
        );

        MovementDelta = transform.position - previousPosition;
    }
}
