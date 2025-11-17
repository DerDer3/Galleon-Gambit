using UnityEngine;
using UnityEngine.InputSystem;

public class TestAudio : MonoBehaviour
{
    void Update()
    {
        if (SoundManager.Instance == null)
        {
            Debug.Log("SoundManager Instance not present!");
            return;
        }

        // Press space to test Island music
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("Testing: Playing music track");
            SoundManager.Instance.play(MusicTracks.Battle);
        }

        // Press left mouse to test a SFX
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("Testing: Playing SFX Button");
            SoundManager.Instance.play(SoundEffects.Sword);
        }
    }
}
