using UnityEngine;

public class DemoTempBattleManager : MonoBehaviour
{

    public GameObject loader; 
    void Start()
    {
        DemoEnemyLoader spawnEnemy = loader.GetComponent<DemoEnemyLoader>();
        spawnEnemy.LoadEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
