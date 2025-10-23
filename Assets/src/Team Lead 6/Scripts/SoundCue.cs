using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SoundCue : MonoBehaviour
{
    AudioClip clip;
    float volume;
    float pitch;
    bool loop;
    //ChannelType channel;

    public void Configure()
    {
        Console.WriteLine("Configure");
    }
}
