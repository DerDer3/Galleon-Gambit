using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameObject cardPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      List<Card> cards = new List<Card>();
      
      cards.Add(new Slash());
      cards.Add(new Slash());

      int index = 0;
      foreach (var c in cards)
      {
        Debug.Log("Creating Card");
        GameObject newCard = Instantiate(cardPrefab);
        CardObject cardObject = newCard.GetComponent<CardObject>();
        cardObject.SetCard(c);
        newCard.transform.localPosition = new Vector3(index * 3, -3, 0);
        index += 1;
      }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
