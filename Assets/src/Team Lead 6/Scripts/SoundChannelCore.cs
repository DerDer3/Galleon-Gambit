using UnityEngine;

//Default strategy for playing soundCues
public abstract class SoundChannelCore
{
    protected readonly AudioSource source;
    protected SoundChannelCore(AudioSource source)
    {
        this.source = source;
    }

    //dynamic binding method
    public virtual void Play(SoundCue cue, float effectiveVolume)
    {
        if (cue == null) return;

        AudioClip clip = cue.GetRandomClip();
        if (clip == null) return;

        source.loop = cue.Loop;
        source.clip = clip;
        source.pitch = 1f; //normal pitch
        source.volume = Mathf.Clamp01(effectiveVolume);//cue.Volume * volumeMultiplier;
        source.Play();
    }

    public void SetVolume(float value)
    {
        source.volume = Mathf.Clamp01(value);
    }
}
