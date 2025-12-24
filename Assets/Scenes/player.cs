using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float runSpeedMultiplier = 2f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float dashForce = 15f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private float doubleTapTime = 0.3f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool canJump = true;
    private float lastDashTime = -999f;
    private float lastAKeyTime = -999f;
    private float lastDKeyTime = -999f;

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

        // A키 더블탭 감지
        if (keyboard.aKey.wasPressedThisFrame)
        {
            if (Time.time - lastAKeyTime < doubleTapTime)
            {
                TriggerDash(-1f);
            }
            lastAKeyTime = Time.time;
        }

        // D키 더블탭 감지
        if (keyboard.dKey.wasPressedThisFrame)
        {
            if (Time.time - lastDKeyTime < doubleTapTime)
            {
                TriggerDash(1f);
            }
            lastDKeyTime = Time.time;
        }

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

    private void TriggerDash(float direction)
    {
        if (Time.time - lastDashTime < dashCooldown) return;
        if (rb == null) return;

        rb.AddForce(Vector2.right * direction * dashForce, ForceMode2D.Impulse);
        lastDashTime = Time.time;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        canJump = true;
    }
}
