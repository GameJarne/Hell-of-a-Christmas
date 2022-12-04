using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 12f;
    float horizontalMovement;
    Rigidbody2D rb;

    [Header("Jumping")]
    [SerializeField] float jumpForce = 20f;
    bool jumped = false;

    [Header("Ground Check")]
    [SerializeField] LayerMask groundMask;
    BoxCollider2D playerCollider;

    [Header("Animation")]
    [SerializeField] Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");

        if (!jumped && Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            jumped = true;
        }
    }

    private void FixedUpdate()
    {
        // animation : check if player is moving : if so, which direction
        if (rb.velocity.x < 0)
        {
            transform.localRotation = new Quaternion(0, 0, 0, 0);
            animator.SetBool("isWalking", true);
        }
        else if (rb.velocity.x > 0)
        {
            transform.localRotation = new Quaternion(0, 180, 0, 0);
            animator.SetBool("isWalking", true);
        } else
        {
            animator.SetBool("isWalking", false);
        }

        // other stuff
        rb.velocity = new Vector2(horizontalMovement * moveSpeed, rb.velocity.y);

        if (jumped)
        {
            jumped = false;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    bool IsGrounded()
    {
        return Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.down, .1f, groundMask);
    }
}
