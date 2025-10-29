using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.InputSystem;

public class test : MonoBehaviour
{

    void Update()
    {
        if (SoundManager.Instance != null)
        {
            //MusicTracks.[] determines what background music plays
            //Try "Main" or "battle"
            SoundManager.Instance.play(MusicTracks.Battle);
        }

        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            // 3. Play the sound effect using your mapped enum
            SoundManager.Instance.play(SoundEffects.sword);
            
            // Optional: Change/start music here if needed
            // SoundManager.Instance.play(MusicTracks.Battle);
        }

    }
    
}
