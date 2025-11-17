using UnityEngine;

public class SFXChannelCore : SoundChannelCore
{
    public SFXChannelCore(AudioSource source) : base(source)
    {
        // SFX-specific setup if needed
        source.loop = false;
    }

    public override void Play(SoundCue cue, float effectiveVolume)
    {
        if (cue == null) return;

        AudioClip clip = cue.GetRandomClip();
        if (clip == null) return;

        // Fire-and-forget style
        source.PlayOneShot(clip, Mathf.Clamp01(effectiveVolume));//cue.Volume * volumeMultiplier);
    }

    /*public override void SetVolume(float volume)
    {
        volumeMultiplier = Mathf.Clamp01(volume);
    }*/
}
