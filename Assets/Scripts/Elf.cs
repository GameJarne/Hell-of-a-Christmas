using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elf : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    Rigidbody2D rb;
    float moveDir = -1; // -1 = left ; 1 = right
    Animator animator;

    bool allowMoving = true;

    [Header("Wall Raycasting")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float rayDistance = 1f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public IEnumerator OnAttackedPlayer()
    {
        allowMoving = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0f;

        animator.Play("Elf Idle");

        yield return new WaitForSecondsRealtime(2f);

        if (moveDir == 1)
        {
            moveDir *= -1;
            transform.localRotation = new Quaternion(0, 0, 0, 0);
        }
        else
        {
            moveDir *= -1;
            transform.localRotation = new Quaternion(0, 180, 0, 0);
        }
        allowMoving = true;
    }

    private void Update()
    {
        if (allowMoving)
        {
            if (Physics2D.Raycast(transform.position, -transform.right, rayDistance, groundLayer))
            {
                moveDir *= -1;
            }

            if (rb.velocity.x != 0)
            {
                animator.Play("Elf Run");
                if (moveDir == 1)
                {
                    transform.localRotation = new Quaternion(0, 180, 0, 0);
                }
                else
                {
                    transform.localRotation = new Quaternion(0, 0, 0, 0);
                }
            }
            else
            {
                animator.Play("Elf Idle");
            }
        }
    }

    void FixedUpdate()
    {
        if (allowMoving)
        {
            float movement = moveDir * Time.deltaTime;
            rb.velocity = new Vector2(movement * speed, rb.velocity.y);
        }
    }
}
