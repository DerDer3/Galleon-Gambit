using UnityEngine;
using TMPro;
using System;

public class PlayerClass : MonoBehaviour
{
    // --- DYNAMIC OBSERVER PATTERN IMPLEMENTATION ---
    public static Action<int> OnHealthChanged;

    // Changed to private fields with properties/getters for controlled access
    private int playerHealth = 100;
    private int maxHealth = 100;

    public TextMeshProUGUI healthText;

    public int get_health() { return playerHealth; }
    public void set_health(int x)
    {
        playerHealth = x;
        // Ensure health doesn't go below zero
        if (playerHealth < 0) playerHealth = 0;
        if (playerHealth > maxHealth) playerHealth = maxHealth;

        OnHealthChanged?.Invoke(playerHealth);
    }

    // Dynamic addition: Subscription logic to update the local UI text
    private void OnEnable()
    {
        OnHealthChanged += UpdateHealthTextUI;
        // Initial setup for health text when starting
        OnHealthChanged?.Invoke(playerHealth);
    }

    private void OnDisable()
    {
        OnHealthChanged -= UpdateHealthTextUI;
    }

    private void UpdateHealthTextUI(int newHealth)
    {
        if (healthText != null)
        {
            healthText.text = "" + newHealth;
        }
    }
}