using Unity.VisualScripting;
using UnityEngine;
[System.Serializable]
public class EnemyData
{
    public string type;
    public string enemyName;
    
    public string enemyTitle;
    public int health;
    public int attack;
    public string imagePath;
}
[System.Serializable]
class EnemyArray
{
    public EnemyData[] enemies;

}
public class EnemyLoader : MonoBehaviour
{
    public EnemyHUD enemyHUDPrefab;  // Assign your EnemyHUD prefab
    public Transform uiParent;
    public void LoadEnemy()
    {

        TextAsset file = Resources.Load("enemies") as TextAsset;
        EnemyArray enemies = JsonUtility.FromJson<EnemyArray>(file.text);

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
    
        EnemyObject enemy = CreateEnemyFromData(enemies.enemies[0]);
        EnemyHUD ui = Instantiate(enemyHUDPrefab, uiParent);
        if (ui != null)
        {
            ui.SetUp(enemies.enemies[0]);
            ui.SetHUD(enemy);
        }


    }

    private EnemyObject CreateEnemyFromData(EnemyData data){
        switch (data.type)
        {
            case "minor":
                return new MinorEnemy(data);
            case "boss":
                return new BossEnemy(data);
            default:
                Debug.Log("Nope!");
                return new MinorEnemy(data); //fallback enemy spawning
        }
    }
        
}


