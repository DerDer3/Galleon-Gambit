using UnityEngine;

public class TempBattleManager : MonoBehaviour
{

    public GameObject loader; 
    void Start()
    {
        EnemyLoader spawnEnemy = loader.GetComponent<EnemyLoader>();
        spawnEnemy.LoadEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
