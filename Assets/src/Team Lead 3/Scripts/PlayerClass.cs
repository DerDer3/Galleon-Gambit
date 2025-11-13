using UnityEngine;
using TMPro;

// Renamed Player (from TL2_Player.cs)
public class PlayerClass : MonoBehaviour
{
    // Changed to private fields with properties/getters for controlled access
    private int playerHealth = 100;

    public TextMeshProUGUI healthText;

    // Use PascalCase for public methods in C# convention
    public int get_health() { return playerHealth; }
    public void set_health(int x)
    {
        playerHealth = x;
        // Ensure health doesn't go below zero
        if (playerHealth < 0) playerHealth = 0;
    }

    void Update()
    {
        // Update the UI text
        if (healthText != null)
        {
            healthText.text = "" + playerHealth;
        }
    }
}