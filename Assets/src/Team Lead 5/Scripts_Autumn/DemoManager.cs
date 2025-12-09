using UnityEngine;
using System.Collections;

public class DemoManager : MonoBehaviour
{
    //public DeckManager DeckManager;
    //public HandManager HandManager;


    public PlayerClass MainPlayer;
    //public ManaClass PlayerMana;

    public GameObject loader;
    private EnemyObject currentEnemy;
    private EnemyLoader spawnEnemy;
    
    

    private void Awake()
    {
        
    }


    private void Start()
    {
       //SoundManager.Instance.play(MusicTracks.Battle);
       spawnEnemy = loader.GetComponent<EnemyLoader>();
       MainPlayer = FindAnyObjectByType<PlayerClass>();
       spawnEnemy.LoadEnemy(0);
       currentEnemy = spawnEnemy.CurrentEnemy;
       
    }

    public void Update()
    {
        

    }

    public void PlayerTurn(){
        Debug.Log("Player taking turn!");
        Debug.Log(MainPlayer.get_health());
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

   



}
