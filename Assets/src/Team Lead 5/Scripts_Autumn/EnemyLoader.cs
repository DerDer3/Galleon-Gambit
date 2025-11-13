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
public class EnemyArray
{
    public EnemyData[] enemies;

}
public class EnemyLoader : MonoBehaviour
{

    
    public EnemyHUD enemyHUDPrefab;  // Assign your EnemyHUD prefab
    public Transform uiParent;

    private EnemyObject currentEnemy;
    public EnemyObject CurrentEnemy
    {
        get { return currentEnemy; }
    }
    public void LoadEnemy()
    {

        TextAsset file = Resources.Load("enemies") as TextAsset;
        EnemyArray enemies = JsonUtility.FromJson<EnemyArray>(file.text);
        

            if (enemies == null)
            {
                Debug.LogError("EnemyArray failed to deserialize!");
                return;
            }

            if (enemies.enemies == null || enemies.enemies.Length == 0)
            {
                Debug.LogError("No enemies found in JSON!");
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
    
        currentEnemy = CreateEnemyFromData(enemies.enemies[0]);
        EnemyHUD ui = Instantiate(enemyHUDPrefab, uiParent);
        if (ui != null)
        {
            ui.SetUp(enemies.enemies[0]);
            ui.SetHUD(currentEnemy);
        }


    }

    private EnemyObject CreateEnemyFromData(EnemyData data)
    {
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

    public void DamageEnemy(int amt)
    {
        if (currentEnemy != null)
        {
            currentEnemy.TakeDamage(amt);
            EnemyHUD hud = uiParent.GetComponentInChildren<EnemyHUD>();
            if (hud != null)
                hud.UpdateHealth();
            
        }
    }
    
}


