using UnityEngine;
using UnityEngine.EventSystems;

public class CardObject : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Card linkedCard; // the card this object represents
    private SpriteRenderer spriteRenderer;

    public void SetCard(Card card)
    {
        linkedCard = card;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
      linkedCard.Play();
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
      transform.position += Vector3.up;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
      transform.position += Vector3.down;
    }

    public void ChangeColor(float r, float g, float b)
    {
      spriteRenderer = GetComponent<SpriteRenderer>();
      spriteRenderer.color = new Color(r, g, b, 1); 
    }
}
