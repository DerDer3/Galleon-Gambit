using System.Collections;
using NUnit.Framework;
using UnityEditor.Experimental.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class EnemySpawnTesting
{

    private CreateEnemy _spawner;
    private GameObject prefab;

    [UnitySetUp]
    public IEnumerator LoadScene()
    {
        SceneManager.LoadScene("DemoBattleScene");
        yield return null;
    }

    [SetUp]
    public void SetUp()
    {
        _spawner = new CreateEnemy();
        prefab = GameObject.CreatePrimitive(PrimitiveType.Cube);
    }

    [UnityTest]
    public IEnumerator StressSpawn_Prefabs_Continuous()
    {
        int spawnCount = 0;

        int i = 0, max = 100;

        while (spawnCount < max)
        {
            i++;
            _spawner.onBattleStart();

            if (spawnCount % 10 == 0)
            {
                Debug.Log(spawnCount);
            }
        }

        yield return null;
        
    }

}
