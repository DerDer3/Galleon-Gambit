using UnityEngine;

public class GameManager2 : MonoBehaviour
{
    // Singleton Pattern
    public static GameManager2 Instance { get; private set; }

    [Header("Managers")]
    public DeckManager DeckManager;
    public HandManager HandManager;

    [Header("Game State References")]
    public PlayerClass MainPlayer;
    public ManaClass PlayerMana;

    // Added by Autumn - Enemy Loader and Objects
    public GameObject loader;
    private EnemyObject currentEnemy;
    private EnemyLoader spawnEnemy;
    public DemoManager demoMode;
    public bool isDemoMode = StaticClass.CrossSceneInformation;
    // Core Game State variables
    private bool isGameReady = false;
    public bool IsPlayerTurn { get; set; } = false; // Player starts turn 0 (false) or maybe needs a StartGame call; still in development/

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeManagersAndState();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

    }

    private void Start()
    {
       //SoundManager.Instance.play(MusicTracks.Battle);

       if(isDemoMode == true){
            demoMode.StartDemo();
            return;
       }

        if (DeckManager != null && MainPlayer != null && PlayerMana != null)
        {
            DeckManager.InitializeDeckAndDrawHand();
            isGameReady = true;
            Debug.Log("Game initialized. Starting turn setup.");

            // Start the first turn
            StartPlayerTurn();
        }
        else
        {
            // Debug.LogError("GameManager missing critical references (DeckManager, Player, or Mana). Game cannot start.");
        }

        //Added by Autumn - Enemy Spawner 

        spawnEnemy = loader.GetComponent<EnemyLoader>(); //adjust spawn enemy health
        //if boss level, change value to 1
        spawnEnemy.LoadEnemy(0);
        currentEnemy = spawnEnemy.CurrentEnemy;

        if (currentEnemy != null)
        {
            Debug.Log($"Enemy Spawned with {currentEnemy.currentHealth} health!");
        }
    }

    public void Update()
    {
        if (!isGameReady) return;

        CheckForGameOver();

        if (HandManager != null)
        {
            HandManager.UpdateHandVisuals();
        }
    }

    private void InitializeManagersAndState()
    {
        DeckManager = GetComponentInChildren<DeckManager>();
        if (DeckManager == null) Debug.LogError("DeckManager not found.");

        HandManager = FindAnyObjectByType<HandManager>();
        if (HandManager == null) Debug.LogError("HandManager not found.");

        if (DeckManager != null) DeckManager.SetHandManager(HandManager);

        MainPlayer = FindAnyObjectByType<PlayerClass>();
        PlayerMana = FindAnyObjectByType<ManaClass>();

    }

    // --- Turn Management Functions ---

    public void StartPlayerTurn()
    {
        IsPlayerTurn = true;
        // Reset mana and draw cards at the start of the turn
        PlayerMana.set_amount(PlayerMana.get_max_amount()); // Assuming starting mana is max mana (e.g. 3)
        Debug.Log($"Player Turn started. Mana restored to {PlayerMana.get_amount()}.");

    }

    public void EndPlayerTurn()
    {
        if (!IsPlayerTurn) return;

        IsPlayerTurn = false;
        Debug.Log("Player Turn ended. Starting Enemy Turn.");

        Invoke(nameof(StartEnemyTurn), 1.0f); // Wait 1 second before enemy turn
    }

    private void StartEnemyTurn()
    {
        // Placeholder for enemy actions (e.g., enemy attacks player)
        Debug.Log("Enemy Turn: Enemy attacks!");
        /* Commented out by Autumn - replaced with enemy attack
        MainPlayer.set_health(MainPlayer.get_health() - 10);
        */
        MainPlayer.set_health(MainPlayer.get_health() - currentEnemy.Attack());


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
            isGameReady = false;
        }
    }

}