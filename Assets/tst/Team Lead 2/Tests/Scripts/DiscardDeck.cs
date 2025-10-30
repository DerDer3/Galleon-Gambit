using UnityEngine;
using System.Collections.Generic;

public class DiscardDeck : CardDeck
{
    // Move a played or discarded card into this discard pile
    public void AddToDiscard(Card card)
    {
        if (card == null)
        {
            return;
        }

        cards.Add(card);
        Debug.Log($"Card discarded: {card.cardName}");
    }

    // Move all cards from discard pile back into another deck with them already shuffled.
    public void ReshuffleInto(CardDeck targetDeck)
    {
        if (cards.Count == 0)
        {
            return;
        }

        targetDeck.AddCards(new List<Card>(cards));
        cards.Clear();
        targetDeck.Shuffle(targetDeck);

    }

    public int DiscardCount()
    {
        return cards.Count;
    }
}
