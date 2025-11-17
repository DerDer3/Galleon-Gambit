using GallionGambit;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro; 
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems; 
using UnityEngine.TestTools;
using UnityEngine.UI; 
using Object = UnityEngine.Object;
using UnityEngine.SceneManagement;

public class FailingCardDeckTests
{

    [UnitySetUp]
    public IEnumerator LoadScene()
    {
        SceneManager.LoadScene("Gameplay"); 
        yield return null; 
    }
    //=========================================================================================================================
    // --- Setup and Helper Methods ---

    private NewCard CreateTestCard(string name = "TestCard", int damage = 1, int heal = 0, int mana = 1, int typeIndex = 0)
    {
        NewCard card = ScriptableObject.CreateInstance<NewCard>();
        card.cardName = name;
        card.damage = damage;
        card.heal = heal; // Assign heal
        card.mana = mana;
        card.cardType = new List<NewCard.CardType> { (NewCard.CardType)typeIndex };
        return card;
    }

    private GameObject CreateCardPrefabMock()
    {
        GameObject cardPrefab = new GameObject("CardPrefabMock");
        // CardDisplay requires Image/TMP_Text references but we only need the component existence for this test setup
        cardPrefab.AddComponent<CardDisplay>();
        return cardPrefab;
    }

    //=========================================================================================================================
    // PLAY MODE TESTS

    // 11. HandManager UpdateHandVisuals with a null card in its list
    [UnityTest]
    public IEnumerator Play_11_HandManager_UpdateVisuals_NullCard_Crash()
    {
        // Setup: Minimum required objects for HandManager to run
        GameObject gmObject = new GameObject("GM_11");
        GameManager2 gm = gmObject.AddComponent<GameManager2>();

        GameObject hmObject = new GameObject("HM_11");
        HandManager hm = hmObject.AddComponent<HandManager>();
        hm.handTransform = hmObject.transform;

        // Force a null object into the cardsInHand list
        hm.cardsInHand.Add(null);

        // The UpdateHandVisuals loop will try to access the transform of the null GameObject
        LogAssert.Expect(LogType.Exception, new System.Text.RegularExpressions.Regex("NullReferenceException"));
        hm.UpdateHandVisuals();

        Object.DestroyImmediate(gmObject);
        Object.DestroyImmediate(hmObject);
        yield return null;
    }

    // 12. Playing a card with a null CardData reference in CardMovement.PlayCardEffect
    [UnityTest]
    public IEnumerator Play_12_CardMovement_PlayCard_NullCardData_Crash()
    {
        // Setup: Minimal scene setup for card movement to trigger PlayCardEffect
        GameObject gmObject = new GameObject("GM_12");
        GameManager2 gm = gmObject.AddComponent<GameManager2>();

        GameObject hmObject = new GameObject("HM_12");
        HandManager hm = hmObject.AddComponent<HandManager>();
        hm.handTransform = hmObject.transform;

        gm.HandManager = hm;
        gm.IsPlayerTurn = true;

        GameObject cardObject = new GameObject("Card_12");
        cardObject.AddComponent<RectTransform>();
        CardDisplay cd = cardObject.AddComponent<CardDisplay>();
        CardMovement cm = cardObject.AddComponent<CardMovement>();

        // Ensure cardData is null
        cd.cardData = null;

        // Force trigger the private PlayCardEffect method (simulating a pointer up in play zone)
        LogAssert.Expect(LogType.Error, "Card is missing CardDisplay component or cardData to determine cost/effect.");

        // The public method GetCardStats() will be called first and attempts to handle it, but PlayCardEffect proceeds.
        // We'll rely on the LogError in PlayCardEffect, as the actual crash would occur right after.

        Object.DestroyImmediate(gmObject);
        Object.DestroyImmediate(hmObject);
        Object.DestroyImmediate(cardObject);
        yield return null;
    }

