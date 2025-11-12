using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class MapTests : MonoBehaviour
{
    [Test]
    public void LevelNames()
    {
        Battle binfo = new();
        Assert.AreEqual(binfo.Name(), "Fight");
        LevelInfo info = binfo;
        Assert.AreEqual(info.Name(), "Battle");
    }

    [Test]
    public void LevelDescriptions()
    {
        LevelInfo info = new();
        Assert.AreEqual(info.Description(), "Land ho to rest ye pegs.");

        info = new Battle();
        Assert.AreEqual(info.Description(), "There be scallywags to plunder.");

        info = new Shop();
        Assert.AreEqual(info.Description(), "Bargain ye treasures.");
    }

    // [UnityTest]
    // public IEnumerator LevelsConnect()
    // {

    //     yield return null; // Wait for one frame

    // }
    
    public class LevelInfo
    {
        public int LevelId { get; private set; }

        public string Name() => GetType().Name;
        public virtual string Description() => "Land ho to rest ye pegs.";
    }

    public class Battle : LevelInfo
    {
        // Overshadowed method to show dynamic/static binding.
        public new string Name() => "Fight";
        public override string Description() => "There be scallywags to plunder.";
    }

    public class Shop : LevelInfo
    {
        public override string Description() => "Bargain ye treasures.";
    }

    public class Boss : LevelInfo
    {
        public override string Description() => "There be a captain!";
    }

    public class Treasure : LevelInfo
    {
        public override string Description() => "A bounteous booty.";
    }

    public class Unknown : LevelInfo
    {
        public override string Description() => "Uncharted land for lootin'?";
    }
}
