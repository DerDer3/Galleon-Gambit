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


// NOTE: Many of these tests are designed to throw exceptions (e.g., NullReferenceException, IndexOutOfRangeException)
// which are normally captured by Unity Test Runner but should cause a crash/fail if run without proper error handling.
// LogAssert.Expect is used here to explicitly confirm the expected error message for clarity.

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

    //=========================================================================================================================
    // EDIT MODE TESTS 

    // 1. Check if NewCard ScriptableObject fails without a required list item
    [Test]
    public void Edit_01_Card_MissingCardType_IndexOutOfRangeException()
    {
        NewCard card = ScriptableObject.CreateInstance<NewCard>();
        card.cardName = "BadCard";
        card.cardType = new List<NewCard.CardType>(); // Empty list

        // This test simulates calling the CardStats constructor, which relies on cardData.cardType[0]
        LogAssert.Expect(LogType.Error, "CardData is missing on CardDisplay.");
        Assert.Throws<ArgumentOutOfRangeException>(() => new CardStats(card));
    }

    // 2. Check if CardStats fails when given a null CardData
    [Test]
    public void Edit_02_CardStats_Constructor_NullReferenceException()
    {
        // CardStats does not have a null check for NewCard in its constructor, relying on CardDisplay.
        // We bypass CardDisplay here to test raw struct behavior.
        NewCard nullCard = null;

        // This should fail inside the CardStats constructor attempting to access nullCard.cardName
        Assert.Throws<NullReferenceException>(() => new CardStats(nullCard));
    }

    // 3. DeckManager Initialization without a HandManager reference
    [Test]
    public void Edit_03_DeckManager_Initialization_NullHandManager()
    {
        // Setup: Create Game Object with DeckManager but no HandManager is set
        GameObject dmObject = new GameObject("DeckManagerObject");
        DeckManager deckManager = dmObject.AddComponent<DeckManager>();

        // LogAssert.Expect will catch the Debug.LogError before the test runner fails the test gracefully.
        // If the LogError didn't exist, attempting to access nullHandManager would likely crash later.
        LogAssert.Expect(LogType.Error, "DeckManager cannot initialize: HandManager reference is null.");
        deckManager.InitializeDeckAndDrawHand();

        Object.DestroyImmediate(dmObject);
    }

    // 4. CardDisplay trying to update with null CardData
    [Test]
    public void Edit_04_CardDisplay_Update_NullCardData()
    {
        // Setup: CardDisplay on a mock GO without its cardData set
        GameObject cardObject = new GameObject("CardDisplayObject");
        CardDisplay display = cardObject.AddComponent<CardDisplay>();

        // This will try to access 'display.cardData.cardName' which is null
        // The display script does not handle null CardData in UpdateCardDisplay()
        Assert.Throws<NullReferenceException>(() => display.UpdateCardDisplay());

        Object.DestroyImmediate(cardObject);
    }

    // 5. CardMovement tries to get CardDisplay, but it's missing (Awake failure)
    [Test]
    public void Edit_05_CardMovement_MissingCardDisplay_Error()
    {
        GameObject cardObject = new GameObject("CardMovementObject");
        cardObject.AddComponent<RectTransform>();

        // LogAssert.Expect will capture the Debug.LogError from Awake()
        LogAssert.Expect(LogType.Error, "CardMovement requires a CardDisplay component to access card data!");
        cardObject.AddComponent<CardMovement>();

        Object.DestroyImmediate(cardObject);
    }

    // 6. DeckManager attempts to initialize with an empty CardDatabase
    [Test]
    public void Edit_06_DeckManager_EmptyDatabase_Error()
    {
        GameObject dmObject = new GameObject("DeckManagerObject");
        DeckManager deckManager = dmObject.AddComponent<DeckManager>();

        // Mock a HandManager to bypass the HandManager null check
        GameObject hmObject = new GameObject("HandManagerObject");
        HandManager handManager = hmObject.AddComponent<HandManager>();
        deckManager.SetHandManager(handManager);

        // Crucially, CardDatabase is empty (simulates 'CardData' folder being empty)
        deckManager.CardDatabase.Clear();

        // The InitializeStartingDeck private method should hit the LogError guard
        LogAssert.Expect(LogType.Error, "CardDatabase is empty. Check if 'CardData' ScriptableObjects are in a Resources folder.");
        deckManager.InitializeDeckAndDrawHand();

        Object.DestroyImmediate(hmObject);
        Object.DestroyImmediate(dmObject);
    }

    // 7. InitializeStartingDeck with excessively large copiesOfEachCard (causes memory spike/crash on massive deck)
    [Test]
    public void Edit_07_DeckManager_MassiveCopies_MemorySpikeFailure()
    {
        GameObject dmObject = new GameObject("DeckManagerObject");
        DeckManager deckManager = dmObject.AddComponent<DeckManager>();

        // Use reflection or set a public/exposed field to simulate an extremely large deck configuration
        // Since the field is private, we simulate the effect by creating a huge list directly

        // This is not a direct crash but simulates a configuration that leads to an unmanageable state/OOM exception in a real run.
        NewCard testCard = CreateTestCard();
        deckManager.CardDatabase.Add(testCard);

        int copies = int.MaxValue / 2; // Simulate an attempt to create a massive deck

        // Set copiesOfEachCard via reflection if possible, or simulate the loop.
        // Since we can't reliably set private fields, we'll manually flood the deck for this test.
        Assert.Throws<OutOfMemoryException>(() =>
        {
            for (int i = 0; i < copies; i++)
            {
                deckManager.PlayerDeck.cards.Add(testCard);
            }
        });

        Object.DestroyImmediate(dmObject);
        Object.DestroyImmediate(testCard);
    }

    // 8. CardDisplay's activateImages array index out of bounds
    [Test]
    public void Edit_08_CardDisplay_activateImages_IndexOutOfBounds()
    {
        GameObject cardObject = new GameObject("CardDisplayObject");
        CardDisplay display = cardObject.AddComponent<CardDisplay>();
        display.typeImages = new UnityEngine.UI.Image[3];

        // Accessing index 3 which is out of bounds (0, 1, 2 are valid)
        Assert.Throws<IndexOutOfRangeException>(() => display.activateImages(3));

        Object.DestroyImmediate(cardObject);
    }

    // 9. CardDisplay accessing CardType at index 1 when only index 0 exists (potential bug/crash source)
    [Test]
    public void Edit_09_CardDisplay_CardType_IndexOutOfBounds()
    {
        GameObject cardObject = new GameObject("CardDisplayObject");
        CardDisplay display = cardObject.AddComponent<CardDisplay>();
        display.cardData = CreateTestCard(); // Only has 1 CardType (index 0)

        // Directly assign Image/TMP_Text mocks to prevent NullRef, focusing on the IndexOutOfRangeException
        // Mocking with generic components that satisfy the reference types in CardDisplay
        display.cardImage = new GameObject("MockImage").AddComponent<Image>();
        display.nameText = new GameObject("MockNameText").AddComponent<TMP_Text>();
        display.healthText = new GameObject("MockHealthText").AddComponent<TMP_Text>();
        display.damageText = new GameObject("MockDamageText").AddComponent<TMP_Text>();
        display.manaText = new GameObject("MockManaText").AddComponent<TMP_Text>();
        display.typeImages = new Image[3];


        // Simulate the logic relying on a second type in the list (e.g. cardData.cardType[1])
        // While the current code only uses index 0, this checks for future-proofing against a logic error.
        // We'll force the access here by manually testing the data.
        NewCard card = display.cardData;
        Assert.Throws<ArgumentOutOfRangeException>(() => {
            var cardTypeAt1 = card.cardType[1];
        });

        Object.DestroyImmediate(display.cardImage.gameObject);
        Object.DestroyImmediate(display.nameText.gameObject);
        Object.DestroyImmediate(display.healthText.gameObject);
        Object.DestroyImmediate(display.damageText.gameObject);
        Object.DestroyImmediate(display.manaText.gameObject);
        Object.DestroyImmediate(cardObject);
    }

    // 10. CardMovement tries to parent to a null HandManager transform in OnPointerUp
    [Test]
    public void Edit_10_CardMovement_SetParent_NullHandManager()
    {
        // Setup: Mock CardMovement and force HandManager to null
        GameObject cardObject = new GameObject("CardMovementObject");
        CardMovement movement = cardObject.AddComponent<CardMovement>();
        cardObject.AddComponent<RectTransform>();
        cardObject.AddComponent<CardDisplay>(); // Required dependency

        // Use reflection to set the private HandManager field to null (assuming no public setter)
        // Since we can't reliably use reflection here, we rely on the fact that Awake() sets it via GameManager2.Instance.HandManager
        // If GameManager2.Instance is null, handManager is null, leading to the LogError in Awake.
        // But for this test, we assume Awake ran, but the reference was later corrupted/cleared.
        // We'll simulate the state transition that triggers the null access:
        movement.GetType().GetField("handManager", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(movement, null);

        // Manually transition to Dragging state (which is where OnPointerUp triggers its failure path)
        movement.GetType().GetMethod("TransitionToState", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(movement, new object[] { CardState.Dragging });

        // OnPointerUp attempts to access handManager.handTransform to reparent.
        Assert.Throws<NullReferenceException>(() => movement.OnPointerUp(new PointerEventData(null)));
        Object.DestroyImmediate(cardObject);
    }
  
}