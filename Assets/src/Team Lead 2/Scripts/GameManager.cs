using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public GameState mainGame;
    List<GameObject> cardHand = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
<<<<<<< HEAD
<<<<<<< Updated upstream
<<<<<<< Updated upstream
      
      mainGame.cards.Add(new Slash());
      mainGame.cards.Add(new ShipRepair());
      mainGame.cards.Add(new Slash());
      mainGame.cards.Add(new ShipRepair());
      mainGame.cards.Add(new Slash());
      mainGame.cards.Add(new ShipRepair());
=======
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
>>>>>>> 246c58e50b236f02eb53e0e308c5adfe1d16faa3

      int index = 0;
      for(int i = 0; i < 5; i++)
      {
        GameObject newCard = Instantiate(cardPrefab);
        CardObject cardObject = newCard.GetComponent<CardObject>();
        cardObject.SetCard(mainGame.mainDeck.DrawCardWithReshuffle(), mainGame);
        newCard.transform.localPosition = new Vector3((index * 2) -4, -3, 0);
        index += 1;
=======
=======
>>>>>>> Stashed changes
      for(int i = 0; i < 10000; i++)
      {
        mainGame.mainDeck.AddCard(new ShipRepair(), "Repair");
      }

      mainGame.mainDeck.Shuffle(mainGame.mainDeck);

      for(int i = 0; i < 9000; i++)
      {
        DrawHandCard();
        UpdateHandLayout();
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
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

    void DrawHandCard()
    {
        GameObject newCard = Instantiate(cardPrefab);
        CardObject cardObject = newCard.GetComponent<CardObject>();
        cardObject.SetCard(mainGame.mainDeck.DrawCardWithReshuffle(), mainGame);
        cardHand.Add(newCard);

        UpdateHandLayout();
    }

    void UpdateHandLayout()
    {
      int n = cardHand.Count;

      if(n == 0) return;

      // float maxFanAngle = 25f;
      float totalWidth = 14f;
      float spacing = totalWidth / Mathf.Max(n - 1, 1);
      float yOffset = -3f;

      // float angleStep = n > 1 ? maxFanAngle / (n - 1) : 0f;
      // float startAngle = -maxFanAngle / 2f;
      float startX = -(n - 1) * spacing / 2f;

      for(int i = 0; i < n; i++)
      {
        // float angle = startAngle + i * angleStep;
        float xPos = startX + i * spacing;

        cardHand[i].transform.localPosition = new Vector3(xPos, yOffset, 0);
        // cardHand[i].transform.localRotation = Quaternion.Euler(0, 0, -angle);
      }
    }
}
