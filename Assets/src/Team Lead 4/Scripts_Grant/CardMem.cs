using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CardMem : MonoBehaviour
{
    [HideInInspector] public int MemCardId;
    public Image faceImage;   // assign in prefab: child "Face" Image
    public Image backImage;   // assign in prefab: root Image

    TreasureMemoryCore controller;
    bool isFlipped = false;
    bool isMatched = false;
    bool isAnimating = false;

    [Header("Flip Settings")]
    public float flipDuration = 0.3f;

    // Called by GameController when creating the card
    public void Init(int id, Sprite face, Sprite back, TreasureMemoryCore ctrl)
    {
        MemCardId = id;
        faceImage.sprite = face;
        backImage.sprite = back;
        controller = ctrl;
        ShowBackInstant();
    }

    // Called by Button OnClick in the prefab
    public void OnClick()
    {
        if (isMatched || controller == null || controller.IsBusy || isFlipped || isAnimating)
            return;

        StartCoroutine(FlipToFace());
    }

    IEnumerator FlipToFace()
    {
        isAnimating = true;

        // rotate halfway to hide back
        yield return StartCoroutine(RotateY(0f, 90f, flipDuration / 2f));

        // swap images at midpoint
        faceImage.enabled = true;
        backImage.enabled = false;

        // finish rotation to show front
        yield return StartCoroutine(RotateY(90f, 0f, flipDuration / 2f));

        isFlipped = true;
        isAnimating = false;

        controller.CardFlipped(this);
    }

    public IEnumerator FlipToBack()
    {
        isAnimating = true;

        yield return StartCoroutine(RotateY(0f, 90f, flipDuration / 2f));

        faceImage.enabled = false;
        backImage.enabled = true;

        yield return StartCoroutine(RotateY(90f, 0f, flipDuration / 2f));

        isFlipped = false;
        isAnimating = false;
    }

    IEnumerator RotateY(float startAngle, float endAngle, float duration)
    {
        float time = 0f;
        Quaternion startRot = Quaternion.Euler(0f, startAngle, 0f);
        Quaternion endRot = Quaternion.Euler(0f, endAngle, 0f);

        while (time < duration)
        {
            time += Time.deltaTime;
            transform.localRotation = Quaternion.Slerp(startRot, endRot, time / duration);
            yield return null;
        }

        transform.localRotation = endRot;
    }

    public void ShowFace()
    {
        StopAllCoroutines();
        transform.localRotation = Quaternion.identity;
        faceImage.enabled = true;
        backImage.enabled = false;
        isFlipped = true;
    }

    public void ShowBack()
    {
        StopAllCoroutines();
        transform.localRotation = Quaternion.identity;
        faceImage.enabled = false;
        backImage.enabled = true;
        isFlipped = false;
    }

    void ShowBackInstant() => ShowBack();

    public void MarkMatched()
    {
        isMatched = true;
        var btn = GetComponent<Button>();
        if (btn) btn.interactable = false;
    }
}

