using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// NOTE : when taking damage must do 0.5 for 1 heart gone; so just take the damage amount / 2

namespace Player
{
    public class PlayerHealth : MonoBehaviour
    {
        [Header("Health")]
        [SerializeField] Health health = new Health();
        [SerializeField] float waterDamage = 1.0f;

        bool allowTakingDamage = true;

        [Header("References")]
        [SerializeField] Movement movement;
        [SerializeField] List<HealthIcon> healthIcons = new List<HealthIcon>();

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
            }
        }

        private IEnumerator TakeWaterDamage()
        {
            health.RemoveHealth(waterDamage / 2);

            movement.allowMoving = false;
            movement.animator.Play("Player Baby");
            allowDying = false;
            SetHealthIcons(true);
            yield return new WaitForSecondsRealtime(1.5f);
            SetHealthIcons(false);
            movement.allowMoving = true;
            yield return new WaitForSecondsRealtime(1f);
            allowDying = true;
            allowTakingDamage = true;
        }

        private void SetHealthIcons(bool dead)
        {
            foreach (HealthIcon icon in healthIcons)
            {
                if (icon.index <= health.health)
                {
                    icon.icon.sprite = (dead) ? deadSprite : normalSprite;
                } else
                {
                    icon.icon.gameObject.SetActive(false);
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