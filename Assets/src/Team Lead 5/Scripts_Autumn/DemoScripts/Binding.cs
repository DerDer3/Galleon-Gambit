using UnityEngine;
using System.Collections;

public class Binding : MonoBehaviour
{
    public GameObject loader;
    public DemoGameState mainGame;
    void Start()
    {
        DemoEnemyLoader spawnEnemy = loader.GetComponent<DemoEnemyLoader>();
        spawnEnemy.LoadEnemy();
        StartCoroutine(ExampleLoop());
    }

    private IEnumerator ExampleLoop()
    {
        //Dynamic Binding Example
        int currentHealth = mainGame.mainPlayer.get_health();
        mainGame.mainPlayer.set_health(100);
        yield return new WaitForSeconds(2f);
        DemoBossEnemy e = new DemoBossEnemy();
        int dmg = e.DealDamage();

        mainGame.mainPlayer.set_health(currentHealth - dmg);
         
         yield return new WaitForSeconds(1f);
    }



}
