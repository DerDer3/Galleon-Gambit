using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GallionGambit;

public class DeckManager : MonoBehaviour
{
    public List<NewCard> totalCards = new List<NewCard>();

    [HideInInspector]
    public List<NewCard> CardDatabase = new List<NewCard>();

    public Deck PlayerDeck = new Deck();
    public Deck DiscardDeck = new Deck();

    private HandManager handManager;

    public void SetHandManager(HandManager manager)
    {
        handManager = manager;
    }

    public void InitializeDeckAndDrawHand()
    {
        if (handManager == null)
        {
            return;
        }

        NewCard[] cards = Resources.LoadAll<NewCard>("CardData");
        CardDatabase.AddRange(cards);

        //Initial deck is manually populated.
        InitializeStartingDeck();

        // Draw initial hand
        for (int i = 0; i < handManager.maxHand; i++)
        {
            DrawCardToHand();
        }
    }

    public void startup()
    {
        NewCard[] cards = Resources.LoadAll<NewCard>("CardData");
        CardDatabase.AddRange(cards);

        //Initial deck is manually populated.
        InitializeStartingDeck();

        // Get HandManager reference if not set in inspector
        if (handManager == null)
        {
            handManager = FindAnyObjectByType<HandManager>();
        }

        // Draw initial hand
        for (int i = 0; i < handManager.maxHand; i++)
        {
            DrawCardToHand();
        }
    }

    private void InitializeStartingDeck()
    {
        foreach (var card in CardDatabase)
        {
            for (int i = 0; i < 3; i++)
            {
                PlayerDeck.cards.Add(card);
            }
        }

        PlayerDeck.Shuffle();
    }

    public void DrawCardToHand()
    {
        //Try to draw a card
        NewCard drawnCard = PlayerDeck.DrawCard();

        // Check if a card was drawn, and if not, reshuffle
        if (drawnCard == null)
        {
            Debug.Log("Player Deck empty. Checking Discard Pile...");

            if (DiscardDeck.cards.Count > 0)
            {
                // Transfer discard cards to the player deck
                PlayerDeck.TransferCardsFrom(DiscardDeck);
                PlayerDeck.Shuffle();

                // Try drawing again
                drawnCard = PlayerDeck.DrawCard();
            }
            else
            {
                //Debug.LogWarning("Both Player Deck and Discard Pile are empty. Cannot draw card.");
                return;
            }
        }

        // Add the drawn card to the hand
        if (drawnCard != null && handManager != null)
        {
            handManager.AddToHand(drawnCard);
        }
    }

}
