using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMotor : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField, Min(0f)]
    private float moveSpeed = 6f;

    [SerializeField, Min(0f)]
    private float acceleration = 25f;

    [SerializeField, Min(0f)]
    private float deceleration = 30f;

    [Header("Jump")]
    [SerializeField, Min(0f)]
    private float jumpHeight = 2f;

    [Header("Dash")]
    private Vector3 dashVelocity;
    private float dashTimer;

    [SerializeField]
    private float gravity = -9.81f;

    private CharacterController characterController;

    private Vector3 moveDirection;
    private Vector3 horizontalVelocity;
    private float verticalVelocity;
    
    private MovingObject currentMovingObject;
    public bool IsGrounded => characterController.isGrounded;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void LateUpdate()
    {
        bool wasGrounded = characterController.isGrounded;

        UpdateHorizontalVelocity();
        UpdateDash();

        Vector3 platformVelocity = Vector3.zero;
        if(wasGrounded && currentMovingObject != null)
        {
            platformVelocity = currentMovingObject.MovementDelta;
        }

        UpdateVerticalVelocity(wasGrounded);

        Vector3 velocity = horizontalVelocity + dashVelocity + Vector3.up * verticalVelocity;
        
        Vector3 movement = velocity * Time.deltaTime + platformVelocity;
        characterController.Move(movement);

        if(!characterController.isGrounded)
        {
            currentMovingObject = null;
        }
    }

    public void SetMoveDirection(Vector3 direction)
    {
        direction.y = 0f;

        if(direction.sqrMagnitude > 1f)
        {
            direction.Normalize();
        }

        moveDirection = direction;
    }

    public void Jump()
    {
        if(!IsGrounded)
        {
            return;
        }

        verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    private void UpdateHorizontalVelocity()
    {
        Vector3 targetVelocity = moveDirection * moveSpeed;

        float accelerationRate = (moveDirection.sqrMagnitude > 0.001f) ? acceleration : deceleration;

        horizontalVelocity = Vector3.MoveTowards(horizontalVelocity, targetVelocity, accelerationRate * Time.deltaTime);
    }

    private void UpdateVerticalVelocity(bool wasGrounded)
    {

        if(wasGrounded && currentMovingObject != null)
        {
            verticalVelocity = 0f;
            return;
        }

        if(wasGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += gravity * Time.deltaTime;
    }

    private void UpdateDash()
    {
        if(dashTimer <= 0f)
        {
            dashVelocity = Vector3.zero;
            return;
        }

        dashTimer -= Time.deltaTime;
    }

    public void Dash(Vector3 direction, float speed, float duration)
    {
        if(direction.sqrMagnitude < 0.001f)
        {
            return;
        }

        direction.y = 0f;
        direction.Normalize();

        dashVelocity = direction * speed;
        dashTimer = duration;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.normal.y < 0.5f)
        {
            return;
        }

        currentMovingObject = hit.collider.GetComponentInParent<MovingObject>();
    }
}
