The card prefab is used for the summoning and automated creation of cards in Galleon Gambit. Cards created in the Card Data folder will automatically be applicable to be used in this prefab when called in the Gameplay scene.
# Hierarchy
- CardPrefab
	- CardVisuals
		- CardCanvas
		- Highlight (Invisible until activated)
		- CardImage
		- Traits
			- Health
				- HealthNum
			- Type
				- HealTypeIMG
				- ManaTypeIMG
				- DamageTypeIMG
			- Damage
				- DamageNUM
			- Mana
				- ManaIMG
				- ManaNum
			- BGImage
			- Name

# Usage
### Found in Hand Manager

```C#
public GameObject cardPrefab;
```

The card prefab is cloned and added visually to the player's hand in the script HandManager.cs inside the function AddToHand():

```C#
    public void AddToHand(NewCard cardData)
    {
        if (cardsInHand.Count < maxHand)
        {
            GameObject newCard = Instantiate(cardPrefab, handTransform.position, Quaternion.identity, handTransform);
            cardsInHand.Add(newCard);

            CardDisplay display = newCard.GetComponent<CardDisplay>();
            display.cardData = cardData;
            display.UpdateCardDisplay();
        }

        UpdateHandVisuals();
    }
```

### Appearance Decided in Card Display
The card prefab's design and application to card data is done in the script CardDisplay.cs where the information from the card variable is called and used to decide what should be activated in the design, colors used, and the functionality of the prefab.

### Used in Card Movement
While not explicity mentioned, CardMovement.cs takes the card prefabs from HandManager.cs and moves them to respond to player input (I.e., hovered over and played).