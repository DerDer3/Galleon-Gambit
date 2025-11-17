using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using TMPro;
using System.Reflection;
using UnityEngine.UI;

public class Memory_Test
{
    GameObject coreGO;
    TreasureMemoryCore core;

    // Creates a simple sprite in memory
    private Sprite MakeTestSprite(Color color)
    {
        Texture2D tex = new Texture2D(32, 32);
        for (int x = 0; x < 32; x++)
            for (int y = 0; y < 32; y++)
                tex.SetPixel(x, y, color);

        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, 32, 32), new Vector2(.5f, .5f));
    }

    // Creates a temporary CardMem prefab for tests
    private GameObject MakeTestCardPrefab()
    {
        GameObject go = new GameObject("TestCardPrefab");
        go.AddComponent<Button>();

        // Back Image
        Image back = go.AddComponent<Image>();

        // Face child
        GameObject faceObj = new GameObject("Face");
        faceObj.transform.SetParent(go.transform);
        Image face = faceObj.AddComponent<Image>();

        CardMem card = go.AddComponent<CardMem>();
        card.faceImage = face;
        card.backImage = back;

        return go;
    }

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Create controller object
        coreGO = new GameObject("MemoryGameCore_Test");
        core = coreGO.AddComponent<TreasureMemoryCore>();

        // Create prefab dynamically
        core.cardPrefab = MakeTestCardPrefab();

        // Create sprites dynamically
        core.cardBack = MakeTestSprite(Color.black);

        core.faces = new System.Collections.Generic.List<Sprite>()
        {
            MakeTestSprite(Color.red),
            MakeTestSprite(Color.blue)
        };

        // Board parent
        GameObject parent = new GameObject("BoardParent");
        core.boardParent = parent.transform;

        // Win text
        GameObject w = new GameObject("WinText");
        core.winText = w.AddComponent<TextMeshProUGUI>();

        // Call Start manually using reflection (private method)
        MethodInfo startMethod =
            typeof(TreasureMemoryCore).GetMethod("Start", BindingFlags.Instance | BindingFlags.NonPublic);

        startMethod.Invoke(core, null);

        yield return null;
    }

    // ---------------------------------------------------------------
    // 1. BOARD CREATION
    // ---------------------------------------------------------------
    [UnityTest]
    public IEnumerator Board_Creates_Correct_Number_Of_Cards()
    {
        yield return null;

        int expected = core.faces.Count * 2;
        Assert.AreEqual(expected, core.boardParent.childCount);
    }

    // ---------------------------------------------------------------
    // Helper: read private bool isMatched from CardMem
    // ---------------------------------------------------------------
    private bool GetIsMatched(CardMem c)
    {
        return (bool) typeof(CardMem)
            .GetField("isMatched", BindingFlags.NonPublic | BindingFlags.Instance)
            .GetValue(c);
    }

    // ---------------------------------------------------------------
    // 2. MATCH TEST
    // ---------------------------------------------------------------
    [UnityTest]
    public IEnumerator Matching_Two_Cards_Sets_isMatched_True()
    {
        CardMem first = null;
        CardMem second = null;

        foreach (Transform t in core.boardParent)
        {
            CardMem c = t.GetComponent<CardMem>();

            if (first == null)
                first = c;
            else if (c.MemCardId == first.MemCardId)
            {
                second = c;
                break;
            }
        }

        core.CardFlipped(first);
        core.CardFlipped(second);

        yield return new WaitForSeconds(1f);

        Assert.IsTrue(GetIsMatched(first));
        Assert.IsTrue(GetIsMatched(second));
    }

    // ---------------------------------------------------------------
    // 3. WIN MESSAGE TEST
    // ---------------------------------------------------------------
    [UnityTest]
    public IEnumerator Win_Message_Shows_When_All_Pairs_Found()
    {
        typeof(TreasureMemoryCore)
            .GetField("pairsFound", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(core, core.faces.Count);

        typeof(TreasureMemoryCore)
            .GetMethod("ShowWinMessage", BindingFlags.NonPublic | BindingFlags.Instance)
            .Invoke(core, null);

        yield return null;

        Assert.IsTrue(core.winText.gameObject.activeSelf);
        Assert.AreEqual("You won 5 gold coins!", core.winText.text);
    }

    // ---------------------------------------------------------------
    // 4. RESTART TEST
    // ---------------------------------------------------------------
    [UnityTest]
    public IEnumerator Restart_Grant_Rebuilds_The_Board()
    {
        int before = core.boardParent.childCount;

        core.Restart_Grant();
        yield return null;

        int after = core.boardParent.childCount;

        Assert.AreEqual(before, after);
        Assert.IsFalse(core.winText.gameObject.activeSelf);
    }
}



