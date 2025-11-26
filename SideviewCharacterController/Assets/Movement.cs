using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float speed = 5;

    [Header("Jump")]
    [SerializeField] float jumpForce = 5;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] Transform groundCheckOrigin;
    [SerializeField] float jumpBuffer = 0.2f;
    [SerializeField] float coyoteTime = 0.2f;

    [Header("Fall speed clamp")]
    [SerializeField] float fallSpeedClamp = -10f;

    Rigidbody2D rb;
    Vector2 inputVector;

    bool canJump;
    float jumpBufferTimer;
    bool jumpButtonHeld;
    float coyoteTimeTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        canJump = Physics2D.Raycast(groundCheckOrigin.position, Vector2.down, 0.1f, whatIsGround);

        jumpBufferTimer -= Time.deltaTime;

        if (canJump == true)
            coyoteTimeTimer = coyoteTime;
        else
            coyoteTimeTimer -= Time.deltaTime;

        Debug.Log(rb.linearVelocityY);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(inputVector.x * speed, rb.linearVelocityY);

        if (coyoteTimeTimer > 0 && jumpBufferTimer > 0)
        {
            rb.linearVelocityY = jumpForce;
            jumpBufferTimer = 0;
        }

        if (jumpButtonHeld == false && rb.linearVelocityY > 0)
        {
            rb.linearVelocityY *= 0.5f;

            coyoteTimeTimer = 0;
        }

        if (rb.linearVelocityY < fallSpeedClamp)
            rb.linearVelocityY = fallSpeedClamp;
    }

    public void Move(InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            jumpBufferTimer = jumpBuffer;
            jumpButtonHeld = true;
        }

        if (context.phase == InputActionPhase.Canceled)
        {
            jumpButtonHeld = false;
        }
    }
}
