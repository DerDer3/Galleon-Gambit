using UnityEngine;

//Default strategy for playing soundCues
public abstract class SoundChannelCore
{
    protected readonly AudioSource source;
    //protected float volumeMultiplier = 1f; //Set by soundManager
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
        source.volume = Mathf.Clamp01(effectiveVolume);//cue.Volume * volumeMultiplier;
        source.Play();
    }

    /*public virtual void SetVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        volumeMultiplier = volume;

        //Only adjust volume if music is playing
        if (source.isPlaying)
        {
            source.volume = volumeMultiplier;
        }
    }*/
}
