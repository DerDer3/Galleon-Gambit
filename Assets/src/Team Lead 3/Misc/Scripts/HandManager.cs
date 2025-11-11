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

    public float handSpread = 5f;

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

    private void updateHandVisuals()
    {
        //throw new NotImplementedException();

        int cardCount = cardHand.Count;

        for (int i = 0; i < cardCount; i++)
        {
            float rotationangle = (handSpread * (i - (cardCount - 1) / 2f));
            cardHand[i].transform.localRotation = Quaternion.Euler(0f,0f, rotationangle);
        }
    }
}
