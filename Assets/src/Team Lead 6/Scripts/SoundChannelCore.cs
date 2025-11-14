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
    public virtual void Play(SoundCue cue)
    {
        if (cue == null) return;

        AudioClip clip = cue.GetRandomClip();
        if (clip == null) return;

        source.loop = cue.Loop;
        source.clip = clip;
        source.volume = cue.Volume;
        source.Play();
    }

    public virtual void SetVolume(float volume)
    {
        source.volume = Mathf.Clamp01(volume);
    }
}
