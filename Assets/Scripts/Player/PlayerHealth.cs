using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// NOTE : when taking damage must do 0.20 for 1 heart gone; so just take the damage amount / 2

namespace Player
{
    public class PlayerHealth : MonoBehaviour
    {
        [Header("Health")]
        [SerializeField] Health health = new Health();
        [SerializeField] float waterDamage = 1.0f;
        [SerializeField] float elfDamage = 1.0f;

        bool allowTakingDamage = true;
        bool allowJumpOutWater = false;

        [Header("References")]
        [SerializeField] Movement movement;
        [SerializeField] List<HealthIcon> healthIcons = new List<HealthIcon>();
        [SerializeField] UIDeathManager uiDeathManager;

        [Header("Dying")]
        bool allowDying = true;
        [SerializeField] Sprite deadSprite;
        [SerializeField] Sprite normalSprite;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Water") && allowTakingDamage && allowDying)
            {
                allowTakingDamage = false;
                print("is in water");

                health.RemoveHealth(waterDamage);

                if (!health.IsDead())
                    StartCoroutine(TakeWaterDamage());
            } else if (other.CompareTag("Water") && allowJumpOutWater)
            {
                movement.rb.velocity = Vector2.zero;
                movement.rb.angularVelocity = 0;
                movement.Jump();
            }

            if (other.CompareTag("Elf Water") && allowTakingDamage && allowDying)
            {
                allowTakingDamage = false;
                print("hit an elf's water");

                health.RemoveHealth(elfDamage);

                if (!health.IsDead())
                {
                    StartCoroutine(TakeElfDamage());
                    Elf elf = other.transform.parent.GetComponent<Elf>();
                    elf.StartCoroutine(elf.OnAttackedPlayer());
                }
            }
        }

        private IEnumerator TakeWaterDamage()
        {
            health.RemoveHealth(waterDamage);

            movement.allowMoving = false;
            movement.rb.velocity = (movement.rb.velocity.y > 0) ? new Vector2(0f, 0f) : new Vector2(0f, movement.rb.velocity.y);
            movement.animator.Play("Player Baby");
            allowDying = false;
            SetHealthIcons(true);

            if (!health.IsDead())
            {
                yield return new WaitForSecondsRealtime(1.5f);

                SetHealthIcons(false);
                movement.Jump();
                movement.allowMoving = true;
                allowJumpOutWater = true;

                yield return new WaitForSecondsRealtime(1f);

                allowDying = true;
                allowTakingDamage = true;
                allowJumpOutWater = false;
            }
            else
            {
                Die();
                yield break;
            }
        }

        private IEnumerator TakeElfDamage()
        {
            health.RemoveHealth(elfDamage);

            movement.allowMoving = false;
            movement.rb.velocity = (movement.rb.velocity.y > 0) ? new Vector2(0f, 0f) : new Vector2(0f, movement.rb.velocity.y);
            movement.animator.Play("Player Baby");
            allowDying = false;
            SetHealthIcons(true);

            if (!health.IsDead())
            {
                yield return new WaitForSecondsRealtime(1.5f);

                SetHealthIcons(false);
                movement.Jump();
                movement.allowMoving = true;

                yield return new WaitForSecondsRealtime(0.3f);

                allowDying = true;
                allowTakingDamage = true;
            }
            else
            {
                Die();
                yield break;
            }
        }

        void Die()
        {
            uiDeathManager.OnDie();
        }

        private void SetHealthIcons(bool dead)
        {
            foreach (HealthIcon icon in healthIcons)
            {
                if (icon.index <= health.health)
                {
                    icon.icon.color = new Color(255f, 255f, 255f, 1f);
                    icon.icon.sprite = (dead) ? deadSprite : normalSprite;
                } else
                {
                    // icon.icon.gameObject.SetActive(false);
                    icon.icon.color = new Color(0f, 0f, 0f, 0.25f);
                }
            }
        }

        [System.Serializable]
        public class HealthIcon
        {
            public Image icon;
            public int index;
        }
    }
}