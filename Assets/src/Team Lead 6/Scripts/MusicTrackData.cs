using UnityEngine;
using System;

[Serializable]
public class MusicTrackData
{
    //Value from MusicTracks enum
    public MusicTracks track;

    //Specific music cue to play
    public SoundCue cue;
}
