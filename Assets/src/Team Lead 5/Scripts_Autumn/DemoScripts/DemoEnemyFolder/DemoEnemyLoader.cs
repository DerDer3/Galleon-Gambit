using Unity.VisualScripting;
using UnityEngine;
[System.Serializable]
public class DemoEnemyData
{
    public string type;
    public string enemyName;
    
    public string enemyTitle;
    public int health;
    public int attack;
    public string imagePath;
}
[System.Serializable]
class DemoEnemyArray
{
    public DemoEnemyData[] enemies;

}
public class DemoEnemyLoader : MonoBehaviour
{
    public DemoEnemyHUD enemyHUDPrefab;  // Assign your EnemyHUD prefab
    public Transform uiParent;
    public void LoadEnemy()
    {

        TextAsset file = Resources.Load("enemies") as TextAsset;
        DemoEnemyArray enemies = JsonUtility.FromJson<DemoEnemyArray>(file.text);

        if (enemyHUDPrefab == null)
        {
            Debug.LogError("Enemy prefab not assigned!");
            return;
        }

        //will spawn all enemies at once
        /*
        foreach (var item in enemies.enemies)
        {
            EnemyObject enemy = CreateEnemyFromData(item);
            EnemyHUD ui = Instantiate(enemyHUDPrefab, uiParent);
            if (ui != null)
            {
                ui.SetUp(item);
                ui.SetHUD(enemy);
            }
        }
        */
        //Which we don't want... maybe for testing tho ;)
    
        DemoEnemyObject enemy = CreateEnemyFromData(enemies.enemies[0]);
        DemoEnemyHUD ui = Instantiate(enemyHUDPrefab, uiParent);
        if (ui != null)
        {
             ui.transform.localPosition = Vector3.zero; // center under parent
            ui.transform.localScale = Vector3.one; // reset scale
            ui.SetUp(enemies.enemies[0]);
            ui.SetHUD(enemy);
            Debug.Log("HUD position: " + ui.transform.position);
            Debug.Log("Enemy Loaded");
           
        }
        else
        {
            Debug.Log("Enemy Not loaded");
        }


    }

    private DemoEnemyObject CreateEnemyFromData(DemoEnemyData data){
        switch (data.type)
        {
            case "minor":
                return new DemoMinorEnemy(data);
            case "boss":
                return new DemoBossEnemy(data);
            default:
                Debug.Log("Nope!");
                return new DemoMinorEnemy(data); //fallback enemy spawning
        }
    }
        
}


