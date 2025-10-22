using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>Information about a level on the map.</summary>
public class Level : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public GameObject Checkmark;
    public List<GameObject> NextLevels { get => nextLevels; private set => nextLevels = value; }
    public bool IsSelectable = true;
    public bool IsDone { set => SetIsDone(value); get => isDone; }
    public GameObject LevelConnectionPrefab;

    private List<GameObject> nextLevels = new();
    private bool isDone;
    private bool isHovering;
    private Vector3 initialScale;
    private Vector3 hoverScale;
    private readonly float scaleSpeed = 10f;

    public void OnPointerEnter(PointerEventData eventData) => isHovering = true;
    public void OnPointerExit(PointerEventData eventData) => isHovering = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isHovering && !IsDone)
        {
            print("TODO: set scene and change level");
            IsDone = true;
        }
    }

    public void AddNextLevel(GameObject level)
    {
        NextLevels.Add(level);
        CreateLevelConnection(level);
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

    private void Start()
    {
        initialScale = transform.localScale;
        hoverScale = initialScale * 1.25f;
    }

    private void Update()
    {
        UpdateScaleAnimation();
        UpdateSelectableAnimation();
    }

    private void UpdateScaleAnimation()
    {
        var targetScale = (isHovering && !IsDone) ? hoverScale : initialScale;
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, scaleSpeed * Time.deltaTime);
    }
    
    private void UpdateSelectableAnimation()
    {
        if (IsSelectable && !IsDone)
        {
            float angle = Mathf.Sin(Time.time) * 15f;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    // ============================== Level Types ==============================

    public class Info
    {
        public int LevelId { get; private set; }

        public string Name() => GetType().Name;
        public virtual string Description() => "Land ho to rest ye pegs.";
    }

    public class Battle : Info
    {
        // Overshadowed method to show dynamic/static binding.
        public new string Name() => "Fight";
        public override string Description() => "There be scallywags to plunder.";
    }

    public class Shop : Info
    {
        public override string Description() => "Bargain ye treasures.";
    }

    public class Boss : Info
    {
        public override string Description() => "There be a captain!";
    }

    public class Treasure : Info
    {
        public override string Description() => "A bounteous booty.";
    }

    public class Unknown : Info
    {
        public override string Description() => "Uncharted land for lootin'?";
    }
}
