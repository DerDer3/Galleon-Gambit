using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>Generates the main game map.</summary>
public class MapGenerator : MonoBehaviour
{
    public GameObject LevelPrefab;
    public MapCamera cam;
    public TextMeshProUGUI LevelNameText;

    private readonly Vector2 separation = new(3f, 4f);
    private readonly float variation = 0.25f;

    void Start()
    {
        GenerateMap();
    }

    private void GenerateMap()
    {
        var currentY = 0f;
        var previous = new List<Level>();

        var rows = Random.Range(3, 6);
        for (int i = 0; i < rows; i++)
        {
            // Create next levels (row)
            var next = new List<Level>();
            var cols = Random.Range(1, 4);
            for (int j = 0; j < cols; j++)
            {
                var x = j * separation.x - (cols - 1) * separation.x / 2f;
                var level = CreateLevel(x + Variation(), currentY + Variation());
                if (i % 2 == 0)
                    level.Information = new Level.Battle(0);
                else level.RandomizeInformation();
                next.Add(level);
            }

            // Connect previous levels (previous row)
            if (previous.Count != 0)
            {
                foreach (var level in next)
                {
                    var pre = previous[Random.Range(0, previous.Count)];
                    pre.AddNextLevel(level.GameObject());
                    // level.AddNextLevel(next[Random.Range(0, next.Count)].GameObject());
                }
            }

            previous = next;
            currentY += separation.y;
        }

        var boss = CreateLevel(0f, currentY);
        boss.Information = new Level.Boss(0);

        // Connect previous levels (previous row)
        foreach (var level in previous)
        {
            level.AddNextLevel(boss.GameObject());
        }

        // var rows = Random.Range(3, 6);
        // var currentY = separation.y * (rows - 1);
        // var boss = CreateLevel(0f, separation.y * rows);
        // boss.Information = new Level.Boss(0);
        // var roots = new List<Level> { boss };

        // for (int i = 0; i < rows; i++)
        // {
        //     var next = new List<Level>();

        //     var cols = Random.Range(1, 4);
        //     for (int j = 0; j < cols; j++)
        //     {
        //         var x = j * separation.x - (cols-1) * separation.x / 2f;
        //         var level = CreateLevel(x + Variation(), currentY + Variation());
        //         if (i % 2 == 0)
        //             level.Information = new Level.Battle(0);
        //         else level.RandomizeInformation();
        //         next.Add(level);
        //     }

        //     currentY -= separation.y;
        // }
    }

    private float Variation()
    {
        return Random.Range(-variation, variation);
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
