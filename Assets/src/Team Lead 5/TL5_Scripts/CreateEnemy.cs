using UnityEngine;

public class CreateEnemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject enemyBattleLocation;
    public GameObject EnemyPrefab;
    Enemy currentEnemy;
    public EnemyHUD enemyHUD;

    void Start()
    {
        // can move this to wherever we put 'battle start'
        onBattleStart();
    }

    // Update is called once per frame
    void Update()
    {
        //EnemyTurn();
        //Testing for enemy. Kills it instantly. 
    }

    void onBattleStart()
    {

        GameObject enemySpawn = Instantiate(EnemyPrefab, enemyBattleLocation.transform.position, Quaternion.identity);
        currentEnemy = enemySpawn.GetComponent<Enemy>();
        currentEnemy.maxHealth = 10;
        currentEnemy.enemyName = "James";
        currentEnemy.currentHealth = currentEnemy.maxHealth;
        currentEnemy.damage = 1;
        enemyHUD.SetHUD(currentEnemy);
        Debug.Log("Enemy Spawned~");

    }

    void EnemyTurn()
    {
        //in player attack script, can do bool isDead = currentEnemy.TakeDamage([player_attack])
        bool isDead = currentEnemy.TakeDamage(1);
        enemyHUD.UpdateHealth(currentEnemy.currentHealth);
        if (isDead)
        {
            //end battle
            currentEnemy.isDead = true;
            Debug.Log("enemy died");
        }
        else
        {
            //take turn
            //player will take "currentEnemy.damage" amt of damage
            Debug.Log("enemy attack");
        
        }
    }
    
    
}
