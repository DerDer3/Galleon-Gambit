using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SoundChannel
{
    //ChannelType type;
    float volume;
    bool muted;

    private void SetVolume()
    {
        Console.WriteLine("SoundChannel: SetVolume()");
    }

    private void SetMuted()
    {
        Console.WriteLine("SoundChannel: SetMuted()");
    }
}
