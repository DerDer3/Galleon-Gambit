using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public GameState mainGame;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      
      mainGame.cards.Add(new Slash());
      mainGame.cards.Add(new ShipRepair());
      mainGame.cards.Add(new Slash());
      mainGame.cards.Add(new ShipRepair());
      mainGame.cards.Add(new Slash());
      mainGame.cards.Add(new ShipRepair());

      int index = 0;
      int cardCount = mainGame.cards.Count;
      foreach (var c in mainGame.cards)
      {
        Debug.Log("Creating Card: " + c.cardName);
        GameObject newCard = Instantiate(cardPrefab);
        CardObject cardObject = newCard.GetComponent<CardObject>();
        cardObject.SetCard(c, mainGame);
        cardObject.ChangeColor(index * 0.1f + 0.1f, index * 0.1f + 0.1f, index * 0.1f + 0.1f);
        newCard.transform.localPosition = new Vector3((index * 2) -5, -3, 0);
        index += 1;
      }
    }

    // Update is called once per frame
    void Update()
    {
       if(mainGame.mana.get_amount() == 0)
       {
         mainGame.turn = true;
         int currentHealth = mainGame.mainPlayer.get_health();
         mainGame.mainPlayer.set_health(currentHealth - 10);
         mainGame.mana.set_amount(3);
       }
       else
       {
         mainGame.turn = false;
       }
    }
}
