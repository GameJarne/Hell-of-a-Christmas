using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIDeathManager : MonoBehaviour
{
    [SerializeField] GameObject deathScreen;

    private void Awake()
    {
        deathScreen.SetActive(false);
    }

    public void OnDie()
    {
        deathScreen.SetActive(true);
    }

    public void OnRespawnButton()
    {
        deathScreen.SetActive(false);
        SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex );
    }
}
