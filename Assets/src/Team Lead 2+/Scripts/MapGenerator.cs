using TMPro;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>Generates the main game map.</summary>
public class MapGenerator : MonoBehaviour
{
    public GameObject LevelPrefab;
    public MapCamera cam;
    public TextMeshProUGUI LevelNameText;

    void Start()
    {
        var level1 = CreateLevel(0.129999995f, -2.36999989f);
        level1.Information = new Level.Battle(0);

        var level2 = CreateLevel(-4.61999989f, 2.54999995f);
        level2.Information = new Level.Unknown(0);

        var level3 = CreateLevel(3.93000007f, 2.54999995f);
        level3.Information = new Level.Treasure(0);

        level1.AddNextLevel(level2.GameObject());
        level1.AddNextLevel(level3.GameObject());
    }

    private void UpdateCameraBounds(float ypos)
    {
        if (ypos > cam.YMax)
            cam.YMax = ypos;
        else if (ypos < cam.YMin)
            cam.YMin = ypos;
    }

    private Level CreateLevel(float x, float y)
    {
        UpdateCameraBounds(y);
        var level = Instantiate(LevelPrefab);
        level.transform.position = new Vector3(x, y, 0);
        var lvl = level.GetComponent<Level>();
        lvl.HoverEntered.AddListener(OnLevelHoverEnter);
        lvl.HoverExited.AddListener(OnLevelHoverExited);
        return lvl;
    }

    private void OnLevelHoverEnter(Level level)
    {
        LevelNameText.text = level.Information.Name();
    }
    
    private void OnLevelHoverExited()
    {
        LevelNameText.text = "<Select Level>";
    }
}
