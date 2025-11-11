using UnityEngine;
using GallionGambit;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;
using System;

public class HandManager : MonoBehaviour
{
    public DeckManager deckManager;

    public GameObject cardPrefab;

    public Transform handTransform;
     
    public float fanSpread = 12f;

    public float cardspacing = -105;

    public float verticalspacing = 30f;

    public List<GameObject> cardsInHand = new List<GameObject>();

    public int maxHand = 5;

     
    public void AddToHand(NewCard cardData)
    {
        if(cardsInHand.Count < maxHand)
        {
            GameObject newCard = Instantiate(cardPrefab, handTransform.position, Quaternion.identity, handTransform);
            cardsInHand.Add(newCard);

            newCard.GetComponent<CardDisplay>().cardData = cardData;
        }
        
        updateHandVisuals();
    }

    private void Update()
    {
        //ADD TO GAME MANAGER
       updateHandVisuals();
    }

    public void updateHandVisuals()
    {
        int cardCount = cardsInHand.Count;

        if(cardCount == 1)
        {
            cardsInHand[0].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            cardsInHand[0].transform.localPosition = new Vector3(0f, 0f, 0f);
            return;
        }

        for (int i = 0; i < cardCount; i++)
        {
            float rotationangle = (fanSpread * (i - (cardCount - 1) / 2f));
            cardsInHand[i].transform.localRotation = Quaternion.Euler(0f,0f, rotationangle);

            float horizontalOffset = (cardspacing * (i - (cardCount - 1) / 2f));

            float normalizedPosition = (2f * i / (cardCount - 1) - 1f);
            float verticalOffset = verticalspacing * (1-normalizedPosition * normalizedPosition);

            cardsInHand[i].transform.localPosition = new Vector3(horizontalOffset, verticalOffset, 0f);
        }
    }
}
