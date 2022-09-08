using System;

using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private string title = "Monster";
    [SerializeField] private float maxHealth = 10f;

    private float currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public string GetTitle()
    {
        return title;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}
