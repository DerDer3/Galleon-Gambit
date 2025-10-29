using UnityEngine;
using System;

[Serializable]
public class MusicTrackData
{
    //Value from MusicTracks enum
    public MusicTracks track;

    //Specific musicd file to play
    public AudioClip clip;
}
