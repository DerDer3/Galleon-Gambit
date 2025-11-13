using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Canvas cardCanvas;
    public GameObject cardPrefab;
    public GameState mainGame;
<<<<<<< HEAD
    List<GameObject> cardHand = new List<GameObject>();
=======

>>>>>>> 7d9bd76b5153a68cbf92ee5e3bf6234846ce92c5
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      for(int i = 0; i < 10; i++)
      {
        mainGame.mainDeck.AddCard(CardCreator.CreateRandomCard(), "Test");
        DrawHandCard();
        UpdateHandLayout();
      }
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< HEAD
    }

    void DrawHandCard()
    {
        GameObject newCard = Instantiate(cardPrefab, cardCanvas.transform, false);
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
      float totalWidth = 700f;
      float spacing = totalWidth / Mathf.Max(n - 1, 1);
      float yOffset = -150f;

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
=======
        if (mainGame.mainPlayer.get_health() <= 0)
        {
            Debug.Log("Gameover");
            SceneManager.LoadScene("GameOverScene");
        }
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
>>>>>>> 7d9bd76b5153a68cbf92ee5e3bf6234846ce92c5
    }
}
