using TMPro;
using UnityEngine;

/// <summary>Stress tests the map by creating levels until something happens.</summary>
public class MapStressTest : MonoBehaviour
{
    public GameObject LevelPrefab;
    public MapCamera cam;
    public TextMeshProUGUI StressTestText;

    private int numLevels;
    private GameObject root;

    void Start()
    {
        cam.YMin = -10;
        cam.YMax = 10;
        root = Instantiate(LevelPrefab);
    }

    private void Update()
    {
        for (int i = 0; i <= Time.timeSinceLevelLoad; i++)
        {
            StressTest();
        }
    }

    private void StressTest()
    {
        var lvl = Instantiate(LevelPrefab);
        root.GetComponent<Level>().AddNextLevel(lvl);
        lvl.transform.position = new Vector3(Random.Range(-10f, 10f), Random.Range(-15f, 15f), 0);

        var fps = (int)(1f / Time.unscaledDeltaTime);
        StressTestText.text = $"Count: {++numLevels}\t FPS: {fps}";
    }
}
