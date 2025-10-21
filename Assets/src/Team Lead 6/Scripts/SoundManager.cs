using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum MusicTracks
    {
        test
    }

    public enum SoundEffects
    {
        test
    }

    public void playMusic(MusicTracks title)
    {
        switch (title)
        {
            case MusicTracks.test:
                Console.WriteLine("Play Music");
                break;
        }
        
    }

    public void playSFX(SoundEffects title)
    {
        switch (title)
        {
            case SoundEffects.test:
                Console.WriteLine("Play SFX");
                break;
        }
    }
}
