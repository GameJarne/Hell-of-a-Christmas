using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public SceneAsset playScene;
    public Canvas settingsCanvas;

    private void Start()
    {
        settingsCanvas.enabled = false;
    }

    public void OnPlayButton()
    {
        SceneManager.LoadSceneAsync(playScene.name, LoadSceneMode.Single);
    }

    public void OnSettingsButton()
    {
        settingsCanvas.enabled = !settingsCanvas.enabled;
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}
