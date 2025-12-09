using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DemoManager : MonoBehaviour
{

    public static DemoManager Instance { get; private set; }

    public DeckManager DeckManager;
    public HandManager HandManager;
    private CardMovement CardMovement;

    public PlayerClass MainPlayer;
    public ManaClass PlayerMana;

    public GameObject loader;
    private EnemyObject currentEnemy;
    private EnemyLoader spawnEnemy;
    
    

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
       spawnEnemy = loader.GetComponent<EnemyLoader>();
       MainPlayer = FindAnyObjectByType<PlayerClass>();
       spawnEnemy.LoadEnemy(0);
       currentEnemy = spawnEnemy.CurrentEnemy;

       if (DeckManager != null && MainPlayer != null && PlayerMana != null)
        {
            DeckManager.InitializeDeckAndDrawHand();
            Debug.Log("Game initialized. Starting turn setup.");
           
        }
        else
        {
            // Debug.LogError("GameManager missing critical references (DeckManager, Player, or Mana). Game cannot start.");
        }


       
    }

    public void Update()
    {
        

    }

    public void PlayerTurn(){
        Debug.Log("Player taking turn!");
        Debug.Log(MainPlayer.get_health());
        Debug.Log(HandManager.cardsInHand[0]);
        HandManager.PlayCard(HandManager.cardsInHand[0]);

        spawnEnemy.DamageEnemy(5);
    }

    public void EnemyTurn(){
        Debug.Log("Enemy taking turn!");
        MainPlayer.set_health(MainPlayer.get_health() - currentEnemy.Attack());
    }

    public void StartDemo(){
        Debug.Log("Started Demo!!");
        
        StartCoroutine(Turns());

    }

    

    private IEnumerator Turns(){

        while((MainPlayer.get_health()) > 0 || (currentEnemy.currentHealth) > 0){
           yield return new WaitForSeconds(4);
            PlayerTurn();
            yield return new WaitForSeconds(4);
            EnemyTurn();
            yield return new WaitForSeconds(4);
        }
        
    }

    private void InitializeManagersAndState()
    {
        DeckManager = FindAnyObjectByType<DeckManager>();
        if (DeckManager == null) Debug.LogError("DeckManager not found.");

        HandManager = FindAnyObjectByType<HandManager>();
        if (HandManager == null) Debug.LogError("HandManager not found.");

        if (DeckManager != null) DeckManager.SetHandManager(HandManager);

        CardMovement = FindAnyObjectByType<CardMovement>();
        if (CardMovement == null) Debug.LogError("CardManager not found.");

        MainPlayer = FindAnyObjectByType<PlayerClass>();
        PlayerMana = FindAnyObjectByType<ManaClass>();

    }

   



}
