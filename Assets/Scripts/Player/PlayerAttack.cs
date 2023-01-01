using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerAttack : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.CompareTag("Elf"))
            {
                Elf elf = other.gameObject.GetComponent<Elf>();
                if (!elf.isImmune)
                    elf.StartCoroutine(elf.OnPlayerAttacked());
            }
        }
    }
}