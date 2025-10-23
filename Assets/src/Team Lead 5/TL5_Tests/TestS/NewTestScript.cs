using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class NewTestScript
{
    private CreateEnemy _spawner;
    private GameObject prefab;

    [UnitySetUp]
    public IEnumerator LoadScene()
    {
        var op = SceneManager.LoadSceneAsync("DemoBattleScene");
        while (!op.isDone)
            yield return null;
    }

    [SetUp]
    public void SetUp()
    {
        _spawner = new GameObject("Spawner").AddComponent<CreateEnemy>();
        _spawner.enemyBattleLocation = new GameObject("EnemySpawn");
        _spawner.EnemyPrefab = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _spawner.EnemyPrefab.AddComponent<Enemy>();
        _spawner.enemyHUD = new GameObject("HUD").AddComponent<EnemyHUD>();
        prefab = GameObject.CreatePrimitive(PrimitiveType.Cube);
    }

    [UnityTest]
    public IEnumerator StressSpawn_Prefabs_Continuous()
    {
        int spawnCount = 0;

        int i = 0, max = 1000;
        yield return null;
        while (spawnCount < max)
        {
            i++;
            spawnCount++;
            _spawner.onBattleStart();

            if (spawnCount % 10 == 0)
            {
                Debug.Log(spawnCount);
            }
            yield return new WaitForSeconds(0.1f);
        }

        
        
    }
}
