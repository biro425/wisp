using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float runSpeedMultiplier = 2f;
    [SerializeField] private float jumpForce = 7f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool canJump = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (rb != null)
            rb.freezeRotation = true;
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
        FixRotation();
    }

    private void FixRotation()
    {
        if (rb != null)
            rb.angularVelocity = 0f;
    }

    private void HandleMovement()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        float moveDirection = 0f;
        float currentSpeed = moveSpeed;

        // Shift 누르면 달리기
        if (keyboard.shiftKey.isPressed)
            currentSpeed *= runSpeedMultiplier;


        if (keyboard.aKey.isPressed)
        {
            moveDirection = -1f;
            if (spriteRenderer != null)
                spriteRenderer.flipX = true;
        }
        else if (keyboard.dKey.isPressed)
        {
            moveDirection = 1f;
            if (spriteRenderer != null)
                spriteRenderer.flipX = false;
        }

        if (rb != null)
            rb.linearVelocity = new Vector2(moveDirection * currentSpeed, rb.linearVelocity.y);
    }

    private void HandleJump()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null || !keyboard.spaceKey.wasPressedThisFrame) return;

        if (canJump && rb != null)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            canJump = false;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        canJump = true;
    }
}
