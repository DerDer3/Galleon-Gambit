using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GameManagerStressTests
{
    GameManager gm;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        gm = new GameObject("GM").AddComponent<GameManager>();
        gm.mainGame = new GameObject("GameState").AddComponent<GameState>();
        gm.mainGame.mainDeck = new PlayerDeck();
        gm.cardPrefab = new GameObject("cardPrefab");
        gm.cardPrefab.AddComponent<CardObject>();
        yield return null;
    }

    [UnityTest]
    public IEnumerator Stress_DrawCards()
    {
        for (int i = 0; i < 500; i++)
        {
            gm.mainGame.mainDeck.AddCard(new ShipRepair(), "Repair");
            gm.SendMessage("DrawHandCard");
            gm.SendMessage("UpdateHandLayout");
        }
        Assert.Pass("No crash after 500 draws");
        yield return null;
    }
}

