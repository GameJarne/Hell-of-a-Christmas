using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Stats", menuName = "Player/Player Stats")]
public class PlayerStats : ScriptableObject
{
    public int highScore = 0; // highest score of presents collected

    public void SetHighScore(int newScore) // sets highscore if new score is higher
    {
        highScore = (newScore > highScore) ? newScore : highScore;
    }
}
