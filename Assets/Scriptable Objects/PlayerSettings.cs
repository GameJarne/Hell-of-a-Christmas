using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Settings", menuName = "Player/Player Settings")]
public class PlayerSettings : ScriptableObject
{
    public int maxPresents = 5;
    public bool postProcessing = true;
    public float musicVolume = 0.05f;

    [Header("Saving and Loading")]
    public string fileName = "playerSettings.json";

    public void ResetSettings()
    {
        maxPresents = 5;
        postProcessing = true;
        musicVolume = 0.05f;
        Save();
    }

    #region Saving and Loading
    public void Save()
    {
        // init
        SavedStats stats = new SavedStats();
        stats.maxPresents = maxPresents;
        stats.postProcessing = postProcessing;
        stats.musicVolume = musicVolume;

        // setup save
        string location = Application.streamingAssetsPath + "/saves";
        string fullLocation = location + "/" + fileName;
        if (!Directory.Exists(location))
            Directory.CreateDirectory(location);

        // save
        string json = JsonUtility.ToJson(stats);
        File.WriteAllText(fullLocation, json);
    }

    public void Load()
    {
        // init
        string location = Application.streamingAssetsPath + "/saves";
        string fullLocation = location + "/" + fileName;

        if (File.Exists(fullLocation))
        {
            // load
            string json = File.ReadAllText(fullLocation);
            SavedStats stats = new SavedStats();
            stats = JsonUtility.FromJson<SavedStats>(json);

            // set stats to loaded stats
            maxPresents = stats.maxPresents;
            postProcessing = stats.postProcessing;
            musicVolume = stats.musicVolume;
        }
    }

    class SavedStats
    {
        public int maxPresents = 5;
        public bool postProcessing = true;
        public float musicVolume = 0.05f;
    }
    #endregion
}
