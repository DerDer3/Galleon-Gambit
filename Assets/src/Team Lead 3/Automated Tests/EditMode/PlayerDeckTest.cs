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

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(playerDeck.gameObject);
    }

    //STRESS TEST: Add and draw a large number of cards (10,000)
    [Test]
    public void StressTest_AddAndDrawManyCards()
    {
        //controlled:
        const int largeCount = 10000;

        // Add a large number of Slash cards
        //while( true){ //for non controlled
        for (int i = 0; i < largeCount; i++)
        {
            playerDeck.AddCard(new Slash(), "Attack");
        }

        Assert.AreEqual(largeCount, playerDeck.cards.Count, "All cards should have been added.");

        // Draw all cards and ensure no nulls returned
        for (int i = 0; i < largeCount; i++)
        {
            Card drawn = playerDeck.DrawCard();
            Assert.NotNull(drawn, $"Card #{i} was null during draw.");
        }

        Assert.AreEqual(0, playerDeck.cards.Count, "Deck should be empty after drawing all cards.");
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
