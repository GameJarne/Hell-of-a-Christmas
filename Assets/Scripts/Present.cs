using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Present : MonoBehaviour
{


    public enum PresentType
    {
        Red,
        Blue,
        Yellow,
        Green,
        Purple
    }

    Animator animator;
    bool isBurning = false;
    bool isBurned = false;
    [SerializeField] PresentType type;
    [SerializeField] float timeBeforeBurn = 2f;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isBurned)
        {
            if (isBurning)
            {
                animator.Play($"{type} Present Burn");
            }
            else
            {
                animator.Play($"{type} Present");
            }
        }
        else
        {
            animator.Play("Burned Present");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isBurning = true;
            StartCoroutine(BurnPresent());
        }
    }

    IEnumerator BurnPresent()
    {
        yield return new WaitForSecondsRealtime(timeBeforeBurn);

        isBurned = true;
    }
}
