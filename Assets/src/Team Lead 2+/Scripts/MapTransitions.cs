using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>Manages transitions between levels and the world map.</summary>
public class MapTransitions : MonoBehaviour
{
    public static MapTransitions Instance { get; private set; }

    public ScreenTransition ScreenTransition;
    public string MapScene;
    private Scene mapScene;
    public string GameScene;
    private Scene gameScene;

    private SceneTransitions scenes;
    private string transitioningToScene;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        scenes = new();

        mapScene = SceneManager.GetSceneByName(MapScene);
        Assert.IsNotNull(mapScene);
        SceneManager.sceneLoaded += OnSceneLoaded;

        Assert.IsNotNull(ScreenTransition);
    }

    public bool Transitioning()
    {
        return transitioningToScene != "";
    }

    public void TransitionToMap()
    {
        ScreenTransition.ShowTransition();
        transitioningToScene = MapScene;
    }

    public void TransitionLevel(Level level)
    {
        ScreenTransition.ShowTransition();
        transitioningToScene = GameScene;
    }

    public void OnTransitionComplete()
    {
        scenes.LoadInAdditionAndSetActive(transitioningToScene);
        transitioningToScene = "";
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == GameScene)
            gameScene = scene;
    }

    /// <summary>Scene adding/switching functionality. Maintains scene state between transitions.</summary>
    private class SceneTransitions
    {
        private Dictionary<string, Scene> loadedScenes = new();

        public SceneTransitions()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public void LoadInAdditionAndSetActive(string sceneName)
        {
            if (!loadedScenes.ContainsKey(sceneName))
            {
                SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            }
            else
            {
                var scene = loadedScenes[sceneName];
                Disable(SceneManager.GetActiveScene());
                SceneManager.SetActiveScene(scene);
                Enable(scene);
            }
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            loadedScenes.Add(scene.name, scene);
            if (loadedScenes.Count > 1)
            {
                Disable(SceneManager.GetActiveScene());
                SceneManager.SetActiveScene(scene);
            }
        }

        private static void SetObjectsActive(Scene scene, bool active)
        {
            if (scene == null) return;
            foreach (var obj in scene.GetRootGameObjects())
            {
                obj.SetActive(active);
            }
        }
        private static void Disable(Scene scene) => SetObjectsActive(scene, false);
        private static void Enable(Scene scene) => SetObjectsActive(scene, true);
    }
}
