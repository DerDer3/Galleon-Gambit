using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>Manages transitions between levels and the world map.</summary>
public class MapTransitions : MonoBehaviour
{
    public static MapTransitions Instance { get; private set; }

    public string MapScene;
    private Scene mapScene;
    public string GameScene;
    private Scene gameScene;
    private bool hasLoadedGame;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        mapScene = SceneManager.GetSceneByName(MapScene);
        Assert.IsNotNull(mapScene);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void TransitionLevel(Level level)
    {
        if (!hasLoadedGame)
        {
            SceneManager.LoadScene(GameScene, LoadSceneMode.Additive);
            hasLoadedGame = true;
        }
        else
        {
            EnableScene(gameScene);
            SceneManager.SetActiveScene(gameScene);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.name == GameScene)
        {
            DisableScene(mapScene);
            gameScene = scene;
            SceneManager.SetActiveScene(gameScene);
        }
    }

    private static void SetSceneActive(Scene scene, bool active)
    {
        if (scene == null) return;
        foreach (var obj in scene.GetRootGameObjects())
        {
            obj.SetActive(active);
        }
    }
    private static void DisableScene(Scene scene) => SetSceneActive(scene, false);
    private static void EnableScene(Scene scene) => SetSceneActive(scene, true);
}
