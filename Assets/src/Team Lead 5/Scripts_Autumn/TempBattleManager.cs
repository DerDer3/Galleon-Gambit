using UnityEngine;

public class TempBattleManager : MonoBehaviour
{

    public GameObject loader;
    private EnemyObject currentEnemy;
    void Start()
    {
        EnemyLoader spawnEnemy = loader.GetComponent<EnemyLoader>(); //adjust spawn enemy health
        spawnEnemy.LoadEnemy(0);
        currentEnemy = spawnEnemy.CurrentEnemy;

        if (currentEnemy != null)
        {
            Debug.Log($"Enemy Spawned with {currentEnemy.currentHealth} health!");
        }

        spawnEnemy.DamageEnemy(5);
        
        Debug.Log($"Enemy now has {currentEnemy.currentHealth} health!");


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
