using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class FadeOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    CanvasGroup cg;

    void Awake()
    {
        cg = gameObject.GetComponent<CanvasGroup>();
        if (cg == null) cg = gameObject.AddComponent<CanvasGroup>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(FadeTo(0.5f, 0.15f));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(FadeTo(1f, 0.15f));
    }

    public Image buttonImage;
public Text buttonText;

IEnumerator FadeTo(float targetAlpha, float duration)
{
    float startImage = buttonImage.color.a;
    float startText = buttonText.color.a;
    float time = 0f;

    while (time < duration)
    {
        time += Time.deltaTime;
        Color imgColor = buttonImage.color;
        imgColor.a = Mathf.Lerp(startImage, targetAlpha, time / duration);
        buttonImage.color = imgColor;

        Color txtColor = buttonText.color;
        txtColor.a = Mathf.Lerp(startText, targetAlpha, time / duration);
        buttonText.color = txtColor;

        yield return null;
    }
}
}
