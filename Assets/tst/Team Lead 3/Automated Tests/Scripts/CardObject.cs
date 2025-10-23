using UnityEngine;
using UnityEngine.EventSystems;
//using TMPro;

public class CardObject : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Card linkedCard; // the card this object represents
    private GameState linkedState;
    private SpriteRenderer spriteRenderer;
    //public TextMeshProUGUI cardName;
   // public TextMeshProUGUI manaAmount;
    Vector3 currentPos;

    public void SetCard(Card card, GameState state)
    {
        linkedCard = card;
        linkedState = state;
        //cardName.text = linkedCard.cardName;
        //manaAmount.text = "" + linkedCard.cardCost;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
      if(linkedState.turn == false)
      {
        linkedCard.Play(linkedState);
      }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
      transform.position += Vector3.up;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
      transform.position = currentPos;
    }

    public void ChangeColor(float r, float g, float b)
    {
      spriteRenderer = GetComponent<SpriteRenderer>();
      spriteRenderer.color = new Color(r, g, b, 1); 
    }

    void Start()
    {
      currentPos = transform.position;
    }

    void Update()
    {
    }
}
