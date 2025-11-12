using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>Animates screen transitions between scenes.</summary>
[RequireComponent(typeof(Image))]
public class ScreenTransition : MonoBehaviour
{
    public UnityEvent TransitionComplete;
    private Image blackScreen;
    private State state = State.Transparent;
    private float alpha;

    private void Awake()
    {
        blackScreen = GetComponent<Image>();
        UpdateAlpha();
    }

    private void Update()
    {
        switch (state)
        {
            case State.FadingToBlack:
                alpha = Mathf.Min(alpha + Time.deltaTime, 1f);
                UpdateAlpha();
                if (alpha == 1f)
                {
                    state = State.FadingFromBlack;
                    TransitionComplete.Invoke();
                }
                break;

            case State.FadingFromBlack:
                alpha = Mathf.Max(alpha - Time.deltaTime, 0f);
                UpdateAlpha();
                if (alpha == 0f)
                {
                    state = State.Transparent;
                }
                break;

            case State.Transparent:
                // Do nothing
                break;
        }
    }

    public void ShowTransition()
    {
        state = State.FadingToBlack;
    }

    private void UpdateAlpha()
    {
        var c = blackScreen.color;
        blackScreen.color = new Color(c.r, c.g, c.b, alpha);

        // Blocks user from pressing things while transition is happening
        blackScreen.raycastTarget = alpha != 0f;
    }

    private enum State
    {
        FadingToBlack,
        FadingFromBlack,
        Transparent,
    }
}
