using UnityEngine;

/// <summary>Visual indicator for a level connecting to another level.</summary>
[RequireComponent(typeof(LineRenderer))]
public class LevelConnection : MonoBehaviour
{
    public GameObject FromLevel;
    public GameObject ToLevel;

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
