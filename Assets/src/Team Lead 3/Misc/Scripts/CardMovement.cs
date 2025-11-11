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

    private HandManager handManager; // <--- NEW: Reference to the HandManager

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

        // Find the HandManager in the scene
        handManager = FindObjectOfType<HandManager>();
        if (handManager == null)
        {
            Debug.LogError("CardMovement requires a HandManager component in the scene to play cards.");
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
                // If HandManager is found, it will update the hand visuals, 
                // which re-sets the position and rotation if in hand.
                if (handManager != null) { handManager.updateHandVisuals(); }
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
                // This state is now managed externally by the PlayCardEffect method, 
                // which destroys the GameObject. We keep the case block empty for safety 
                // but the object should be destroyed before this runs.
                glowEffect.SetActive(false);
                playArrow.SetActive(false);
                break;
        }

        currentState = newState;
    }

    // --- Play Card Helper Method ---

    private void PlayCardEffect()
    {
        if (handManager != null)
        {
            Debug.Log($"Card Played: {gameObject.name}");
            handManager.PlayCard(this.gameObject);
        }
        else
        {
            Debug.LogError("Cannot play card, HandManager reference is missing.");
            TransitionToState(CardState.Default);
        }
    }

    // --- Pointer Handlers ---

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Prevent hover effects if we are currently dragging a different card
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
                // Set the card's parent transform to the canvas root while dragging 
                // so it always renders on top of the hand.
                rTrans.SetParent(rootCanvas.transform, true);
                TransitionToState(CardState.Dragging);
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (currentState == CardState.PotentialPlay)
        {
            // CRITICAL: Play the card effect if released in the play zone
            PlayCardEffect();
            // Since PlayCardEffect destroys this object, no further code runs here.
        }
        else if (currentState == CardState.Dragging)
        {
            // If the card was dragged but not released in the play zone, return it to the hand parent
            rTrans.SetParent(handManager.handTransform, true);
            TransitionToState(CardState.Default);
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