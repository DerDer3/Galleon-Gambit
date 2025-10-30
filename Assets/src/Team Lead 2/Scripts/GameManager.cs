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
