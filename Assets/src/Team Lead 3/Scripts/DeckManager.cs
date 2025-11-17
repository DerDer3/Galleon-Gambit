using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GallionGambit;

public class DeckManager : MonoBehaviour
{
    // Configuration for starting deck
    [SerializeField] private int copiesOfEachCard = 3;

    [HideInInspector]
    public List<NewCard> CardDatabase = new List<NewCard>();

    public PlayerDeckType PlayerDeck = new PlayerDeckType();
    public DiscardDeckType DiscardDeck = new DiscardDeckType();

    private HandManager handManager;

    public void SetHandManager(HandManager manager)
    {
        handManager = manager;
    }

    public void InitializeDeckAndDrawHand()
    {
        if (handManager == null)
        {
            //Debug.LogError("DeckManager cannot initialize: HandManager reference is null.");
            return;
        }

        // Dynamically load all cards from the Resources folder
        NewCard[] cards = Resources.LoadAll<NewCard>("CardData");
        CardDatabase.AddRange(cards);

        // Dynamic Deck Population
        InitializeStartingDeck();

        // Draw initial hand based on maxHand count
        for (int i = 0; i < handManager.maxHand; i++)
        {
            DrawCardToHand();
        }
    }

    // Dynamic deck creation: adds a fixed number of copies of every card in the database.
    private void InitializeStartingDeck()
    {
        if (CardDatabase.Count == 0)
        {
            //Debug.LogError("CardDatabase is empty. Check if 'CardData' ScriptableObjects are in a Resources folder.");
            return;
        }

        foreach (var card in CardDatabase)
        {
            for (int i = 0; i < copiesOfEachCard; i++)
            {
                PlayerDeck.cards.Add(card);
            }
        }

        PlayerDeck.Shuffle();
        //Debug.Log($"Player Deck initialized with {PlayerDeck.cards.Count} cards (x{copiesOfEachCard} of each unique card).");
    }

    public void DrawCardToHand()
    {
        //Try to draw a card
        NewCard drawnCard = PlayerDeck.DrawCard();

        // Check if a card was drawn, and if not, reshuffle
        if (drawnCard == null)
        {
            //Debug.Log("Player Deck empty. Checking Discard Pile...");

            if (DiscardDeck.cards.Count > 0)
            {
                // Transfer discard cards to the player deck. This uses the base class TransferCardsFrom method.
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