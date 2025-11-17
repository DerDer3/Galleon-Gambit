using UnityEngine;
using System;

[Serializable]
public class SFXTrackData
{
    //Value from SoundEffects enum
    public SoundEffects effect;

    //Specific SFX cue to play
    public SoundCue cue;
}
