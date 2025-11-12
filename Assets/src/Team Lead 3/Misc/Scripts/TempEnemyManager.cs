using UnityEngine;
using TMPro;

public class EnemyManager : MonoBehaviour
{
    // Singleton pattern
    public static EnemyManager Instance { get; private set; }

    [SerializeField]
    private int maxHealth = 100;

    private int currentHealth;

    // Assign your TextMeshProUGUI component in the Inspector to this field
    [SerializeField]
    private TextMeshProUGUI healthText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // This manager is typically scene-specific, so no DontDestroyOnLoad is needed.
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth < 0)
        {
            currentHealth = 0;
            // Handle enemy defeat logic here
            Debug.Log("Enemy Defeated!");
        }
        UpdateHealthUI();
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = currentHealth.ToString();
        }
        else
        {
            Debug.LogWarning("Enemy Health TextMeshProUGUI component is not assigned.");
        }
        Debug.Log($"Enemy Health: {currentHealth}");
    }
}