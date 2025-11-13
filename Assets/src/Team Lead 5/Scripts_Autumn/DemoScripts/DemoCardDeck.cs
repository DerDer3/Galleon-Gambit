// using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class DemoCardDeck
{
    public List<DemoCard> cards = new List<DemoCard>();

    //Add single card to card deck.
    public void AddCard(DemoCard card, string type)
    {
        //Intended use:  playerDeck.AddCard(new Slash(), "Attack");
        if (card == null)
        {
            return;
        }

        cards.Add(card);
    }

    // Add a list of cards into the deck
    public void AddCards(List<DemoCard> newCards)
    {
        if (newCards == null || newCards.Count == 0)
        {
            return;
        }

        cards.AddRange(newCards);
    }

    // Draw a card from the top of the deck (or random if preferred)
    public DemoCard DrawCard()
    {
        if (cards.Count == 0)
        {
            return null;
        }

        DemoCard drawnCard = cards[0];
        cards.RemoveAt(0);

        Debug.Log($"Drew card: {drawnCard.cardName}");
        return drawnCard;
    }

    // Fisher-Yates based shuffle
    public void Shuffle(DemoCardDeck deck)
    {
        if (deck == null || deck.cards.Count == 0)
        {
            return;
        }

        System.Random rng = new System.Random();
        int n = deck.cards.Count;

        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            DemoCard temp = deck.cards[k];
            deck.cards[k] = deck.cards[n];
            deck.cards[n] = temp;
        }

    }

    // Return number of cards
    public int Count(DemoCardDeck deck)
    {
        if (deck == null) return 0;
        return deck.cards.Count;
    }
}