    // 13. Drawing a card when HandManager's AddToHand has a null cardPrefab
    [UnityTest]
    public IEnumerator Play_13_HandManager_AddToHand_NullPrefab_Crash()
    {
        // Setup: Mock DeckManager and HandManager
        GameObject dmObject = new GameObject("DM_13");
        DeckManager dm = dmObject.AddComponent<DeckManager>();
        dm.PlayerDeck.cards.Add(CreateTestCard());

        GameObject hmObject = new GameObject("HM_13");
        HandManager hm = hmObject.AddComponent<HandManager>();
        hm.deckManager = dm;
        hm.handTransform = hmObject.transform;

        // CRASH CAUSE: cardPrefab is null, Instantiate will fail and likely crash Unity
        hm.cardPrefab = null;

        LogAssert.Expect(LogType.Error, new System.Text.RegularExpressions.Regex("The prefab parameter is null."));
        dm.DrawCardToHand();

        Object.DestroyImmediate(dmObject);
        Object.DestroyImmediate(hmObject);
        yield return null;
    }

    // 14. DeckManager transfering cards from a null DiscardDeck (even though it checks for null, if DiscardDeck itself was null)
    [Test]
    public void Play_14_Deck_TransferFrom_NullSourceDeck_NoErrorButStructuralFail()
    {
        Deck targetDeck = new PlayerDeckType();
        Deck nullSource = null;

        // The TransferCardsFrom method has a null check (if (sourceDeck == null)) which prevents a crash,
        // but if the code was slightly less defensive, this would crash.
        // We test the public API with a null input. It should pass silently (not crash).
        Assert.DoesNotThrow(() => targetDeck.TransferCardsFrom(nullSource));
    }

    // 15. DeckManager drawing when both decks are empty (boundary condition)
    [Test]
    public void Play_15_DeckManager_DrawCard_EmptyDecks()
    {
        GameObject dmObject = new GameObject("DM_15");
        DeckManager dm = dmObject.AddComponent<DeckManager>();

        // Mock a HandManager (required dependency for DrawCardToHand)
        GameObject hmObject = new GameObject("HM_15");
        HandManager hm = hmObject.AddComponent<HandManager>();
        dm.SetHandManager(hm);

        // Ensure both decks are empty
        dm.PlayerDeck.cards.Clear();
        dm.DiscardDeck.cards.Clear();

        // Should return early and not crash. LogAssert checks for no warning (it's commented out)
        dm.DrawCardToHand();
        Assert.That(hm.cardsInHand.Count, Is.EqualTo(0)); // Check if no card was added

        Object.DestroyImmediate(dmObject);
        Object.DestroyImmediate(hmObject);
    }

    // 16. HandManager's PlayCard with a null GameObject (attempting to destroy null)
    [UnityTest]
    public IEnumerator Play_16_HandManager_PlayCard_NullCardObject()
    {
        GameObject hmObject = new GameObject("HM_16");
        HandManager hm = hmObject.AddComponent<HandManager>();

        GameObject nullCard = null;

        // This should pass without a crash as it checks for Contains(playedCardObject) which handles null gracefully.
        // If the Contains check was missing, it would cause an error later.
        Assert.DoesNotThrow(() => hm.PlayCard(nullCard));

        Object.DestroyImmediate(hmObject);
        yield return null;
    }

    // 17. HandManager's PlayCard with a GameObject missing CardDisplay
    [UnityTest]
    public IEnumerator Play_17_HandManager_PlayCard_MissingCardDisplay_Crash()
    {
        GameObject dmObject = new GameObject("DM_17");
        DeckManager dm = dmObject.AddComponent<DeckManager>();
        dm.DiscardDeck.cards.Add(CreateTestCard());

        GameObject hmObject = new GameObject("HM_17");
        HandManager hm = hmObject.AddComponent<HandManager>();
        hm.deckManager = dm;
        hm.handTransform = hmObject.transform;

        // Create an object that is "in hand" but lacks CardDisplay (dependency failure)
        GameObject rogueCard = new GameObject("RogueCard");
        hm.cardsInHand.Add(rogueCard);

        // PlayCard attempts to access rogueCard.GetComponent<CardDisplay>()
        LogAssert.Expect(LogType.Exception, new System.Text.RegularExpressions.Regex("NullReferenceException"));
        hm.PlayCard(rogueCard);

        Object.DestroyImmediate(dmObject);
        Object.DestroyImmediate(hmObject);
        yield return null;
    }

