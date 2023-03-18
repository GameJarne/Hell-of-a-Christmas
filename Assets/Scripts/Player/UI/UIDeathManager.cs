using Player;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIDeathManager : MonoBehaviour
{
    [SerializeField] PlayerStats playerStats;
    [SerializeField] PresentCollection presentCollection;

    [Space]
    [SerializeField] GameObject deathScreen;
    [SerializeField] string mainMenuScene;

    [Space]
    [SerializeField] TextMeshProUGUI causeOfDeathText;
    [SerializeField] TextMeshProUGUI highScoreText;


    private void Awake()
    {
        deathScreen.SetActive(false);
    }

    public void OnDie(string cause)
    {
        causeOfDeathText.text = cause;
        
        playerStats.SetHighScore(presentCollection.presentsCollected);
        highScoreText.text = $"High Score: {playerStats.highScore}";

        deathScreen.SetActive(true);
    }

    public void OnRespawnButton()
    {
        deathScreen.SetActive(false);
        SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex );
    }

    public void OnLeaveButton()
    {
        SceneManager.LoadSceneAsync(mainMenuScene, LoadSceneMode.Single);
    }
}
