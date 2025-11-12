using UnityEngine;

/// <summary>Visual indicator for a level connecting to another level.</summary>
[RequireComponent(typeof(LineRenderer))]
public class LevelConnection : MonoBehaviour
{
    /// <summary>The level where this connection starts.</summary>
    public GameObject FromLevel { set; private get; }
    /// <summary>The level where this connection ends.</summary>
    public GameObject ToLevel { set; private get; }

    private LineRenderer line;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
    }

    void Update()
    {
        line.SetPosition(0, FromLevel.transform.position);
        line.SetPosition(1, ToLevel.transform.position);
    }
}
