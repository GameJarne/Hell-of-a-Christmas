using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

namespace Player
{
    public class PresentCollection : MonoBehaviour
    {
        [Header("Presents Collected")]
        [SerializeField] TextMeshProUGUI presentsCollectedText;
        public int presentsCollected = 0;

        [Header("UI and Icons")]
        [SerializeField] List<Image> healthIcons = new List<Image>();
        [Space]
        [SerializeField] List<PresentIcon> presentIconSprites = new List<PresentIcon>();
        [SerializeField] Image presentIcon;
        [Space]
        [SerializeField] Sprite happyHealthIcon;
        [SerializeField] Sprite normalHealthIcon;
        [SerializeField] Sprite fireHealthIcon;

        PresentSpawner presentSpawner;

        private void Start()
        {
            presentSpawner = PresentSpawner.instance;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Present"))
            {
                print("detected a present");

                Present present = other.GetComponent<Present>();
                if (present.isBurned || present.isBurning) { return; }

                if (presentSpawner != null)
                    presentSpawner.OnPresentCollected(present.transform.position);

                present.isBurning = true;
                StartCoroutine(present.BurnPresent());
                StartCoroutine(SetHealthIcons(present.timeBeforeBurn));

                Sprite newIcon = GetPresentIconSprite(present);
                if (newIcon != null) { presentIcon.sprite = newIcon; }

                presentsCollected++;
                presentsCollectedText.text = presentsCollected.ToString();
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

        public IEnumerator SetHealthIcons(float waitUntilUndo)
        {
            foreach (var icon in healthIcons)
            {
                icon.sprite = fireHealthIcon;
            }

            yield return new WaitForSecondsRealtime(waitUntilUndo * 0.75f);

            foreach (var icon in healthIcons)
            {
                icon.sprite = happyHealthIcon;
            }

            yield return new WaitForSecondsRealtime(1.0f);

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