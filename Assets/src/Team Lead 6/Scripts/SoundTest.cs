using UnityEngine;
using UnityEngine.InputSystem;

public class test : MonoBehaviour
{

    void Update()
    {
        if (SoundManager.Instance != null)
        {
            if (Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame)
            {
                SoundManager.Instance.PlayOneShotSound();
            }
        }
    }
    /*private Mouse mouse;

    void OnEnable()
    {
        mouse = Mouse.current;
        if (mouse != null)
        {
            mouse.rightButton.wasPressedThisFrame.performed += OnRightClick;
        }
    }
    void OnDisable()
    {
        if (mouse != null)
        {
            mouse.rightButton.wasPressedThisFrame.performed -= OnRightClick;
        }
    }
    private void OnRightClick(InputAction.CallbackContext context)
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayOneShotSound();
        }
    }*/
}
