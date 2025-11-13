using UnityEngine;
using System.Collections.Generic;
using System.Linq;

// Renamed from _CardDeck.cs
public class Deck
{
    // The actual list of cards in this deck (e.g., Draw Pile or Discard Pile)
    public List<GallionGambit.NewCard> cards = new List<GallionGambit.NewCard>();

    public GallionGambit.NewCard DrawCard()
    {
        if (cards.Count == 0)
        {
            return null;
        }

        // Draw the top card (index 0)
        GallionGambit.NewCard drawnCard = cards[0];
        cards.RemoveAt(0);

        return drawnCard;
    }

    public void Shuffle()
    {
        System.Random rng = new System.Random();
        int n = cards.Count;

        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            GallionGambit.NewCard temp = cards[k];
            cards[k] = cards[n];
            cards[n] = temp;
        }
    }

    public void TransferCardsFrom(Deck sourceDeck)
    {
        if (sourceDeck == null || sourceDeck.cards.Count == 0)
        {
            return;
        }

        cards.AddRange(sourceDeck.cards);
        sourceDeck.cards.Clear();
    }
}