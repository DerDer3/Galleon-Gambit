using UnityEngine;

public class DemoPlayerDeck : DemoCardDeck
{
    public DemoDiscardDeck discardPile = new DemoDiscardDeck();

    // Draw a card and automatically handle empty deck reshuffle if needed
    public DemoCard DrawCardWithReshuffle()
    {
        if (cards.Count == 0)
        {
            discardPile.ReshuffleInto(this);
        }

        DemoCard drawn = DrawCard();
        return drawn;
    }

    // After playing a card, move it to the discard pile
    public void DiscardCard(DemoCard card)
    {
        discardPile.AddToDiscard(card);
    }
}
