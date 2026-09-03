using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField]
    private Transform cameraRoot;
    
    [SerializeField, Min(0f)]
    private float lookSensitivity = 0.1f;

    [SerializeField]
    private float minCameraPitch = -30f;

    [SerializeField]
    private float maxCameraPitch = 70f;

    [Header("Dash")]
    [SerializeField, Min(0f)]
    private float dashCooldown = 1f;

    [SerializeField, Min(0f)]
    private float dashDuration = 0.5f;

    [SerializeField, Min(0f)]
    private float dashSpeed = 10f;

    [SerializeField]
    private GameObject dashHitbox;

    private PlayerInput playerInput;
    private PlayerMotor motor;

    private Vector2 moveInput;
    private float cameraPitch;

    private float dashCooldownTimer;
    
    private void Awake()
    {
        motor = GetComponent<PlayerMotor>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        dashHitbox.SetActive(false);
    }

    private void Update()
    {
        dashCooldownTimer -= Time.deltaTime;

        UpdateMovementDirection();
    }

    private void OnEnable()
    {
        playerInput.onActionTriggered += OnActionTriggered;
    }

    private void OnDisable()
    {
        playerInput.onActionTriggered -= OnActionTriggered;
    }

    private void OnActionTriggered(InputAction.CallbackContext context)
    {
        if(context.action.actionMap.name != "Player")
        {
            return;
        }

        if(context.action.name == "Move")
        {
            moveInput = context.ReadValue<Vector2>();
        }
        else if(context.action.name == "Jump")
        {
            OnJump(context);
        }
        else if(context.action.name == "Look")
        {
            OnLook(context);
        }
        else if(context.action.name == "Dash")
        {
            OnDash(context);
        }
    }

    public void UpdateMovementDirection()
    {
        Vector3 forward = cameraRoot.forward;
        Vector3 right = cameraRoot.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 direction = forward * moveInput.y + right * moveInput.x;

        if(direction.sqrMagnitude > 1f)
        {
            direction.Normalize();
        }

        motor.SetMoveDirection(direction);
    }

    private Vector3 GetMovementDirection()
    {
        Vector3 forward = cameraRoot.forward;
        Vector3 right = cameraRoot.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 direction = forward * moveInput.y + right * moveInput.x;

        if(direction.sqrMagnitude > 1f)
        {
            direction.Normalize();
        }

        return direction;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        motor.Jump();
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        if(input.sqrMagnitude < 0.01f)
        {
            return;
        }

        RotatePlayer(input.x);
        RotateCameraPitch(input.y);
    }

    private void RotatePlayer(float mouseX)
    {
        float yaw = mouseX * lookSensitivity;
        transform.Rotate(0f, yaw, 0f, Space.Self);
    }

    private void RotateCameraPitch(float mouseY)
    {
        cameraPitch -= mouseY * lookSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, minCameraPitch, maxCameraPitch);

        cameraRoot.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        if(!context.performed || dashCooldownTimer > 0f)
        {
            return;
        }  

        Vector3 direction = GetMovementDirection();

        if(direction.sqrMagnitude <= 0.01f)
        {
            direction = transform.forward;
        }

        motor.Dash(direction,dashSpeed, dashDuration);
        StartCoroutine(DashCoroutine(dashDuration));
        dashCooldownTimer = dashCooldown;
    }

    private IEnumerator DashCoroutine(float duration)
    {
        dashHitbox.SetActive(true);

        yield return new WaitForSeconds(duration);

        dashHitbox.SetActive(false);
    }
}
