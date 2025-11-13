using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Runtime.CompilerServices;
using GallionGambit;

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

    private HandManager handManager;

    private Vector2 originalLocalPointerPosition;
    private Vector3 originalLocalPosition;
    private Vector3 originalScale;
    private Quaternion originalRotation;

    // --- State & Settings ---
    private CardState currentState = CardState.Default;

    [SerializeField] private float selectScale = 1.1f;
    [SerializeField] private Vector3 potentialPlayPosition;
    [SerializeField] private Vector2 cardPlay;
    [SerializeField] private float lerpFactor = 0.1f;

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

    private void TransitionToState(CardState newState)
    {
        if (currentState == newState) return;

        switch (newState)
        {
            case CardState.Default:
                rTrans.localScale = originalScale;
                rTrans.localRotation = originalRotation;
                rTrans.localPosition = originalLocalPosition;


                if (handManager != null) { handManager.UpdateHandVisuals(); }
                glowEffect.SetActive(false);
                playArrow.SetActive(false);
                break;

            case CardState.Hovered:
            case CardState.Dragging:
                HandleDragState();

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

                HandlePlayState();

                if (!Input.GetMouseButton(0))
                {
                    TransitionToState(CardState.Default);
                }
                glowEffect.SetActive(false);
                playArrow.SetActive(false);
                break;
        }

        currentState = newState;
    }

    //=========================================================================================================================
    //Card States

    private void HandleDragState()
    {
        rTrans.localRotation = Quaternion.identity;
    }

    private void HandlePlayState()
    {
        rTrans.localPosition = potentialPlayPosition;
        rTrans.localRotation = Quaternion.identity;

        if (Input.mousePosition.y < cardPlay.y)
        {
            currentState = CardState.Dragging;
            playArrow.SetActive(false);
        }
    }

    private void PlayCardEffect()
    {
        if (GameManager2.Instance == null)
        {
            Debug.LogError("Cannot play card: GameManager2 not initialized.");
            TransitionToState(CardState.Default);
            return;
        }

        // --- NEW LOGIC: Check Mana & Deduct Cost ---
        // ASSUMPTION: CardObject.CardData has a public 'ManaCost' property/field. Using 1 for now.
        int manaCost = 1; // Placeholder: Replace with actual CardObject data reference later (e.g., cardObject.CardData.ManaCost)

        if (GameManager2.Instance.TryPlayCard(manaCost))
        {
            Debug.Log($"Card Played: {gameObject.name}. Mana remaining: {GameManager2.Instance.PlayerMana.get_amount()}");

            // 1. Execute the card's effect here (e.g., GameManager2.Instance.MainPlayer.set_health(damage))
            // This is where your card's unique logic (Attack, Defense, etc.) would execute.

            // 2. Add card to discard pile via HandManager
            if (handManager != null)
            {
                handManager.PlayCard(this.gameObject);
            }
        }
        else
        {
            Debug.LogWarning($"Cannot play card: Insufficient Mana ({manaCost}).");
            // Return card to hand if play failed
            rTrans.SetParent(handManager.handTransform, true);
            TransitionToState(CardState.Default);
        }
    }

    //=========================================================================================================================
    // States: 

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentState == CardState.Default)
        {
            originalLocalPosition = rTrans.localPosition;
            originalRotation = rTrans.localRotation;
            originalScale = rTrans.localScale;
            currentState = CardState.Hovered;

            rTrans.SetAsLastSibling();

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
        // Only allow picking up the card if it's the player's turn
        if (GameManager2.Instance != null && !GameManager2.Instance.IsPlayerTurn) return;

        if (currentState == CardState.Hovered || currentState == CardState.Default)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rootCanvas.GetComponent<RectTransform>(),
                eventData.position,
                eventData.pressEventCamera,
                out originalLocalPointerPosition))
            {
                originalLocalPointerPosition /= rootCanvas.scaleFactor;

                originalLocalPosition = rTrans.localPosition;

                rTrans.SetParent(rootCanvas.transform, true);
                TransitionToState(CardState.Dragging);
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (currentState == CardState.PotentialPlay)
        {
            PlayCardEffect();
            // Transition is handled inside PlayCardEffect now (Default on success, or stays in hand on fail)
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
        // Only allow dragging if it's the player's turn
        if (GameManager2.Instance != null && !GameManager2.Instance.IsPlayerTurn) return;

        if (currentState == CardState.Dragging)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rootCanvas.GetComponent<RectTransform>(),
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localPointerPosition))
            {
                Vector2 localPoint = localPointerPosition / rootCanvas.scaleFactor;
                Vector3 offsetToOrgin = localPoint - originalLocalPointerPosition;
                Vector3 targetPosition = originalLocalPosition + offsetToOrgin;

                rTrans.localPosition = Vector3.Lerp(rTrans.localPosition, targetPosition, lerpFactor);

                if (rTrans.localPosition.y > cardPlay.y - 1)//End cards wouldn't work unless I added the -1.
                {
                    TransitionToState(CardState.PotentialPlay);
                }
                else
                {
                    TransitionToState(CardState.Dragging);
                }
            }
        }//end of if
    }//End of on drag
}