using UnityEngine;

public class MusicChannelCore : SoundChannelCore
{
    private SoundCue currentCue;
    public MusicChannelCore(AudioSource source) : base(source)
    {
        //Music specific setup
        source.loop = true;
        source.priority = 0; //Highest priority
    }

    public override void Play(SoundCue cue, float effectiveVolume)
    {
        if (cue == null) return;

        /*if (currentCue == cue && source.isPlaying)
            return;

        currentCue = cue;*/

        AudioClip clip = cue.GetRandomClip();
        if (clip == null) return;

        //Crossfade logic here
        source.loop = true;
        source.clip = clip;
        source.volume = cue.Volume;
        source.Play();

        //Maybe add system logs
    }

    /*public override void SetVolume(float volume)
    {
        volumeMultiplier = Mathf.Clamp01(volume);

        if (currentCue != null && source.isPlaying)
        {
            source.volume = currentCue.Volume * volumeMultiplier;
        }
    }*/
}
