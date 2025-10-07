using UnityEngine;

public class CardObject : MonoBehaviour
{
    private Card linkedCard; // the data this visual represents

    public void SetCard(Card card)
    {
        linkedCard = card;
    }

    public void OnMouseDown()
    {
        linkedCard.Play();
    }
}

