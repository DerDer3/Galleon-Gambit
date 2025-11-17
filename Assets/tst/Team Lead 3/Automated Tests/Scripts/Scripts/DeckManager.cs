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
        Debug.Log($"[DEBUG] DeckManager: HandManager reference set: {handManager != null}");
    }

    public void InitializeDeckAndDrawHand()
    {
        Debug.Log($"[DEBUG] DeckManager: Initializing Deck and Hand. HandManager null check: {handManager == null}");
        if (handManager == null)
        {
            Debug.LogError("DeckManager cannot initialize: HandManager reference is null.");
            return;
        }

        // Dynamically load all cards from the Resources folder
        NewCard[] cards = Resources.LoadAll<NewCard>("CardData");
        CardDatabase.AddRange(cards);
        Debug.Log($"[DEBUG] DeckManager: Loaded {CardDatabase.Count} unique cards from resources.");

        // Dynamic Deck Population
        InitializeStartingDeck();

        // Draw initial hand based on maxHand count
        Debug.Log($"[DEBUG] DeckManager: Attempting to draw initial hand of size {handManager.maxHand}.");
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
        Debug.Log($"[DEBUG] DeckManager: Deck populated with {PlayerDeck.cards.Count} cards.");

        PlayerDeck.Shuffle();
        Debug.Log($"Player Deck initialized with {PlayerDeck.cards.Count} cards (x{copiesOfEachCard} of each unique card). Deck is shuffled.");
    }

    public void DrawCardToHand()
    {
        Debug.Log($"[DEBUG] DrawCardToHand: Current PlayerDeck size: {PlayerDeck.cards.Count}, DiscardDeck size: {DiscardDeck.cards.Count}, Hand size: {handManager.cardsInHand.Count}.");
        if (handManager == null)
        {
            Debug.LogError("[CRITICAL] DrawCardToHand: HandManager is null. Cannot draw.");
            return;
        }
        if (handManager.cardsInHand.Count >= handManager.maxHand)
        {
            Debug.LogWarning("[DEBUG] DrawCardToHand: Hand is full. Skipping draw.");
            return;
        }

        //Try to draw a card
        NewCard drawnCard = PlayerDeck.DrawCard();

        // Check if a card was drawn, and if not, reshuffle
        if (drawnCard == null)
        {
            Debug.Log("[DEBUG] DrawCardToHand: Player Deck empty. Checking Discard Pile...");

            if (DiscardDeck.cards.Count > 0)
            {
                Debug.Log($"[DEBUG] DrawCardToHand: Transferring {DiscardDeck.cards.Count} cards from Discard to Player Deck.");
                // Transfer discard cards to the player deck. This uses the base class TransferCardsFrom method.
                PlayerDeck.TransferCardsFrom(DiscardDeck);
                PlayerDeck.Shuffle();
                Debug.Log($"[DEBUG] DrawCardToHand: Reshuffle complete. New PlayerDeck size: {PlayerDeck.cards.Count}.");


                // Try drawing again
                drawnCard = PlayerDeck.DrawCard();
                if (drawnCard != null)
                {
                    Debug.Log($"[DEBUG] DrawCardToHand: Successfully drew {drawnCard.cardName} after reshuffle.");
                }
            }
            else
            {
                Debug.LogWarning("[DEBUG] DrawCardToHand: Both Player Deck and Discard Pile are empty. Cannot draw card.");
                return;
            }
        }

        // Add the drawn card to the hand
        if (drawnCard != null)
        {
            handManager.AddToHand(drawnCard);
        }
    }

}