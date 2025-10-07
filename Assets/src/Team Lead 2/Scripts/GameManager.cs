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
      cards.Add(new HealPotion());
      cards.Add(new Slash());
      cards.Add(new HealPotion());
      cards.Add(new Slash());
      cards.Add(new HealPotion());

      int index = 0;
      foreach (var c in cards)
      {
        Debug.Log("Creating Card: " + c.cardName);
        GameObject newCard = Instantiate(cardPrefab);
        CardObject cardObject = newCard.GetComponent<CardObject>();
        cardObject.SetCard(c);
        cardObject.ChangeColor(index * 0.1f + 0.1f, index * 0.1f + 0.1f, index * 0.1f + 0.1f);
        newCard.transform.localPosition = new Vector3((index * 2) -5, -3, 0);
        index += 1;
      }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
