using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager2 : MonoBehaviour
{
    //Singleton pattern
    public static GameManager2 Instance { get; private set; }

    private int player_health;

    private int player_mana;

    private int player_xp;

    //Subject to change
    private int diff = 5;

    public DeckManager DeckManager { get; private set; }

    //For Kevin:
    // public AudioManager AudioManager {get; private set;}

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

    private void InitializeManagers()
    {
        DeckManager = GetComponentInChildren<DeckManager>();
        //AudioManager = GetComponentInChildren<DeckManager>();

        if(DeckManager == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefab/DeckManager");
            if(prefab == null)
            {
                Debug.Log($"DeckManager Prefab not found.");
            }
            else
            {
                Instantiate(prefab, transform.position, Quaternion.identity, transform);
                DeckManager = GetComponentInChildren<DeckManager>();
            }
        }
    }

    public int Player_health
    {
        get { return player_health; }
        set {  player_health= value; }
    }

    public int Player_mana
    {
        get { return player_mana; }
        set { player_mana = value; }
    }

    public int Player_xp
    {
        get { return player_xp; }
        set { player_xp = value; }
    }

    public int Diff
    {
        get { return diff; }
        set { diff = value; }
    }

}