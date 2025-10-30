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
            //Try "Main", "Battle", "Island", or "Boss"
            SoundManager.Instance.play(MusicTracks.Boss);
        }

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            // 3. Play the sound effect using your mapped enum
            SoundManager.Instance.play(SoundEffects.sword);
            
            // Optional: Change/start music here if needed
            // SoundManager.Instance.play(MusicTracks.Battle);
        }

    }
    
}
