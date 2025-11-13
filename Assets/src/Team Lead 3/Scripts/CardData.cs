using UnityEngine;
using GallionGambit; // Assuming NewCard is in this namespace
using TMPro;

// NOTE: Ensure this file is the ONLY place where the CardObject class is defined.
// The CardObject component lives on the instantiated CardPrefab.
public class CardData : MonoBehaviour
{
    // Mana cost exposed to CardMovement
    public int ManaCost { get; private set; } = 0;

    // Reference to the CardDisplay component for visual data access
    private CardDisplay cardDisplay;

    void Awake()
    {
        // Get the CardDisplay component on this same GameObject
        cardDisplay = GetComponent<CardDisplay>();
        if (cardDisplay == null)
        {
            Debug.LogError("CardObject requires a CardDisplay component on the same GameObject.");
        }
    }

    // Public method to initialize the card (called by DeckManager/HandManager)
    // HandManager calls this upon drawing a card
    public void SetCard(NewCard data, GameManager2 gameManager)
    {
        if (data == null)
        {
            Debug.LogError("SetCard called with null CardData! Card is unplayable.");
            ManaCost = 999;
            return;
        }

        // Set the card data on the CardDisplay component
        if (cardDisplay != null)
        {
            cardDisplay.cardData = data;
            // Optionally update the visuals here, or rely on CardDisplay's Start/Awake if possible
            cardDisplay.UpdateCardDisplay();
        }

        // Use the absolute value of NewCard.mana to determine the cost.
        // Based on BlunderbussGun.asset: mana: -3 -> cost 3
        ManaCost = Mathf.Abs(data.mana);

        // Debug check for setup verification
        Debug.Log($"Initialized Card: {data.cardName} with Mana Cost: {ManaCost}");
    }
}