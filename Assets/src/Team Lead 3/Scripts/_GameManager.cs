using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Needed for referencing TextMeshProUGUI in the Mana/Player components

// This replaces both _GameManager.cs and TL2_GameManager.cs
public class GameManager2 : MonoBehaviour
{
    // Singleton Pattern for easy access
    public static GameManager2 Instance { get; private set; }

    [Header("Managers")]
    public DeckManager DeckManager;
    public HandManager HandManager;

    // Player and Mana Objects (TL2's responsibilities, now accessible centrally)
    [Header("Game State References")]
    public PlayerClass MainPlayer;
    public ManaClass PlayerMana;

    // Core Game State variables
    private bool isGameReady = false;
    public bool IsPlayerTurn { get; set; } = false; // Player starts turn 0 (false) or maybe needs a StartGame call

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Removed DontDestroyOnLoad as this is likely a single scene game state manager
            InitializeManagersAndState();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (DeckManager != null && MainPlayer != null && PlayerMana != null)
        {
<<<<<<< Updated upstream
            // Call the DeckManager's initialization logic (draw initial hand)
=======
            // Call the DeckManager's initialization logic
>>>>>>> Stashed changes
            DeckManager.InitializeDeckAndDrawHand();
            isGameReady = true;
            Debug.Log("Game initialized. Starting turn setup.");

            // Start the first turn
            StartPlayerTurn();
        }
        else
        {
            Debug.LogError("GameManager missing critical references (DeckManager, Player, or Mana). Game cannot start.");
        }
    }

    public void Update()
    {
        if (!isGameReady) return;

        // This is where turn update logic would typically run:
        // 1. Check for win/loss conditions
        CheckForGameOver();

        // 2. Continuous visual updates (from original _GameManager.cs)
        if (HandManager != null)
        {
            HandManager.UpdateHandVisuals();
        }
    }

    private void InitializeManagersAndState()
    {
        // Get DeckManager and HandManager (from existing setup logic in _GameManager.cs)
        DeckManager = GetComponentInChildren<DeckManager>();
        if (DeckManager == null) Debug.LogError("DeckManager not found.");

        HandManager = FindObjectOfType<HandManager>();
        if (HandManager == null) Debug.LogError("HandManager not found.");

        if (DeckManager != null) DeckManager.SetHandManager(HandManager);

        // Get Player and Mana objects (TL2's components)
        MainPlayer = FindObjectOfType<PlayerClass>();
        PlayerMana = FindObjectOfType<ManaClass>();

        if (MainPlayer == null) Debug.LogError("Player (TL2_Player.cs) not found in scene.");
        if (PlayerMana == null) Debug.LogError("Mana (TL2_Mana.cs) not found in scene.");
    }

    // --- Turn Management Functions ---

    public void StartPlayerTurn()
    {
        IsPlayerTurn = true;
        // Reset mana and draw cards at the start of the turn
        PlayerMana.set_amount(PlayerMana.get_max_amount()); // Assuming starting mana is max mana (e.g. 3)
        Debug.Log($"Player Turn started. Mana restored to {PlayerMana.get_amount()}.");

        // Example: Draw one card at the start of the turn
        // DeckManager.DrawCardToHand();
    }

    public void EndPlayerTurn()
    {
        if (!IsPlayerTurn) return;

        IsPlayerTurn = false;
        Debug.Log("Player Turn ended. Starting Enemy Turn.");

        // Optional: Discard hand, trigger enemy action, etc.
        // For now, immediately start the next player turn for testing cycle
        Invoke(nameof(StartEnemyTurn), 1.0f); // Wait 1 second before enemy turn
    }

    private void StartEnemyTurn()
    {
        // Placeholder for enemy actions (e.g., enemy attacks player)
        Debug.Log("Enemy Turn: Enemy attacks!");
        MainPlayer.set_health(MainPlayer.get_health() - 10);

        // End enemy turn and start player turn again
        Invoke(nameof(StartPlayerTurn), 1.0f); // Wait 1 second before player turn
    }

    public bool TryPlayCard(int manaCost)
    {
        if (IsPlayerTurn && PlayerMana.get_amount() >= manaCost)
        {
            PlayerMana.set_amount(PlayerMana.get_amount() - manaCost);
            return true;
        }
        return false;
    }

    // --- Game Over Check ---

    private void CheckForGameOver()
    {
        if (MainPlayer.get_health() <= 0)
        {
            Debug.Log("Gameover: Player Health reached 0.");
            // SceneManager.LoadScene("GameOverScene"); // Re-enable once you have a scene named "GameOverScene"
            // For now, log and freeze the game manager state to prevent further actions
            isGameReady = false;
        }
        // You would also check for Win condition here (e.g., currentEnemy is defeated)
    }

    // You can remove the old Player_health and Player_mana properties from the old _GameManager.cs
    // The player state is now managed by the Player and Mana classes.
}