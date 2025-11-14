using UnityEngine;

public class MusicChannelCore : SoundChannelCore
{
    public MusicChannelCore(AudioSource source) : base(source)
    {
        //Music specific setup
        source.loop = true;
        source.priority = 0; //Highest priority
    }

    public override void Play(SoundCue cue)
    {
        if (cue == null) return;

        AudioClip clip = cue.GetRandomClip();
        if (clip == null) return;

        //Crossfade logic here
        source.loop = true;
        source.clip = clip;
        source.volume = cue.Volume;
        source.Play();

        //Maybe add system logs
    }
}
