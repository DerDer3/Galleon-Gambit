using UnityEngine;
using GallionGambit;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;
using System;

public class HandManager : MonoBehaviour
{
    public GameObject cardPrefab;

    public Transform handTransform;

    public float handSpread = 7.5f;

    public float cardspacing = 100f;

    public float verticalspacing = 100f;

    public List<GameObject> cardHand = new List<GameObject>();

     
    private void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            AddToHand();
        }

    }

    public void AddToHand()
    {
        GameObject newCard = Instantiate(cardPrefab, handTransform.position, Quaternion.identity, handTransform);
        cardHand.Add(newCard);
        updateHandVisuals();
    }

    private void Update()
    {
        updateHandVisuals();
    }

    private void updateHandVisuals()
    {
        int cardCount = cardHand.Count;

        if(cardCount == 0)
        {
            cardHand[0].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            cardHand[0].transform.localPosition = new Vector3(0f, 0f, 0f);
            return;
        }

        for (int i = 0; i < cardCount; i++)
        {
            float rotationangle = (handSpread * (i - (cardCount - 1) / 2f));
            cardHand[i].transform.localRotation = Quaternion.Euler(0f,0f, rotationangle);

            float horizontalOffset = (cardspacing * (i - (cardCount - 1) / 2f));

            float normalizedPosition = (2f * i / (cardCount - 1) - 1f);
            float verticalOffset = verticalspacing * (1-normalizedPosition * normalizedPosition);

            cardHand[i].transform.localPosition = new Vector3(horizontalOffset, verticalOffset, 0f);
        }
    }
}
