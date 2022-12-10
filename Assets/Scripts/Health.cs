using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Health
{
    public float health;
    public float maxHealth;
    bool isDead = false;

    public void AddHealth(float addedHealth)
    {
        health = (health + addedHealth <= maxHealth) ? health + addedHealth : maxHealth;
        isDead = (health == 0) ? true : false;
    }

    public void RemoveHealth(float removedHealth)
    {
        health = (health - removedHealth >= 0) ? health - removedHealth : 0;
        isDead = (health == 0) ? true : false;
    }

    public bool IsDead()
    {
        return isDead;
    }
}
