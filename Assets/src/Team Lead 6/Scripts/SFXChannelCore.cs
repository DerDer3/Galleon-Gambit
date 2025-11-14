using UnityEngine;

public class SFXChannelCore : SoundChannelCore
{
    public SFXChannelCore(AudioSource source) : base(source)
    {
        // SFX-specific setup if needed
        source.loop = false;
    }

    public override void Play(SoundCue cue)
    {
        if (cue == null) return;

        AudioClip clip = cue.GetRandomClip();
        if (clip == null) return;

        // Fire-and-forget style
        source.PlayOneShot(clip, cue.Volume);
    }
}
