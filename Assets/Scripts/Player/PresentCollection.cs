using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Player
{
    public class PresentCollection : MonoBehaviour
    {
        [Header("Presents Collected")]
        [SerializeField] TextMeshProUGUI presentsCollectedText;
        [SerializeField] int presentsCollected = 0;

        [Header("UI and Icons")]
        [SerializeField] List<Image> healthIcons = new List<Image>();
        [Space]
        [SerializeField] List<PresentIcon> presentIconSprites = new List<PresentIcon>();
        [SerializeField] Image presentIcon;
        [Space]
        [SerializeField] Sprite happyHealthIcon;
        [SerializeField] Sprite normalHealthIcon;

        private void OnTriggerEnter2D(Collider2D other)
        {
            print("HELLOOOO");
            if (other.CompareTag("Present"))
            {
                print("detected a present");

                Present present = other.GetComponent<Present>();
                if (present.isBurned || present.isBurning) { return; }

                present.isBurning = true;
                StartCoroutine(present.BurnPresent());

                Sprite newIcon = GetPresentIconSprite(present);
                if (newIcon != null) { presentIcon.sprite = newIcon; }

                presentsCollected++;
                presentsCollectedText.text = presentsCollected.ToString();

                StartCoroutine(SetHappyHealthIcons(1f));
            }
        }

        private Sprite GetPresentIconSprite(Present present)
        {
            foreach (var icon in presentIconSprites)
            {
                if (present.type == icon.type)
                {
                    print($"Present Type: {icon.type}");
                    return icon.sprite;
                }
            }
            return null;
        }

        public IEnumerator SetHappyHealthIcons(float waitUntilUndo)
        {
            foreach (var icon in healthIcons)
            {
                icon.sprite = happyHealthIcon;
            }

            yield return new WaitForSecondsRealtime(waitUntilUndo);

            foreach (var icon in healthIcons)
            {
                icon.sprite = normalHealthIcon;
            }
        }

        [System.Serializable]
        public class PresentIcon
        {
            public Sprite sprite;
            public Present.PresentType type;
        }
    }
}