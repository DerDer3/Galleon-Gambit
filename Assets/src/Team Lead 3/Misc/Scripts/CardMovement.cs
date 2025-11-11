using UnityEngine;
using UnityEngine.EventSystems;

public enum CardState
{
    Default = 0,
    Hovered = 1,
    Dragging = 2,
    PotentialPlay = 3,
    Played = 4
}

public class CardMovement : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    // --- Components & Original Values ---
    private RectTransform rTrans;
    [SerializeField] private Canvas rootCanvas;

    private Vector2 originalLocalPointerPosition;
    private Vector3 originalLocalPosition;
    private Vector3 originalScale;
    private Quaternion originalRotation;

    // --- State & Settings ---
    private CardState currentState = CardState.Default;

    [SerializeField] private float selectScale = 1.1f;
    [SerializeField] private float playThresholdY = 100f;
    [SerializeField] private Vector3 potentialPlayPosition;

    [SerializeField] private GameObject glowEffect;
    [SerializeField] private GameObject playArrow;


    void Awake()
    {
        rTrans = GetComponent<RectTransform>();
        if (rootCanvas == null)
        {
            rootCanvas = GetComponentInParent<Canvas>().rootCanvas;
        }

        originalScale = rTrans.localScale;
        originalRotation = rTrans.localRotation;
    }


    void Update()
    {
        // Only need Update() to continuously apply scale/rotation for the active states
        switch (currentState)
        {
            case CardState.Hovered:
            case CardState.Dragging:
                rTrans.localScale = originalScale * selectScale;
                rTrans.localRotation = Quaternion.identity;
                break;
        }
    }

    // --- State Transition ---

    private void TransitionToState(CardState newState)
    {
        if (currentState == newState) return;

        switch (newState)
        {
            case CardState.Default:
                rTrans.localScale = originalScale;
                rTrans.localRotation = originalRotation;
                rTrans.localPosition = originalLocalPosition;
                glowEffect.SetActive(false);
                playArrow.SetActive(false);
                break;

            case CardState.Hovered:
            case CardState.Dragging:
                glowEffect.SetActive(true);
                playArrow.SetActive(false);
                rTrans.localRotation = Quaternion.identity;
                break;

            case CardState.PotentialPlay:
                rTrans.localPosition = potentialPlayPosition;
                rTrans.localRotation = Quaternion.identity;
                glowEffect.SetActive(true);
                playArrow.SetActive(true);
                break;

            case CardState.Played:
                rTrans.localPosition = potentialPlayPosition;
                rTrans.localRotation = Quaternion.identity;
                glowEffect.SetActive(false);
                playArrow.SetActive(false);
                break;
        }

        currentState = newState;
    }

    // --- Pointer Handlers ---

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentState == CardState.Default)
        {
            originalLocalPosition = rTrans.localPosition;
            TransitionToState(CardState.Hovered);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentState == CardState.Hovered)
        {
            TransitionToState(CardState.Default);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (currentState == CardState.Hovered || currentState == CardState.Default)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rootCanvas.GetComponent<RectTransform>(),
                eventData.position,
                eventData.pressEventCamera,
                out originalLocalPointerPosition))
            {
                originalLocalPosition = rTrans.localPosition;
                TransitionToState(CardState.Dragging);
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (currentState == CardState.Dragging)
        {
            TransitionToState(CardState.Default);
        }
        else if (currentState == CardState.PotentialPlay)
        {
            TransitionToState(CardState.Played);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentState == CardState.Dragging || currentState == CardState.PotentialPlay)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rootCanvas.GetComponent<RectTransform>(),
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localPointerPosition))
            {
                Vector2 localPoint = localPointerPosition / rootCanvas.scaleFactor;
                Vector3 offsetToOriginal = localPoint - originalLocalPointerPosition;
                rTrans.localPosition = originalLocalPosition + offsetToOriginal;

                if (rTrans.localPosition.y > playThresholdY)
                {
                    TransitionToState(CardState.PotentialPlay);
                }
                else
                {
                    TransitionToState(CardState.Dragging);
                }
            }
        }
    }
}