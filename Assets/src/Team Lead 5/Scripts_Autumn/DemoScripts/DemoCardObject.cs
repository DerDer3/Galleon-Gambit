using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DemoCardObject : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private DemoCard linkedCard; // the card this object represents
    private DemoGameState linkedState;
    private SpriteRenderer spriteRenderer;
    public TextMeshProUGUI cardName;
    public TextMeshProUGUI manaAmount;
    Vector3 currentPos;

    //public GameObject pauseObject;

    public void SetCard(DemoCard card, DemoGameState state)
    {
        linkedCard = card;
        linkedState = state;
        cardName.text = linkedCard.cardName;
        manaAmount.text = "" + linkedCard.cardCost;

        float randomGrayValue = Random.Range(0.2f, 0.8f);
        this.ChangeColor(randomGrayValue, randomGrayValue, randomGrayValue);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (linkedState.turn == false)
        {
            linkedCard.Play(linkedState);
            linkedState.mainDeck.DiscardCard(linkedCard);
            SetCard(linkedState.mainDeck.DrawCardWithReshuffle(), linkedState);
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (!checkPause())
        {
            transform.position += Vector3.up;
        }
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

    public bool checkPause()
    {  /*
        if(pauseObject != null)
        {
            if (pauseObject.activeSelf)
            {
                return true;
            }
            else if (pauseObject.activeInHierarchy)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }*/
        return false;
        //Eventually will update to work correctly again.
    }
}
