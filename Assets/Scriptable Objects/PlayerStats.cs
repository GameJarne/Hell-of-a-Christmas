using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Stats", menuName = "Player/Player Stats")]
public class PlayerStats : ScriptableObject
{
    public int highScore = 0; // highest score of presents collected

    [Header("Saving and Loading")]
    public string fileName = "playerStats.json";

    public void SetHighScore(int newScore) // sets highscore if new score is higher
    {
        highScore = (newScore > highScore) ? newScore : highScore;
        Save();
    }

    public void ResetStats()
    {
        highScore = 0;
        Save();
    }

    #region Saving and Loading
    public void Save()
    {
        // init
        SavedStats stats = new SavedStats();
        stats.highScore = highScore;

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
            highScore = stats.highScore;
        }
    }

    class SavedStats
    {
        public int highScore;
    }
    #endregion
}
