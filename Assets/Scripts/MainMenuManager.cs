using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    public string playScene;
    public GameObject settingsCanvas;
    public PlayerStats playerStats;
    public TextMeshProUGUI highScoreText;

    private void Start()
    {
        playerStats.Load();
        settingsCanvas.SetActive(false);
        highScoreText.text = $"High Score: {playerStats.highScore}";
    }

    public void OnPlayButton()
    {
        SceneManager.LoadSceneAsync(playScene, LoadSceneMode.Single);
    }

    public void OnSettingsButton()
    {
        settingsCanvas.SetActive(!settingsCanvas.activeSelf);
    }

    public void OnQuitButton()
    {
        playerStats.Save();
        Application.Quit();
    }
}
