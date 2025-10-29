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
    }
    
}
