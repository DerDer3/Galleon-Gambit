using UnityEngine;

public class PlayerDeck : CardDeck
{
    public DiscardDeck discardPile = new DiscardDeck();

    // Draw a card and automatically handle empty deck reshuffle if needed
    public Card DrawCardWithReshuffle()
    {
        if (cards.Count == 0)
        {
            discardPile.ReshuffleInto(this);
        }

        Card drawn = DrawCard();
        return drawn;
    }

    // After playing a card, move it to the discard pile
    public void DiscardCard(Card card)
    {
        discardPile.AddToDiscard(card);
    }
}
