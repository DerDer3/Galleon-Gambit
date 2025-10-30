using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class MapTests : MonoBehaviour
{
    [Test]
    public void AddTwoNumbers()
    {
        int a = 5;
        int b = 3;

        Assert.AreEqual(9, a + b);
    }

    [UnityTest]
    public IEnumerator LevelsConnect()
    {
        
        yield return null; // Wait for one frame

    }
}
