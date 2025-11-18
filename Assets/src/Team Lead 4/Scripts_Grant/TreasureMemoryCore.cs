using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class TreasureMemoryCore : MonoBehaviour
{
    [Header("References (assign in inspector)")]
    public GameObject cardPrefab;
    public Sprite cardBack;
    public List<Sprite> faces;        // one sprite per unique face (pairs = faces.Count)
    public Transform boardParent;     // BoardPanel transform with GridLayoutGroup
    public TextMeshProUGUI winText;   // UI text to show win message

    [Header("Gameplay")]
    public float mismatchDelay = 0.8f;

    public UnityEvent<int> GameWon;

    CardMem firstCard = null;
    CardMem secondCard = null;
    int pairsFound = 0;
    public bool IsBusy { get; private set; } = false;

    void Start()
    {
        if (winText) winText.gameObject.SetActive(false);
        SetupBoard();
    }

    public void SetupBoard()
    {
        // Clear parent
        foreach (Transform child in boardParent) Destroy(child.gameObject);

        pairsFound = 0;
        firstCard = null;
        secondCard = null;
        IsBusy = false;

        // Build id list (two of each face id)
        List<int> ids = new List<int>();
        for (int i = 0; i < faces.Count; i++)
        {
            ids.Add(i);
            ids.Add(i);
        }

        // Shuffle ids (Fisher-Yates)
        for (int i = ids.Count - 1; i > 0; i--)
        {
            int r = Random.Range(0, i + 1);
            int tmp = ids[i];
            ids[i] = ids[r];
            ids[r] = tmp;
        }

        // Instantiate cards
        for (int i = 0; i < ids.Count; i++)
        {
            GameObject go = Instantiate(cardPrefab, boardParent);
            CardMem cardT = go.GetComponent<CardMem>();
            int id = ids[i];
            cardT.Init(id, faces[id], cardBack, this);
        }
    }

    // Called by Card when clicked
    public void CardFlipped(CardMem cardT)
    {
        if (IsBusy) return;

        if (firstCard == null)
        {
            firstCard = cardT;
            return;
        }

        if (secondCard == null && cardT != firstCard)
        {
            secondCard = cardT;
            StartCoroutine(CheckMatch());
        }
    }

    IEnumerator CheckMatch()
    {
        IsBusy = true;

        // small pause so player sees second card
        yield return new WaitForSeconds(0.25f);

        if (firstCard.MemCardId == secondCard.MemCardId)
        {
            // Match
            firstCard.MarkMatched_Grant();
            secondCard.MarkMatched_Grant();
            pairsFound++;
        }
        else
        {
            // Not match — wait, then flip back
            yield return new WaitForSeconds(mismatchDelay);
            firstCard.ShowBack();
            secondCard.ShowBack();
        }

        firstCard = null;
        secondCard = null;
        IsBusy = false;

        // GAME COMPLETE — transition back to map
        if (pairsFound >= faces.Count)
        {
            ShowWinMessage();

            yield return new WaitForSeconds(3f); // let's the win message stay for 3 seconds

            GameWon.Invoke(5);  // Invoke the event, passing the reward (optional, but consistent)
        }
    }

    void ShowWinMessage()
    {
        if (winText)
        {
            winText.text = "You won 5 gold coins!";
            winText.gameObject.SetActive(true);
        }
    }

    // Optional: call this to restart game
    public void Restart_Grant()
    {
        if (winText) winText.gameObject.SetActive(false);
        SetupBoard();
    }
}

