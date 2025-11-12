using UnityEngine;
using System;
using System.Collections.Generic;

public static class CardCreator
{
    private static List<Func<Card>> cardConstructors = new()
    {
        () => new Slash(),
        () => new ShipRepair(),
        () => new PistolShot(),
        () => new TwinBlades(),
        () => new BoardingCharge(),
        () => new BackStab(),
        () => new WhispersBelow()
    };

    private static System.Random rng = new System.Random();

    public static Card CreateRandomCard()
    {
        int index = rng.Next(cardConstructors.Count);
        return cardConstructors[index]();
    }
}
