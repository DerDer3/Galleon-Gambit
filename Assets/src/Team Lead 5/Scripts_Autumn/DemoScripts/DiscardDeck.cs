using UnityEngine;
using System.Collections.Generic;

public class DemoDiscardDeck : DemoCardDeck
{
    // Move a played or discarded card into this discard pile
    public void AddToDiscard(DemoCard card)
    {
        if (card == null)
        {
            return;
        }

        cards.Add(card);
        Debug.Log($"Card discarded: {card.cardName}");
    }

    // Move all cards from discard pile back into another deck with them already shuffled.
    public void ReshuffleInto(DemoCardDeck targetDeck)
    {
        if (cards.Count == 0)
        {
            return;
        }

        targetDeck.AddCards(new List<DemoCard>(cards));
        cards.Clear();
        targetDeck.Shuffle(targetDeck);

    }

    public int DiscardCount()
    {
        return cards.Count;
    }
}
