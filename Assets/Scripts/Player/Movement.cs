using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Movement : MonoBehaviour
    {
        [Header("Movement")]
        public bool allowMoving = true;
        [SerializeField] float moveSpeed = 12f;
        float horizontalMovement;
        [HideInInspector] public Rigidbody2D rb;

        [Header("Jumping")]
        [SerializeField] float jumpForce = 20f;
        bool jumped = false;
        bool waitingToJump = false;

        [Header("Ground Check")]
        [SerializeField] LayerMask groundMask;
        BoxCollider2D playerCollider;

        [Header("Animation")]
        public Animator animator;
        [SerializeField] SpriteRenderer gfxRenderer;
        bool inAir = false;

        [Header("Particles")]
        [SerializeField] ParticleSystem jumpingParticles;

        private void Start()
        {
            // Initialize variables
            rb = GetComponent<Rigidbody2D>();
            playerCollider = GetComponent<BoxCollider2D>();
        }

        private void Update()
        {
            if (!allowMoving) { return; }

            // get player input
            horizontalMovement = Input.GetAxisRaw("Horizontal");

            inAir = !IsGrounded();

            // Check if the player tried to jump right before landing on the ground;
            if (waitingToJump && !inAir)
            {
                jumped = true;
            }
            // check player jumped
            else if (!jumped && Input.GetKeyDown(KeyCode.Space))
            {
                if (inAir)
                {
                    waitingToJump = true;
                    StartCoroutine(JumpDelay());
                }
                else
                    jumped = true;
            }
        }

        IEnumerator JumpDelay()
        {
            yield return new WaitForSecondsRealtime(0.25f);

            waitingToJump = false;
        }

        public void Jump()
        {
            jumpingParticles.Play();
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        private void FixedUpdate()
        {
            if (!allowMoving) { return; }

            inAir = !IsGrounded();
            UpdateAnimations();

            // move the player
            rb.velocity = new Vector2(horizontalMovement * moveSpeed, rb.velocity.y);

            if (jumped)
            {
                jumped = false;
                Jump();
            }
        }

        bool IsGrounded()
        {
            return Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.down, .1f, groundMask);
        }

        public void UpdateAnimations() // makes sure player animations are updated with what's happening
        {
            if (horizontalMovement < 0)
            {
                gfxRenderer.flipX = false;
            }
            else if (horizontalMovement > 0)
            {
                gfxRenderer.flipX = true;
            }

            // UPDATE MOVING ANIMATION + ROTATION FOR PLAYER
            if (!inAir)
            {
                if (horizontalMovement != 0)
                {
                    animator.Play("Player Walk");
                    return;
                }
                else
                {
                    animator.Play("Player Idle");
                }
            }
            else
            {
                // UPDATE JUMPING ANIMATION
                if (rb.velocity.y > 1.5f)
                {
                    animator.Play("Player Jump");
                    return;
                }
                else if (rb.velocity.y < -1.5f) // UPDATE FALLING ANIMATION
                {
                    animator.Play("Player Fall");
                    return;
                }
                else // PLAY IDLE ANIMATION
                {
                    animator.Play("Player Idle");
                }
            }
        }
    }
}