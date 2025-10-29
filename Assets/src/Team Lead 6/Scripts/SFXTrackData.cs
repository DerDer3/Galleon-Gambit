using UnityEngine;
using System;

[Serializable]
public class SFXTrackData
{
    //Value from MusicTracks enum
    public SoundEffects effect;

    //Specific musicd file to play
    public AudioClip clip;
}
