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

    private Animator animator;
    [HideInInspector] public bool isBurning = false;
    [HideInInspector] public bool isBurned = false;
    public PresentType type;
    public float timeBeforeBurn = 2f;
    [SerializeField] private AudioSource fireSound;

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

    public IEnumerator BurnPresent()
    {
        fireSound.Play();
        yield return new WaitForSecondsRealtime(timeBeforeBurn);

        fireSound.Stop();
        isBurned = true;
        GetComponent<BoxCollider2D>().enabled = false;
        Destroy(gameObject, 60f);
    }
}
