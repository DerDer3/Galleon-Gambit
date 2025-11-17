using UnityEngine;

/// <summary>Manages transitions between levels and the world map.</summary>
public class MapTransitions : MonoBehaviour
{
    public static MapTransitions Instance { get; private set; }

    [SerializeField] private ScreenTransition ScreenTransition;

    [Header("Level Transitions")]
    [SerializeField] private GameObject Menu;
    [SerializeField] private GameObject Treasure;
    [SerializeField] private GameObject Unknown;
    [SerializeField] private GameObject Map;
    [SerializeField] private GameObject Game;

    private GameObject current;
    private GameObject transitioningTo;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        DetectCurrent();
    }

    private void DetectCurrent()
    {
        if (Menu.activeSelf) current = Menu;
        else if (Treasure.activeSelf) current = Treasure;
        else if (Unknown.activeSelf) current = Unknown;
        else if (Map.activeSelf) current = Map;
        else if (Game.activeSelf) current = Game;
    }

    public bool Transitioning()
    {
        return transitioningTo != null;
    }

    public void TransitionToMenu()
    {
        ScreenTransition.ShowTransition();
        transitioningTo = Menu;
    }

    public void TransitionToMap()
    {
        ScreenTransition.ShowTransition();
        transitioningTo = Map;
    }

    public void TransitionLevel(Level level)
    {
        ScreenTransition.ShowTransition();
        if (level.Information is Level.Unknown)
            transitioningTo = Unknown;
        else if (level.Information is Level.Treasure)
            transitioningTo = Treasure;
        else if (level.Information is Level.Boss)
            transitioningTo = Game; // TODO: transition to boss scene
        else if (level.Information is Level.Battle)
            transitioningTo = Game;
        else transitioningTo = Game;
    }

    public void OnTransitionComplete()
    {
        if (transitioningTo == null)
        {
            Debug.LogWarning("Attempted to transition to a `null` level. Transition cannot complete. Ensure the level transition fields are correct.");
            return;
        }

        current.SetActive(false);
        transitioningTo.SetActive(true);
        current = transitioningTo;
        transitioningTo = null;
    }
}
