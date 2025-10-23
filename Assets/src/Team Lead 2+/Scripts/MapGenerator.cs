using UnityEngine;

/// <summary>Generates the main game map.</summary>
public class MapGenerator : MonoBehaviour
{
    public GameObject LevelPrefab;
    public MapCamera cam;

    // void Start()
    // {
    //     var level1 = Instantiate(LevelPrefab);
    //     level1.transform.position = new Vector3(0.129999995f, -2.36999989f, 0);
    //     var level2 = Instantiate(LevelPrefab);
    //     level2.transform.position = new Vector3(-4.61999989f, 2.54999995f, 0);
    //     var level3 = Instantiate(LevelPrefab);
    //     level3.transform.position = new Vector3(3.93000007f, 2.54999995f, 0);

    //     level1.GetComponent<Level>().AddNextLevel(level2);
    //     level1.GetComponent<Level>().AddNextLevel(level3);
    // }
}
