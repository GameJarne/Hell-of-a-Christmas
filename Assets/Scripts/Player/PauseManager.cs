using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class PauseManager : MonoBehaviour
    {
        public string mainMenuScene;
        public GameObject pauseMenu;
        public Movement movement;
        PresentCollection presentCollection;
        bool isPaused = false;

        public PlayerStats playerStats;

        private void Start()
        {
            pauseMenu.SetActive(false);
            movement.allowMoving = true;
            presentCollection = movement.GetComponent<PresentCollection>();
            isPaused = false;
            Time.timeScale = 1.0f;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isPaused = !isPaused;
                OnPauseToggle();
            }
        }

        void OnPauseToggle()
        {
            pauseMenu.SetActive(isPaused);
            movement.allowMoving = !isPaused;
            Time.timeScale = (isPaused) ? 0f : 1f;
        }

        public void OnResumeButton()
        {
            isPaused = !isPaused;
            OnPauseToggle();
        }

        public void OnLeaveButton()
        {
            Time.timeScale = 1f;
            playerStats.SetHighScore(presentCollection.presentsCollected);
            SceneManager.LoadSceneAsync(mainMenuScene, LoadSceneMode.Single);
        }
    }
}