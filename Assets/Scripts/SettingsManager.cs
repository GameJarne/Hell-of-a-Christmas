using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public PlayerSettings playerSettings;
    public PlayerStats playerStats;

    [Header("References")]
    public TextMeshProUGUI highScoreText;
    public Toggle postProcessingToggle;
    public Slider maxPresentsSlider;
    public Slider musicVolumeSlider;
    public TextMeshProUGUI musicVolumeText;
    public TextMeshProUGUI maxPresentsText;

    void Start()
    {
        playerSettings.Load();

        postProcessingToggle.isOn = playerSettings.postProcessing;

        maxPresentsSlider.value = playerSettings.maxPresents;
        maxPresentsText.text = $"Max Presents: {playerSettings.maxPresents}";

        MusicManager.instance.audioSource.volume = playerSettings.musicVolume;
        musicVolumeSlider.value = playerSettings.musicVolume;
        musicVolumeText.text = $"Volume: {Mathf.RoundToInt(playerSettings.musicVolume * 1000)}";
    }

    public void OnMaxPresentsSliderChanged()
    {
        playerSettings.maxPresents = Convert.ToInt32(maxPresentsSlider.value);
        maxPresentsText.text = $"Max Presents: {playerSettings.maxPresents}";
        playerSettings.Save();
    }

    public void OnMusicVolumeChanged()
    {
        playerSettings.musicVolume = musicVolumeSlider.value;
        MusicManager.instance.audioSource.volume = playerSettings.musicVolume;
        musicVolumeText.text = $"Volume: {Mathf.RoundToInt(playerSettings.musicVolume * 1000)}";
        playerSettings.Save();
    }

    public void OnTogglePostProcessing()
    {
        playerSettings.postProcessing = postProcessingToggle.isOn;
        playerSettings.Save();
    }

    public void ResetSettings()
    {
        playerSettings.ResetSettings();

        postProcessingToggle.isOn = playerSettings.postProcessing;

        maxPresentsSlider.value = playerSettings.maxPresents;
        maxPresentsText.text = $"Max Presents: {playerSettings.maxPresents}";

        MusicManager.instance.audioSource.volume = playerSettings.musicVolume;
        musicVolumeSlider.value = playerSettings.musicVolume;
        musicVolumeText.text = $"Volume: {Mathf.RoundToInt(playerSettings.musicVolume * 1000)}";
    }

    public void ResetStats()
    {
        playerStats.ResetStats();
        highScoreText.text = $"High Score: {playerStats.highScore}";
    }
}