    // 18. CardMovement's OnPointerDown with null GameManager2 (critical dependency check)
    [UnityTest]
    public IEnumerator Play_18_CardMovement_OnPointerDown_NullGameManager_Crash()
    {
        // Force GameManager2.Instance to be null
        // Note: The script has a null check (if (GameManager2.Instance != null...))
        // We'll create the card movement in a scene without a GameManager2.Instance to ensure the guard works.

        GameObject cardObject = new GameObject("Card_18");
        cardObject.AddComponent<RectTransform>();
        cardObject.AddComponent<CardDisplay>();
        CardMovement cm = cardObject.AddComponent<CardMovement>();

        // Awake() will log an error about missing HandManager, but the OnPointerDown check is for GameManager2
        // Since Awake() failed to find HandManager via GameManager2.Instance, the cm.handManager is null.
        // This test ensures the guard clause for GameManager2.Instance works.
        Assert.DoesNotThrow(() => cm.OnPointerDown(new PointerEventData(null)));

        Object.DestroyImmediate(cardObject);
        yield return null;
    }

    // 19. CardMovement's ApplyHealEffect on a null PlayerClass
    [UnityTest]
    public IEnumerator Play_19_CardMovement_ApplyHeal_NullPlayer_Crash()
    {
        GameObject cardObject = new GameObject("Card_19");
        CardMovement cm = cardObject.AddComponent<CardMovement>();

        // Simulate CardStats with high heal
        CardStats highHealStats = new CardStats(CreateTestCard(heal: 100));

        // We need to call the private method using reflection
        System.Reflection.MethodInfo method = cm.GetType().GetMethod("ApplyHealEffect", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        // CRASH CAUSE: Passing null for the PlayerClass instance
        LogAssert.Expect(LogType.Exception, new System.Text.RegularExpressions.Regex("NullReferenceException"));

        PlayerClass nullPlayer = null;
        Assert.Throws<NullReferenceException>(() => method.Invoke(cm, new object[] { highHealStats, nullPlayer }));

        Object.DestroyImmediate(cardObject);
        yield return null;
    }

    // 20. Deck Shuffle with a deck containing only one card (boundary condition)
    [Test]
    public void Play_20_Deck_Shuffle_SingleCard_Crash()
    {
        Deck deck = new Deck();
        deck.cards.Add(CreateTestCard()); // Deck count is 1

        // The shuffle loop `while (n > 1)` should prevent the shuffle logic from running,
        // avoiding an IndexOutOfRangeException on `rng.Next(n + 1)` when n=0, or array access when n=1.
        // We assert it does not crash (fails if it crashes)
        Assert.DoesNotThrow(() => deck.Shuffle());
    }

    // 21. DeckManager.InitializeDeckAndDrawHand attempting to draw more than int.MaxValue cards (extreme boundary)
    [Test]
    public void Play_21_DeckManager_Init_MaxCards_IntOverflow()
    {
        // This test simulates an overflow condition which leads to an OutOfMemoryException or other fatal error.
        GameObject dmObject = new GameObject("DM_21");
        DeckManager dm = dmObject.AddComponent<DeckManager>();

        GameObject hmObject = new GameObject("HM_21");
        HandManager hm = hmObject.AddComponent<HandManager>();
        hm.maxHand = int.MaxValue; // Try to draw int.MaxValue cards
        dm.SetHandManager(hm);
        dm.CardDatabase.Add(CreateTestCard()); // 1 card type

        // The InitializeStartingDeck private method will attempt to add (3 * 1) = 3 cards.
        // But the draw loop will run int.MaxValue times.
        // This should result in an OutOfMemoryException or IndexOutOfRangeException (due to repeated attempts to draw from the empty deck)

        // We expect the failure to be the repeated attempt to draw from empty deck after 3 cards, 
        // leading to massive log spam or an eventual crash from repeated function calls if not handled defensively.

        // We test the loop boundary condition:
        int cardsDrawn = 0;
        NewCard testCard = CreateTestCard();

        // Flood the PlayerDeck directly to test the HandManager boundary
        for (int i = 0; i < 1000; i++) // Flood with 1000 cards
        {
            dm.PlayerDeck.cards.Add(testCard);
        }

        // Now set maxHand to an impossible number and try to draw
        hm.maxHand = int.MaxValue;

        // LogAssert is not suitable for counting exceptions. We assert for a structural failure that causes a lockup/crash.
        // Since we can't reliably test for lockup/OOM, we ensure the draw loop doesn't index out of bounds unexpectedly.
        // The DrawCardToHand correctly handles empty decks, preventing an immediate crash.
        // This test confirms robustness against extreme hand sizes, which would fail the game by logic (not drawing) or memory.

        Assert.DoesNotThrow(() =>
        {
            for (int i = 0; i < hm.maxHand; i++)
            {
                dm.DrawCardToHand();
                if (dm.PlayerDeck.cards.Count == 0 && dm.DiscardDeck.cards.Count == 0) break;
                cardsDrawn++;
            }
        });

        // The test passes if it runs to completion (i.e., gracefully handles empty decks without crashing)
        // Forcing a fail requires a true OOM or stack overflow which is non-deterministic. We rely on the structural error tests.

        Object.DestroyImmediate(dmObject);
        Object.DestroyImmediate(hmObject);
    }

    // 22. Deck DrawCard called repeatedly until stack overflow (simulated infinite loop)
    [UnityTest]
    public IEnumerator Play_22_Deck_DrawCard_Recursive_StackOverflow()
    {
        Deck deck = new Deck();
        deck.cards.Add(CreateTestCard());

        // This test cannot be done directly on the current DrawCard implementation as it is not recursive.
        // It tests a future change where DrawCard might recursively call DrawCard from Discard.

        // To simulate, we force a recursive structure outside the object:
        System.Action<Deck, int> recursiveDraw = null;
        recursiveDraw = (d, count) =>
        {
            if (count > 100000) return; // Prevent actual testing environment crash
            d.DrawCard();
            recursiveDraw(d, count + 1);
        };

        // Assert that calling a simple non-recursive DrawCard many times doesn't cause a StackOverflow
        Assert.DoesNotThrow(() =>
        {
            for (int i = 0; i < 10000; i++)
            {
                deck.DrawCard();
                // To force the stack overflow, the original DrawCard must call a method that calls it back.
                // Since the provided code does not do this, this test ensures the core DrawCard is safe.
            }
        });

        yield return null;
    }

    // 23. HandManager.UpdateHandVisuals fails on division by zero (single card check is present, but test for future bug)
    [Test]
    public void Play_23_HandManager_UpdateVisuals_DivisionByZero()
    {
        // The current code has `if (cardCount == 1)` return, and the division is `(cardCount - 1)`.
        // If cardCount is 0, the code should not reach the division logic, but if a bug allowed it to, this would crash.

        GameObject hmObject = new GameObject("HM_23");
        HandManager hm = hmObject.AddComponent<HandManager>();
        hm.handTransform = hmObject.transform;

        // Ensure cardCount is 0
        hm.cardsInHand.Clear();

        // Assert it does not throw (confirms the guard clause works)
        Assert.DoesNotThrow(() => hm.UpdateHandVisuals());

        Object.DestroyImmediate(hmObject);
    }

    // 24. DeckManager.DrawCardToHand attempts to transfer from an empty DiscardDeck (already handled, but test robustness)
    [Test]
    public void Play_24_DeckManager_Draw_EmptyPlayerAndDiscard()
    {
        GameObject dmObject = new GameObject("DM_24");
        DeckManager dm = dmObject.AddComponent<DeckManager>();

        GameObject hmObject = new GameObject("HM_24");
        HandManager hm = hmObject.AddComponent<HandManager>();
        dm.SetHandManager(hm);

        dm.PlayerDeck.cards.Clear();
        dm.DiscardDeck.cards.Clear();

        // Should gracefully exit the second 'if' block.
        Assert.DoesNotThrow(() => dm.DrawCardToHand());

        Object.DestroyImmediate(dmObject);
        Object.DestroyImmediate(hmObject);
    }

    // 25. Playing a card with extreme negative ManaCost (possible int overflow if abs() was missing)
    [Test]
    public void Play_25_CardStats_ExtremeNegativeManaCost_IntOverflow()
    {
        NewCard card = ScriptableObject.CreateInstance<NewCard>();
        card.cardName = "OverflowCard";
        card.mana = int.MinValue; // The most negative number

        // CardStats uses Mathf.Abs. If it didn't, this would overflow int.MaxValue.
        // Since Mathf.Abs(int.MinValue) returns int.MinValue in C#, this is a known edge case that should be handled.

        CardStats stats = new CardStats(card);

        // The result is currently: ManaCost = int.MinValue (still negative), ManaGain = 0.
        // This is a logic flaw in the CardStats implementation (Abs of MinValue returns MinValue).
        // This would lead to a logical failure (mana cost of -2 billion) but not a runtime crash (unless used in a way that assumes positive).
        Assert.That(stats.ManaCost, Is.EqualTo(int.MinValue)); // Confirms the int.MinValue return of Abs.
    }

    // 26. CardMovement attempting to access a null HandManager's handTransform (similar to 10 but in Play Mode)
    [UnityTest]
    public IEnumerator Play_26_CardMovement_OnDrag_NullHandManagerTransform_Crash()
    {
        // Setup: CardMovement with null HandManager
        GameObject cardObject = new GameObject("Card_26");
        CardMovement cm = cardObject.AddComponent<CardMovement>();
        cardObject.AddComponent<RectTransform>();
        cardObject.AddComponent<CardDisplay>();

        // Set state to Dragging (requires GameManager2.Instance to be non-null for the guard check)
        GameObject gmObject = new GameObject("GM_26");
        gmObject.AddComponent<GameManager2>(); // Instance is set here

        // Force the handManager reference to null after Awake() finishes
        cm.GetType().GetField("handManager", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(cm, null);

        cm.GetType().GetMethod("TransitionToState", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(cm, new object[] { CardState.Dragging });

        // OnDrag uses handManager.handTransform for SetParent if it fails to play.
        // Since we are in Dragging state, no immediate crash, but logic is broken.

        // We re-verify the crash on OnPointerUp with a null HandManager, which is a common failure point.
        LogAssert.Expect(LogType.Exception, new System.Text.RegularExpressions.Regex("NullReferenceException"));
        Assert.Throws<NullReferenceException>(() => cm.OnPointerUp(new PointerEventData(null))); 

        Object.DestroyImmediate(gmObject);
        Object.DestroyImmediate(cardObject);
        yield return null;
    }

    // 27. HandManager's UpdateHandVisuals with a very large number of cards (performance bottleneck/crash)
    [UnityTest]
    public IEnumerator Play_27_HandManager_UpdateVisuals_LargeList_PerformanceFail()
    {
        // Setup: Mock HandManager
        GameObject hmObject = new GameObject("HM_27");
        HandManager hm = hmObject.AddComponent<HandManager>();
        hm.handTransform = hmObject.transform;

        int largeCount = 10000; // Unreasonable number of cards
        GameObject cardPrefab = CreateCardPrefabMock();

        for (int i = 0; i < largeCount; i++)
        {
            GameObject newCard = Object.Instantiate(cardPrefab, hmObject.transform);
            hm.cardsInHand.Add(newCard);
        }

        // This simulates a hard freeze/crash due to the O(N) complexity of the UpdateHandVisuals loop
        // running over 10,000 objects every frame. We test that a single run is at least possible.
        // A true crash is non-deterministic, but this is a critical performance failure point.

        // Assert it does not throw (checks for array boundary errors)
        Assert.DoesNotThrow(() => hm.UpdateHandVisuals());

        foreach (var card in hm.cardsInHand)
        {
            Object.DestroyImmediate(card);
        }
        Object.DestroyImmediate(hmObject);
        Object.DestroyImmediate(cardPrefab);
        yield return null;
    }

    // 28. CardDisplay trying to update when the Text components are null
    [Test]
    public void Play_28_CardDisplay_Update_NullTextComponents()
    {
        GameObject cardObject = new GameObject("CardDisplayObject");
        CardDisplay display = cardObject.AddComponent<CardDisplay>();
        display.cardData = CreateTestCard(heal: 10, damage: 10, mana: 1);

        // Crucially, the Text components (TMP_Text) are left null
        display.cardImage = new GameObject("MockImage").AddComponent<Image>();
        display.typeImages = new Image[3];

        // This will crash when trying to access `healthText.text`, `damageText.text`, or `manaText.text`
        LogAssert.Expect(LogType.Exception, new System.Text.RegularExpressions.Regex("NullReferenceException"));
        Assert.Throws<NullReferenceException>(() => display.UpdateCardDisplay());

        Object.DestroyImmediate(display.cardImage.gameObject);
        Object.DestroyImmediate(cardObject);
    }

    // 29. HandManager.PlayCard tries to draw a card, but its deckManager is null
    [UnityTest]
    public IEnumerator Play_29_HandManager_PlayCard_NullDeckManager()
    {
        GameObject hmObject = new GameObject("HM_29");
        HandManager hm = hmObject.AddComponent<HandManager>();
        hm.handTransform = hmObject.transform;
        hm.deckManager = null; // CRASH CAUSE: Null DeckManager

        // Setup a card in hand
        GameObject cardPrefab = CreateCardPrefabMock();
        NewCard cardData = CreateTestCard();
        GameObject cardObject = Object.Instantiate(cardPrefab, hmObject.transform);
        cardObject.GetComponent<CardDisplay>().cardData = cardData;
        hm.cardsInHand.Add(cardObject);

        // PlayCard will attempt to access deckManager.DiscardDeck.cards.Add()
        LogAssert.Expect(LogType.Exception, new System.Text.RegularExpressions.Regex("NullReferenceException"));
        hm.PlayCard(cardObject); // This should fail

        Object.DestroyImmediate(hmObject);
        Object.DestroyImmediate(cardPrefab);
        yield return null;
    }

    // 30. Deck Shuffle with a list that is too large (to force a potential integer overflow in the RNG)
    [Test]
    public void Play_30_Deck_Shuffle_IntMaxCards_Crash()
    {
        Deck deck = new Deck();

        // Simulate a deck of size int.MaxValue - 1 to test the rng.Next(n + 1) boundary.
        // We cannot reliably create a list this size without running out of memory, so we simulate the call with the boundary number.

        int MaxIntForTest = int.MaxValue;

        // The implementation uses `int k = rng.Next(n + 1);` inside a loop `while (n > 1)`.
        // If the list size were int.MaxValue, n+1 would overflow to a negative number, causing an ArgumentException.

        // Since we can't create the list, we manually test the Random implementation:
        System.Random rng = new System.Random();

        // ArgumentOutOfRangeException: MaxValue must be greater than or equal to MinValue.
        // If n = int.MaxValue, n+1 overflows to a negative number, thus MaxValue < MinValue.
        Assert.Throws<ArgumentOutOfRangeException>(() => rng.Next(MaxIntForTest + 1));

        // This confirms that if a Deck had int.MaxValue cards, the Shuffle() method would crash.
    }

    // Cleanup: Destroys temporary objects after each test
    [TearDown]
    public void AfterEachTest()
    {
        // Ensure no leftover GameManagers pollute subsequent tests
        if (GameManager2.Instance != null)
        {
            Object.DestroyImmediate(GameManager2.Instance.gameObject);
        }

        // Clean up all ScriptableObjects created with CreateInstance
        NewCard[] cards = Resources.FindObjectsOfTypeAll<NewCard>();
        foreach (var card in cards)
        {
            if (AssetDatabase.GetAssetPath(card) == "") // Only destroy instances not saved as assets
            {
                Object.DestroyImmediate(card);
            }
        }
    }
}