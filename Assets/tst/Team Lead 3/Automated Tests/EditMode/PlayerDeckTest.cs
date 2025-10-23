using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class PlayerDeckTests
{
    private PlayerDeck playerDeck;

    [SetUp]
    public void Setup()
    {
        playerDeck = new GameObject().AddComponent<PlayerDeck>();
    }

    //BOUNDARY TEST #1: Empty deck and discard pile reshuffle
    [Test]
    public void BoundaryTest_EmptyDeckAndDiscardPile()
    {
        // Make sure both are empty
        Assert.AreEqual(0, playerDeck.cards.Count);
        Assert.AreEqual(0, playerDeck.discardPile.DiscardCount());

        // Attempt to draw when both are empty
        Card result = playerDeck.DrawCardWithReshuffle();

        Assert.IsNull(result, "Expected null when both deck and discard pile are empty.");
        Assert.AreEqual(0, playerDeck.cards.Count, "Deck should remain empty.");
    }

    //BOUNDARY TEST #2: Discard pile contains exactly one card
    [Test]
    public void BoundaryTest_SingleCardInDiscardPileReshuffles()
    {
        // Empty deck
        playerDeck.cards.Clear();

        // Add a single card to discard pile
        Card singleCard = new ShipRepair();
        playerDeck.discardPile.AddToDiscard(singleCard);

        // Drawing should trigger reshuffle and retrieve that card
        Card drawn = playerDeck.DrawCardWithReshuffle();

        Assert.NotNull(drawn, "Expected to draw a card after reshuffle.");
        Assert.AreEqual("Ship Repair", drawn.cardName, "The drawn card should match the discarded card.");
        Assert.AreEqual(0, playerDeck.discardPile.DiscardCount(), "Discard pile should be empty after reshuffle.");
    }
}
