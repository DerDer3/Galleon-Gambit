using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class PlayerDeckStressTests : MonoBehaviour
{
    private PlayerDeck playerDeck;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        SceneManager.LoadScene("SampleScene");
        playerDeck = new PlayerDeck();
        yield return null;
    }

    // PLAYMODE STRESS TEST
    [UnityTest]
    public IEnumerator StressTest_AddAndDrawManyCards()
    {
        int largeCount = 0; 
        while(true)
        {
            largeCount++;
            playerDeck.AddCard(new Slash(), "Attack");
            playerDeck.DrawCard();

            Debug.Log($"Added and drew {largeCount} cards.");

            yield return null;
        }

    }
}
