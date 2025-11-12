using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager2 : MonoBehaviour
{
    //Singleton pattern
    public static GameManager2 Instance { get; private set; }

    public DeckManager DeckManager { get; private set; }
    public HandManager HandManager { get; private set; }


    private int player_health = 100;
    private int player_mana = 0;

    private bool isGameReady = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeManagers();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (DeckManager != null)
        {
            // Call the DeckManager's initialization logic (renamed from startup() for clarity)
            DeckManager.InitializeDeckAndDrawHand();
            isGameReady = true;
        }
        else
        {
            Debug.LogError("DeckManager is NULL. Game cannot start properly.");
        }

    }


    public void Update()
    {
        // Only run update logic if the game has finished its setup (isGameReady)
        if (!isGameReady) return;

        // Centralized Update calls for continuous tasks
        if (HandManager != null)
        {
            HandManager.UpdateHandVisuals();
        }
    }

    private void InitializeManagers()
    {
        // Initialize DeckManager (as a child/prefab)
        DeckManager = GetComponentInChildren<DeckManager>();
        if (DeckManager == null)
        {
            // Existing prefab loading logic for DeckManager
            GameObject prefab = Resources.Load<GameObject>("Prefab/DeckManager");
            if (prefab == null)
            {
                Debug.Log($"DeckManager Prefab not found.");
            }
            else
            {
                Instantiate(prefab, transform.position, Quaternion.identity, transform);
                DeckManager = GetComponentInChildren<DeckManager>();
            }
        }

        HandManager = FindObjectOfType<HandManager>();
        if (HandManager == null)
        {
            Debug.LogError("HandManager not found in scene! Drawing cards will fail.");
        }
        else
        {
            HandManager.deckManager = DeckManager;
        }

        if (DeckManager != null)
        {
            DeckManager.SetHandManager(HandManager);
        }
    }

    public int Player_health
    {
        get { return player_health; }
        set { player_health = value; }
    }

    public int Player_mana
    {
        get { return player_mana; }
        set { player_mana = value; }
    }
}