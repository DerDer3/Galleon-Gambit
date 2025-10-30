using UnityEngine;
using TMPro;

public class HandStressTest : MonoBehaviour
{
    public GameManager gameManager;
    public int drawLimit = 10000;  // change to however many you want to stress test
    public bool runOnStart = true;
    public TextMeshProUGUI testText;

    void Start()
    {
        if (runOnStart)
            StartCoroutine(RunTest());
    }

    private System.Collections.IEnumerator RunTest()
    {
        Debug.Log("Starting hand stress test...");

        for (int i = 0; i < drawLimit; i++)
        {
            gameManager.mainGame.mainDeck.AddCard(new Slash(), "Slash");
            gameManager.SendMessage("DrawHandCard");
            gameManager.SendMessage("UpdateHandLayout");

            if (i % 100 == 0)
                Debug.Log($"Drawn {i} cards so far...");

            testText.text = "" + i;

            // Yield every frame to keep Unity responsive
            yield return null;
        }

        Debug.Log($"Finished drawing {drawLimit} cards!");
    }
}


