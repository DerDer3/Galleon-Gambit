using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

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

    // ------------------------------------------------------------
    // STRESS TEST: Add and draw 10,000 cards to test performance
    // ------------------------------------------------------------
    [Test]
    public void StressTest_AddAndDrawLargeNumberOfCards()
    {
        const int largeCount = 10000;

        // Add a large number of cards
        for (int i = 0; i < largeCount; i++)
        {
            Card newCard = ScriptableObject.CreateInstance<Card>();
            newCard.cardName = "StressCard_" + i;
            playerDeck.AddCard(newCard, "Generic");
        }

        Assert.AreEqual(largeCount, playerDeck.cards.Count, "Not all cards were added.");

        // Draw all cards and ensure no nulls are returned
        for (int i = 0; i < largeCount; i++)
        {
            Card drawn = playerDeck.DrawCard();
            Assert.NotNull(drawn, $"Card #{i} was null on draw.");
        }

        Assert.AreEqual(0, playerDeck.cards.Count, "Deck should be empty after drawing all cards.");
    }

    // ------------------------------------------------------------
    // BOUNDARY TEST #1: Empty deck and discard pile reshuffle
    // ------------------------------------------------------------
    [Test]
    public void BoundaryTest_EmptyDeckAndDiscardPile()
    {
        // Ensure both deck and discard are empty
        Assert.AreEqual(0, playerDeck.cards.Count);
        Assert.AreEqual(0, playerDeck.discardPile.DiscardCount());

        // Try to draw a card (should return null)
        Card result = playerDeck.DrawCardWithReshuffle();

        Assert.IsNull(result, "Expected null card when both deck and discard are empty.");
        Assert.AreEqual(0, playerDeck.cards.Count, "Deck should remain empty.");
    }

    // ------------------------------------------------------------
    // BOUNDARY TEST #2: Discard pile with exactly one card reshuffles
    // ------------------------------------------------------------
    [Test]
    public void BoundaryTest_SingleCardInDiscardPileReshuffles()
    {
        // Empty deck
        playerDeck.cards.Clear();

        // Add one card to discard pile
        Card discardCard = ScriptableObject.CreateInstance<Card>();
        discardCard.cardName = "SingleCard";
        playerDeck.discardPile.AddToDiscard(discardCard);

        // Draw card with reshuffle (should pull from discard)
        Card drawn = playerDeck.DrawCardWithReshuffle();

        Assert.NotNull(drawn, "Expected to draw a card after reshuffle.");
        Assert.AreEqual("SingleCard", drawn.cardName);
        Assert.AreEqual(0, playerDeck.discardPile.DiscardCount(), "Discard pile should be empty after reshuffle.");
    }
}
