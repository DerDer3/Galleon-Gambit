using UnityEngine;
using System;
using System.Collections.Generic;

public class CardCreator
{
    // ---------------- Singleton Pattern ----------------
    private static CardCreator _instance;
    public static CardCreator Instance
    {
        get
        {
            if (_instance == null)
                _instance = new CardCreator();
            return _instance;
        }
    }

    private CardCreator()
    {
        rng = new System.Random();
        cardConstructors = new List<Func<Card>>
        {
            () => new Slash(),
            () => new ShipRepair(),
            () => new PistolShot(),
            () => new TwinBlades(),
            () => new BoardingCharge(),
            () => new BackStab(),
            () => new WhispersBelow()
        };
    }

    private List<Func<Card>> cardConstructors;
    private System.Random rng;

    public Card CreateRandomCard()
    {
        int index = rng.Next(cardConstructors.Count);
        return cardConstructors[index]();
    }
}
