using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public GameState mainGame;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      mainGame.mainDeck.AddCard(new Slash(), "Attack");
      mainGame.mainDeck.AddCard(new Slash(), "Attack");
      mainGame.mainDeck.AddCard(new Slash(), "Attack");
      mainGame.mainDeck.AddCard(new Slash(), "Attack");
      mainGame.mainDeck.AddCard(new Slash(), "Attack");
      mainGame.mainDeck.AddCard(new ShipRepair(), "Repair");
      mainGame.mainDeck.AddCard(new ShipRepair(), "Repair");
      mainGame.mainDeck.AddCard(new ShipRepair(), "Repair");
      mainGame.mainDeck.AddCard(new ShipRepair(), "Repair");
      mainGame.mainDeck.AddCard(new ShipRepair(), "Repair");

      mainGame.mainDeck.Shuffle(mainGame.mainDeck);

      int index = 0;
      for(int i = 0; i < 5; i++)
      {
        GameObject newCard = Instantiate(cardPrefab);
        CardObject cardObject = newCard.GetComponent<CardObject>();
        cardObject.SetCard(mainGame.mainDeck.DrawCardWithReshuffle(), mainGame);
        newCard.transform.localPosition = new Vector3((index * 2) -4, -3, 0);
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
