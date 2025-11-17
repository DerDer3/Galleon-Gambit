using UnityEngine;
using GallionGambit;
// using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;
using System;

public class HandManager : MonoBehaviour
{
    [HideInInspector] public DeckManager deckManager;

    public GameObject cardPrefab;

    public Transform handTransform;

    public float fanSpread = 7.5f;

    public float cardspacing = 100f;

    public float verticalspacing = 100f;

    public List<GameObject> cardsInHand = new List<GameObject>();

    public int maxHand = 5;



    public void PlayCard(GameObject playedCardObject)
    {
        Debug.Log($"[DEBUG] HandManager: Attempting to play card: {playedCardObject?.name ?? "NULL_OBJECT"}.");

        if (playedCardObject != null && cardsInHand.Contains(playedCardObject))
        {
            CardDisplay cardDisplay = playedCardObject.GetComponent<CardDisplay>();
            if (cardDisplay == null)
            {
                Debug.LogError($"[CRASH POINT] HandManager.PlayCard: Card object '{playedCardObject.name}' is missing CardDisplay component.");
                // This would be the point of crash in test Play_17 if the script didn't handle the subsequent null access
                return;
            }

            //Get the card data before destroying the GameObject
            NewCard cardData = cardDisplay.cardData;
            if (cardData == null)
            {
                Debug.LogError($"[CRASH POINT] HandManager.PlayCard: CardDisplay on '{playedCardObject.name}' has null cardData.");
                return;
            }

            // Remove the card from the hand list and destroy the GameObject
            cardsInHand.Remove(playedCardObject);
            Debug.Log($"[DEBUG] HandManager: Removed '{cardData.cardName}' from hand list. New hand size: {cardsInHand.Count}");
            Destroy(playedCardObject);

            // Move the card data to the Discard Pile
            if (deckManager != null)
            {
                deckManager.DiscardDeck.cards.Add(cardData);
                Debug.Log($"Discarded card: {cardData.cardName}. Discard Pile size: {deckManager.DiscardDeck.cards.Count}");

                if (cardsInHand.Count < maxHand)
                {
                    Debug.Log("[DEBUG] HandManager: Hand size below max. Requesting card draw.");
                    deckManager.DrawCardToHand();
                    //Debug.Log("Card Play Cycle Initiated");
                }
            }
            else
            {
                Debug.LogError("[CRASH POINT] HandManager.PlayCard: DeckManager is null. Cannot discard or draw.");
            }

            UpdateHandVisuals();
        }
    }


    public void AddToHand(NewCard cardData)
    {
        Debug.Log($"[DEBUG] HandManager: Attempting to add card '{cardData?.cardName ?? "NULL_DATA"}' to hand. Current size: {cardsInHand.Count}, Max size: {maxHand}.");

        if (cardsInHand.Count < maxHand)
        {
            if (cardPrefab == null)
            {
                Debug.LogError("[CRASH POINT] HandManager.AddToHand: cardPrefab is null. Cannot instantiate card.");
                return;
            }

            GameObject newCard = Instantiate(cardPrefab, handTransform.position, Quaternion.identity, handTransform);
            cardsInHand.Add(newCard);
            Debug.Log($"[DEBUG] HandManager: Instantiated and added '{cardData.cardName}'. New hand size: {cardsInHand.Count}");


            CardDisplay display = newCard.GetComponent<CardDisplay>();
            if (display != null)
            {
                display.cardData = cardData;
                display.UpdateCardDisplay();
            }
            else
            {
                Debug.LogError($"[CRASH POINT] HandManager.AddToHand: New card object missing CardDisplay component.");
            }
        }

        UpdateHandVisuals();
    }

    public void UpdateHandVisuals()
    {
        int cardCount = cardsInHand.Count;
        Debug.Log($"[DEBUG] UpdateHandVisuals: Processing {cardCount} cards.");


        if (cardCount == 1)
        {
            if (cardsInHand[0] == null) { Debug.LogError("[CRASH POINT] UpdateHandVisuals: Card at index 0 is null."); return; } // Test Play_11 safeguard
            cardsInHand[0].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            cardsInHand[0].transform.localPosition = new Vector3(0f, 0f, 0f);
            return;
        }
        else if (cardCount == 0)
        {
            return;
        }

        for (int i = 0; i < cardCount; i++)
        {
            if (cardsInHand[i] == null) { Debug.LogError($"[CRASH POINT] UpdateHandVisuals: Card at index {i} is null."); continue; } // Test Play_11 safeguard

            float rotationAngle = (fanSpread * (i - (cardCount - 1) / 2f));
            cardsInHand[i].transform.localRotation = Quaternion.Euler(0f, 0f, rotationAngle);

            float horizontalOffset = (cardspacing * (i - (cardCount - 1) / 2f));

            // CRASH POINT: If (cardCount - 1) is zero, normalizedPosition could hit DivideByZero, but the count=1 check prevents this.
            float normalizedPosition = (2f * i / (cardCount - 1) - 1f);
            float verticalOffset = verticalspacing * (1 - normalizedPosition * normalizedPosition);

            //Set card position
            cardsInHand[i].transform.localPosition = new Vector3(horizontalOffset, verticalOffset, 0f);
        }
    }
}