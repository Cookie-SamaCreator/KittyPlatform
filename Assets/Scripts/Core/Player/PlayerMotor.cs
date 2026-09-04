using UnityEngine;
[RequireComponent(typeof(CharacterController))]

[RequireComponent(typeof(PlayerPlatformAttachment))]
public sealed class PlayerMotor : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField, Min(0f)]
    private float moveSpeed = 3.5f;

    [SerializeField, Min(0f)]
    private float acceleration = 18f;

    [SerializeField, Min(0f)]
    private float deceleration = 22f;

    [Header("Jump")]
    [SerializeField, Min(0f)]
    private float jumpHeight = 0.6f;

    [Header("Gravity")]
    [SerializeField]
    private float gravity = -9.81f;

    [Header("Ground")]
    [SerializeField, Min(0f)]
    private float groundStickSpeed = 0.05f;

    [Header("Dash")]
    private Vector3 dashVelocity;
    private float dashTimer;

    private const float MinimumDirectionSqrMagnitude = 0.001f;

    private CharacterController characterController;

    private Vector3 moveDirection;
    private Vector3 horizontalVelocity;
    private float verticalVelocity;

    private Vector3 externalMovement;

    private PlayerPlatformAttachment platformAttachment;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        platformAttachment = GetComponent<PlayerPlatformAttachment>();
    }

    private void LateUpdate()
    {
        float deltaTime = Time.deltaTime;

        UpdateHorizontalVelocity();
        UpdateDash();
        UpdateVerticalVelocity();

        Vector3 movement =
            (horizontalVelocity + dashVelocity + Vector3.up * verticalVelocity) * deltaTime +
            externalMovement;

        // External movement is consumed once, typically from a moving platform.
        externalMovement = Vector3.zero;

        characterController.Move(movement);
    }

    public void SetMoveDirection(Vector3 direction)
    {
        direction.y = 0f;

        if (direction.sqrMagnitude <= MinimumDirectionSqrMagnitude)
        {
            moveDirection = Vector3.zero;
            return;
        }

        if (direction.sqrMagnitude > 1f)
        {
            direction.Normalize();
        }

        moveDirection = direction;
    }

    public void AddExternalMovement(Vector3 movement)
    {
        externalMovement += movement;
    }

    public void Jump()
    {
        if (!characterController.isGrounded || jumpHeight <= 0f || gravity >= 0f)
        {
            return;
        }
        
        platformAttachment.Detach();
        
        verticalVelocity =
            Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    public void Dash(Vector3 direction, float speed, float duration)
    {
        direction.y = 0f;

        if (direction.sqrMagnitude < MinimumDirectionSqrMagnitude)
        {
            return;
        }

        if (speed <= 0f || duration <= 0f)
        {
            return;
        }

        direction.Normalize();

        dashVelocity = direction * speed;
        dashTimer = duration;
    }

    private void UpdateHorizontalVelocity()
    {
        Vector3 targetVelocity =
            moveDirection * moveSpeed;

        float accelerationRate =
            moveDirection.sqrMagnitude > MinimumDirectionSqrMagnitude
                ? acceleration
                : deceleration;

        horizontalVelocity = Vector3.MoveTowards(
            horizontalVelocity,
            targetVelocity,
            accelerationRate * Time.deltaTime
        );
    }

    private void UpdateVerticalVelocity()
    {
        if (characterController.isGrounded && verticalVelocity < 0f)
        {
            // Keep the controller grounded on slopes and moving platforms.
            verticalVelocity = -groundStickSpeed;
            return;
        }

        verticalVelocity += gravity * Time.deltaTime;
    }

    private void UpdateDash()
    {
        if (dashTimer <= 0f)
        {
            dashVelocity = Vector3.zero;
            return;
        }

        dashTimer -= Time.deltaTime;

        if (dashTimer <= 0f)
        {
            dashTimer = 0f;
            dashVelocity = Vector3.zero;
        }
    }
}