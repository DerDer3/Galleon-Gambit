using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>Generates the main game map.</summary>
public class MapGenerator : MonoBehaviour
{
    /// <summary>The prefab object for each level in the map.</summary>
    [SerializeField] private GameObject LevelPrefab;
    /// <summary>The map camera.</summary>
    [SerializeField] private MapCamera cam;
    /// <summary>The text that displays the currently focused level.</summary>
    [SerializeField] private TextMeshProUGUI LevelNameText;
    /// <summary>Unlocks all levels if `true`.</summary>
    [SerializeField] private bool BCMode;

    /// <summary>The separation between level rows and columns.</summary>
    private readonly Vector2 separation = new(3f, 4f);
    /// <summary>The random variation of position (for x and y) for each level.</summary>
    private readonly float variation = 0.25f;

    void Start()
    {
        GenerateMap();
    }

    public void ToggleBCMode() {
        BCMode = !BCMode;
        SoundManager.Instance.play(BCMode ? SoundEffects.Card_Win : SoundEffects.Card_Wrong);
    }

    /// <summary>Generates the game map by creating levels and connecting them.</summary>
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
                level.isSelectable = (i == 0) || BCMode;
                if (i == 0)
                    level.Information = new Level.Battle(0);
                else level.RandomizeInformation();
                next.Add(level);
            }

            // Connect previous levels (previous row)
            if (previous.Count != 0)
            {
                foreach (var level in next)
                {
                    var lvl = previous[Random.Range(0, previous.Count)];
                    lvl.AddNextLevel(level.GameObject());
                }

                foreach (var level in previous)
                {
                    if (!level.HasNextLevel())
                    {
                        var lvl = next[Random.Range(0, next.Count)];
                        level.AddNextLevel(lvl.GameObject());
                    }
                }
            }

            previous = next;
            currentY += separation.y;
        }

        var boss = CreateLevel(0f, currentY);
        boss.isSelectable = BCMode;
        boss.Information = new Level.Boss(0);

        // Connect previous levels to boss
        foreach (var level in previous)
        {
            level.AddNextLevel(boss.GameObject());
        }
    }

    private float Variation() => Random.Range(-variation, variation);

    /// <summary>Updates the min and max for the camera position to ensure the entire map may be seen.</summary>
    private void UpdateCameraBounds(float ypos)
    {
        if (ypos > cam.YMax)
            cam.YMax = ypos;
        else if (ypos < cam.YMin)
            cam.YMin = ypos;
    }

    /// <summary>Returns a newly created level with the specified position.</summary>
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
