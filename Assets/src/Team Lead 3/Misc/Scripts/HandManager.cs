using UnityEngine;
using GallionGambit;
using NUnit.Framework;
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

        if (cardsInHand.Contains(playedCardObject))
        {
            //Get the card data before destroying the GameObject
            NewCard cardData = playedCardObject.GetComponent<CardDisplay>().cardData;

            // Remove the card from the hand list and destroy the GameObject
            cardsInHand.Remove(playedCardObject);
            Destroy(playedCardObject);

            // Move the card data to the Discard Pile
            if (deckManager != null)
            {
                deckManager.DiscardDeck.cards.Add(cardData);
                Debug.Log($"Discarded card: {cardData.cardName}. Discard Pile size: {deckManager.DiscardDeck.cards.Count}");

                if (cardsInHand.Count < maxHand)
                {
                    deckManager.DrawCardToHand();
                    Debug.Log("Card Play Cycle Initiated");
                }
            }

            UpdateHandVisuals();
        }
    }


    public void AddToHand(NewCard cardData)
    {
        if (cardsInHand.Count < maxHand)
        {
            GameObject newCard = Instantiate(cardPrefab, handTransform.position, Quaternion.identity, handTransform);
            cardsInHand.Add(newCard);

            CardDisplay display = newCard.GetComponent<CardDisplay>();
            display.cardData = cardData;
            display.UpdateCardDisplay();
        }

        UpdateHandVisuals();
    }

    public void UpdateHandVisuals()
    {
        int cardCount = cardsInHand.Count;

        if (cardCount == 1)
        {
            cardsInHand[0].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            cardsInHand[0].transform.localPosition = new Vector3(0f, 0f, 0f);
            return;
        }

        for (int i = 0; i < cardCount; i++)
        {
            float rotationAngle = (fanSpread * (i - (cardCount - 1) / 2f));
            cardsInHand[i].transform.localRotation = Quaternion.Euler(0f, 0f, rotationAngle);

            float horizontalOffset = (cardspacing * (i - (cardCount - 1) / 2f));

            float normalizedPosition = (2f * i / (cardCount - 1) - 1f); 
            float verticalOffset = verticalspacing * (1 - normalizedPosition * normalizedPosition);

            //Set card position
            cardsInHand[i].transform.localPosition = new Vector3(horizontalOffset, verticalOffset, 0f);
        }
    }
}
