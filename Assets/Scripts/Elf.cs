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
    [HideInInspector] public bool isImmune = false;

    [Header("References")]
    [SerializeField] BoxCollider2D waterTrigger;

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
        isImmune = true;

        allowMoving = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

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

        isImmune = false;

        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        allowMoving = true;
    }

    public IEnumerator OnPlayerAttacked()
    {
        isImmune = true;

        allowMoving = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        waterTrigger.enabled = false;

        animator.Play("Elf ISW");

        yield return new WaitForSecondsRealtime(2f);

        isImmune = false;

        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        allowMoving = true;
        waterTrigger.enabled = true;
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
