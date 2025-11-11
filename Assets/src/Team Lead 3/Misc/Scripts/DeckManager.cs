using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GallionGambit;

public class DeckManager : MonoBehaviour
{
    public List<NewCard> totalCards = new List<NewCard>();

    private int index = 0;

    public void DrawCard(HandManager handManager)
    {
        if (totalCards.Count == 0) {  return; }

        NewCard nextCard = totalCards[index];
        handManager.AddToHand(nextCard);
        index = (index+1) % totalCards.Count;

    }

    private void Start()
    {
        NewCard[] cards = Resources.LoadAll<NewCard>("CardData");

        totalCards.AddRange(cards);

    }

}
