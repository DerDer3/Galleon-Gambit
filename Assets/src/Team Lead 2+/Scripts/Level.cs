using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>Information and interactions of a level on the map.</summary>
public class Level : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [Header("Level Details")]
    /// <summary>Is `true` if this level may be selected (not beaten, predecessor beaten, etc.)</summary>
    [SerializeField] public bool isSelectable = true;
    /// <summary>The prefab object for making connections between levels.</summary>
    [SerializeField] private GameObject LevelConnectionPrefab;
    /// <summary>The list of levels that proceed this level.</summary>
    [SerializeField] private List<GameObject> nextLevels = new();
    /// <summary>Is `true` if the level has been completed.</summary>
    [SerializeField] private bool IsDone { set => SetIsDone(value); get => isDone; }

    [Header("Graphics")]
    /// <summary>The text that displays this level's name.</summary>
    [SerializeField] private TextMeshProUGUI LevelNameText;
    /// <summary>The text that displays this level's description.</summary>
    [SerializeField] private TextMeshProUGUI LevelDescriptionText;
    /// <summary>
    /// The list of names that correspond to sprites.
    /// The names are mapped to sprites based on the name of the class derived from `Info`.
    /// </summary>
    [SerializeField] private List<LevelSprite> LevelSprites;
    /// <summary>The checkmark that is displayed when the level has been completed.</summary>
    [SerializeField] private GameObject Checkmark;

    [Header("Events")]
    public UnityEvent<Level> HoverEntered;
    public UnityEvent HoverExited;

    /// <summary>The level type and additional information about the level.</summary>
    public Info Information { set => SetInfo(value); get => info; }

    private Info info;
    private bool isDone;
    private bool isHovering;

    private Vector3 initialScale;
    private Vector3 hoverScale;
    /// <summary>The value for which delta time is multiplied in lerp for animating scale and transparency.</summary>
    private readonly float scaleSpeed = 10f;

    #region UNITY METHODS

    private void Awake()
    {
        // Make transparent
        LevelNameText.color -= new Color(0f, 0f, 0f, 1f);
        LevelDescriptionText.color = LevelNameText.color;
    }

    private void Start()
    {
        initialScale = transform.localScale;
        hoverScale = initialScale * 1.25f;
    }

    /// <summary>Enters the level into hover mode.</summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isSelectable)
        {
            HoverEntered.Invoke(this);
            isHovering = true;
        }
    }

    /// <summary>Exits the level from hover mode.</summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (isSelectable)
        {
            HoverExited.Invoke();
            isHovering = false;
        }
    }

    /// <summary>Handles switching levels if current level can be played.</summary>
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isHovering && !IsDone && isSelectable)
        {
            IsDone = true;
            SelectLevel();
            foreach (var lvl in nextLevels)
                lvl.GetComponent<Level>().isSelectable = true;
        }
    }

    #endregion
    #region PUBLIC METHODS

    /// <summary>Sets the information about the level and updates the sprite and text accordingly.</summary>
    private void SetInfo(Info value)
    {
        info = value;
        LevelNameText.text = (info is Battle battleInfo) ? battleInfo.Name() : info.Name();
        LevelDescriptionText.text = info.Description();

        var sprite = GetSpriteForLevelType(info);
        if (sprite != null)
        {
            GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }

    /// <summary>Retursn the sprite (based on `LevelSprites`) for the given level type.</summary>
    private Sprite GetSpriteForLevelType(Info i)
    {
        foreach (var spr in LevelSprites)
            if (spr.Name == i.Name())
                return spr.Sprite;
        return null;
    }

    /// <summary>Plays this level by transitioning scenes into the game.</summary>
    public void SelectLevel()
    {
        MapTransitions.Instance.TransitionLevel(this);
        SoundManager.Instance.play(info.SelectSound());
    }

    /// <summary>
    /// Connects this level to the specified level.
    /// A dotted line is also added to signal the levels are connected.
    /// </summary>
    public void AddNextLevel(GameObject level)
    {
        nextLevels.Add(level);
        CreateLevelConnection(level);
    }

    /// <summary>Returns `true` if this level has any levels that proceed it.</summary>
    public bool HasNextLevel() => nextLevels.Count != 0;

    #endregion
    #region PRIVATE METHODS

    /// <summary>Instantiates a level connection from this level to the specified level.</summary>
    private void CreateLevelConnection(GameObject level)
    {
        var connection = Instantiate(LevelConnectionPrefab).GetComponent<LevelConnection>();
        connection.FromLevel = gameObject;
        connection.ToLevel = level;
    }

    /// <summary>Sets the value of `isDone` and updates the checkmark accordingly.</summary>
    private void SetIsDone(bool value)
    {
        isDone = value;
        transform.rotation = Quaternion.identity;
        if (Checkmark)
        {
            Checkmark.SetActive(value);
        }
    }

    private void Update()
    {
        UpdateScaleAnimation();
        UpdateTextAppearAnimation();
        UpdateSelectableAnimation();
        GetComponent<SpriteRenderer>().color = isSelectable ? Color.white : Color.black;
    }

    private void UpdateScaleAnimation()
    {
        var targetScale = (isHovering && !IsDone) ? hoverScale : initialScale;
        targetScale = isSelectable ? targetScale : initialScale * 0.75f;
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, scaleSpeed * Time.deltaTime);
    }

    private void UpdateTextAppearAnimation()
    {
        float target = (isHovering && !IsDone && isSelectable) ? 1f : 0f;
        Color color = LevelNameText.color;
        color.a = Mathf.Lerp(color.a, target, Time.deltaTime * scaleSpeed);
        LevelDescriptionText.color = color;
        LevelNameText.color = color;
    }

    private void UpdateSelectableAnimation()
    {
        if (isSelectable && !IsDone)
        {
            float angle = Mathf.Sin(Time.time) * 15f;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    /// <summary>Assigns a random level type to this level.</summary>
    public void RandomizeInformation()
    {
        var types = new[] { typeof(Battle), typeof(Treasure), typeof(Unknown) };
        var type = types[UnityEngine.Random.Range(0, types.Length)];
        Information = type.Instantiate(true, 0) as Info;
    }

    #endregion
    #region PUBLIC CLASSES

    /// <summary>Helper structure for serializing the sprites for level types.</summary>
    [Serializable]
    public struct LevelSprite
    {
        /// <summary>The name of the class that derives `Info`.</summary>
        public string Name;
        /// <summary>The sprite that belongs to the named class.</summary>
        public Sprite Sprite;
    }

    // ============================== Level Types ==============================

    /// <summary>The base type for all level types.</summary>
    public class Info
    {
        public Info(int levelId) => LevelId = levelId;

        /// <summary>The level ID this level matches.</summary>
        public int LevelId { get; private set; }

        /// <summary>Returns the display name of the level type.</summary>
        public string Name() => GetType().Name;
        /// <summary>Returns the display description of the level type.</summary>
        public virtual string Description() => "Land ho to rest ye pegs.";
        /// <summary>Returns the sound played upon select.</summary>
        public virtual SoundEffects SelectSound() => SoundEffects.Button;
        /// <summary>Returns the music track played upon transitioning to level.</summary>
        public virtual MusicTracks MusicTrack() => MusicTracks.Main;
    }

    /// <summary>A level where the player must battle enemies.</summary>
    public class Battle : Info
    {
        public Battle(int levelId) : base(levelId) { }

        // Overshadowed method to show dynamic/static binding.
        public new string Name() => "Fight";
        public override string Description() => "There be scallywags to plunder!";
        public override SoundEffects SelectSound() => SoundEffects.Enter_Battle;
        public override MusicTracks MusicTrack() => MusicTracks.Battle;
    }

    /// <summary>A level where the player must battle the final boss.</summary>
    public class Boss : Info
    {
        public Boss(int levelId) : base(levelId) { }
        public override string Description() => "A vessel approaches...";
        public override SoundEffects SelectSound() => SoundEffects.Enter_Battle;
        public override MusicTracks MusicTrack() => MusicTracks.Boss;
    }

    /// <summary>A level where the player finds treasure and awards.</summary>
    public class Treasure : Info
    {
        public Treasure(int levelId) : base(levelId) { }
        public override string Description() => "A bounteous booty.";
        public override SoundEffects SelectSound() => SoundEffects.Enter_Memory_Game;
        public override MusicTracks MusicTrack() => MusicTracks.Island;
    }

    /// <summary>A level where the player must make choices that lead to unpredicted outcomes.</summary>
    public class Unknown : Info
    {
        public Unknown(int levelId) : base(levelId) { }
        public override string Description() => "Uncharted land for lootin'?";
        public override SoundEffects SelectSound() => SoundEffects.Button;
        public override MusicTracks MusicTrack() => MusicTracks.Island;
    }

    #endregion
}
