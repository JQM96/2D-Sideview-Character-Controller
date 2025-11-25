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

    Rigidbody2D rb;
    Vector2 inputVector;

    bool canJump;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        canJump = Physics2D.Raycast(groundCheckOrigin.position, Vector2.down, 0.1f, whatIsGround);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(inputVector.x * speed, rb.linearVelocityY);
    }

    public void Move(InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (canJump == false)
            return;

        if (context.phase == InputActionPhase.Performed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, 0);
            rb.AddForce(new Vector2(rb.linearVelocityX, jumpForce), ForceMode2D.Impulse);
        }
    }
}
