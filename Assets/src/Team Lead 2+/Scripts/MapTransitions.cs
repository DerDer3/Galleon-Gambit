using UnityEditor;
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

    /// <summary>The currently active `GameObject`/prefab.</summary>
    private GameObject current;
    private GameObject transitioningTo;
    private MusicTracks transitionSong;

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

    /// <summary>Returns `true` if a level transition is happening.</summary>
    public bool Transitioning() => transitioningTo != null;

    /// <summary>Attempts to transition to the main menu.</summary>
    public void TransitionToMenu()
    {
        ScreenTransition.ShowTransition();
        transitioningTo = Menu;
        transitionSong = MusicTracks.Main;
    }

    /// <summary>Attempts to transition to the main world map.</summary>
    public void TransitionToMap()
    {
        ScreenTransition.ShowTransition();
        transitioningTo = Map;
        transitionSong = MusicTracks.Island;
    }

    public void TransitionToUnknown()
    {
        ScreenTransition.ShowTransition();
        transitioningTo = Unknown;
        Unknown.GetComponentInChildren<EventLevelManager>()?.StartRandomEvent();
    }

    public void TransitionToTreasure()
    {
        ScreenTransition.ShowTransition();
        transitioningTo = Treasure;
        var treasure = Treasure.GetComponentInChildren<TreasureMemoryCore>();
        treasure?.winText?.gameObject.SetActive(false);
        treasure?.SetupBoard();
    }

    /// <summary>Attempts to transition to the level based on its type.</summary>
    public void TransitionLevel(Level level)
    {
        ScreenTransition.ShowTransition();
        if (level.Information is Level.Unknown)
            TransitionToUnknown();
        else if (level.Information is Level.Treasure)
            TransitionToTreasure();
        else if (level.Information is Level.Boss)
            transitioningTo = Game; // TODO: transition to boss scene
        else if (level.Information is Level.Battle)
            transitioningTo = Game;
        else transitioningTo = Game;
        transitionSong = level.Information.MusicTrack();
    }

    /// <summary>Attempts to apply the transition level.</summary>
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

        SoundManager.Instance.play(transitionSong);
        transitionSong = MusicTracks.Main;
    }
}
