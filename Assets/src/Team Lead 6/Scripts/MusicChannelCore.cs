using System.Collections;
using UnityEngine;

public class MusicChannelCore : SoundChannelCore
{
    //private const float FadeDuration = 1.5f; //Seconds
    public MusicChannelCore(AudioSource source) : base(source)
    {
        //Music specific setup
        source.loop = true;
        source.priority = 0; //Highest priority
    }

    //Dynamic: Fade in instead of instant play
    /*public override void Play(SoundCue cue, float effectiveVolume)
    {
        Debug.Log("MusicChannelCore override Play()");

        if (cue == null) return;

        AudioClip clip = cue.GetRandomClip();
        if (clip == null) return;

        source.loop = true;
        source.clip = clip;
        source.pitch = 0.5f; //Lower pitch
        source.volume = Mathf.Clamp01(effectiveVolume * 0.5f); //Half volume
        source.Play();
    }



    /*Scrapped Dynamic fade-in
    public override void Play(SoundCue cue, float effectiveVolume)
    {
        if (SoundManager.Instance == null)
        {
            base.Play(cue, effectiveVolume);
            return;
        }

        SoundManager.Instance.StartCoroutine(FadeInMusic());

        IEnumerator FadeInMusic()
        {
            if (cue == null) yield break;

            AudioClip clip = cue.GetRandomClip();
            if (clip == null) yield break;

            source.loop = true;
            source.clip = clip;
            source.volume = 0f;
            source.Play();

            float t = 0f;
            while (t < FadeDuration)
            {
                t += Time.deltaTime;
                float normalized = Mathf.Clamp01(t / FadeDuration);
                source.volume = Mathf.Lerp(0f, effectiveVolume, normalized);
                yield return null;
            }

            source.volume = effectiveVolume;
        }
    }*/
}
