using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>Information about a level on the map.</summary>
public class Level : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public TextMeshProUGUI LevelNameText;
    public TextMeshProUGUI LevelDescriptionText;
    public List<LevelSprite> LevelSprites;
    public GameObject Checkmark;
    public List<GameObject> NextLevels { get => nextLevels; private set => nextLevels = value; }
    public bool IsSelectable = true;
    public bool IsDone { set => SetIsDone(value); get => isDone; }
    public GameObject LevelConnectionPrefab;
    public Info Information { set => SetInfo(value); get => info; }
    public UnityEvent<Level> HoverEntered;
    public UnityEvent HoverExited;

    private Info info;
    private List<GameObject> nextLevels = new();
    private bool isDone;
    private bool isHovering;
    private Vector3 initialScale;
    private Vector3 hoverScale;
    private readonly float scaleSpeed = 10f;
    private readonly float textAppearSpeed = 10f;

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

    private Sprite GetSpriteForLevelType(Info i)
    {
        foreach (var spr in LevelSprites)
            if (spr.Name == i.Name())
                return spr.Sprite;
        return null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        HoverEntered.Invoke(this);
        isHovering = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        HoverExited.Invoke();
        isHovering = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isHovering && !IsDone)
        {
            IsDone = true;
            SelectLevel();
        }
    }

    public void SelectLevel()
    {
        MapTransitions.Instance.TransitionLevel(this);
    }

    public void AddNextLevel(GameObject level)
    {
        NextLevels.Add(level);
        CreateLevelConnection(level);
    }

    public bool HasNextLevel()
    {
        return nextLevels.Count != 0;
    }

    private void CreateLevelConnection(GameObject level)
    {
        var connection = Instantiate(LevelConnectionPrefab).GetComponent<LevelConnection>();
        connection.FromLevel = gameObject;
        connection.ToLevel = level;
    }

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
    }

    private void UpdateScaleAnimation()
    {
        var targetScale = (isHovering && !IsDone) ? hoverScale : initialScale;
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, scaleSpeed * Time.deltaTime);
    }

    private void UpdateTextAppearAnimation()
    {
        float target = (isHovering && !IsDone) ? 1f : 0f;
        Color color = LevelNameText.color;
        color.a = Mathf.Lerp(color.a, target, Time.deltaTime * textAppearSpeed);
        LevelDescriptionText.color = color;
        LevelNameText.color = color;
    }

    private void UpdateSelectableAnimation()
    {
        if (IsSelectable && !IsDone)
        {
            float angle = Mathf.Sin(Time.time) * 15f;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    public void RandomizeInformation()
    {
        var types = new[] { typeof(Battle), typeof(Treasure), typeof(Unknown) };
        var type = types[UnityEngine.Random.Range(0, types.Length)];
        Information = type.Instantiate(true, 0) as Info;
    }

    [Serializable]
    public struct LevelSprite
    {
        public string Name;
        public Sprite Sprite;
    }

    // ============================== Level Types ==============================

    public class Info
    {
        public Info(int levelId) => LevelId = levelId;

        public int LevelId { get; private set; }

        public string Name() => GetType().Name;
        public virtual string Description() => "Land ho to rest ye pegs.";
    }

    public class Battle : Info
    {
        public Battle(int levelId) : base(levelId) { }

        // Overshadowed method to show dynamic/static binding.
        public new string Name() => "Fight";
        public override string Description() => "There be scallywags to plunder.";
    }

    // Shops Were Removed
    // public class Shop : Info
    // {
    //     public Shop(int levelId) : base(levelId) { }
    //     public override string Description() => "Bargain ye treasures.";
    // }

    public class Boss : Info
    {
        public Boss(int levelId) : base(levelId) { }
        public override string Description() => "A vessel approaches...";
    }

    public class Treasure : Info
    {
        public Treasure(int levelId) : base(levelId) { }
        public override string Description() => "A bounteous booty.";
    }

    public class Unknown : Info
    {
        public Unknown(int levelId) : base(levelId) { }
        public override string Description() => "Uncharted land for lootin'?";
    }
}
