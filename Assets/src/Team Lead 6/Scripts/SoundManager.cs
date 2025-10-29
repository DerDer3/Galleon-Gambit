using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic; //Required for List<>

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance; //{ get; private set; }
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Audio Library Mapping")]
    public List<MusicTrackData> musicLibrary = new List<MusicTrackData>();

    [Header("SFX Library Mapping")]
    public List<SFXTrackData> sfxLibrary = new List<SFXTrackData>();

    //private SoundChannel Music;
    //private SoundChannel SFX;

    void Awake()
    {
        //Singleton Pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (musicSource == null)
            {
                musicSource = GetComponent<AudioSource>();
            }
            if (musicSource == null)
            {
                Debug.LogError("FATAL: Music Source not assigned in the Inspector!");
            }
            if (sfxSource == null)
            {
                sfxSource = GetComponent<AudioSource>();
            }
            if(sfxSource == null)
            {
                Debug.LogError("FATAL: SFX Source not assigned in the Inspector!");
            }
            //play(MusicTracks.Main);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
       
    }
    


    
    public void play(MusicTracks title)
    {
        if (musicSource == null) return;

        AudioClip newClip = GetMusic(title);

        if (newClip == null)
        {
            Debug.LogWarning($"Music Track '{title}' not found in the library. Cannot play.");
            return;
        }

        //Optimization
        if (musicSource.clip == newClip && musicSource.isPlaying)
        {
            return;
        }

        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }

        musicSource.clip = newClip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void play(SoundEffects title)
    {
        if (sfxSource == null)
        {
            Debug.LogError("FATAL: SFX Source is not assigned");
            return;
        }
        AudioClip sfxClip = GetSFX(title);

        if(sfxClip == null)
        {
            Debug.LogWarning($"Sound Effect '{title}' not found in the library. Cannot play.");
            return;
        }

        sfxSource.PlayOneShot(sfxClip);

        Debug.Log($"Playing SFX: {title}");
    }

    private AudioClip GetMusic(MusicTracks title)
    {
        foreach (MusicTrackData data in musicLibrary)
        {
            if (data.track == title)
            {
                return data.clip;
            }
        }
        return null;
    }

    private AudioClip GetSFX(SoundEffects title)
    {
        foreach (SFXTrackData data in sfxLibrary)
        {
            if (data.effect == title)
            {
                return data.clip;
            }
        }
        return null;
    }

    /*public void SetChannelVolume()
    {
        Console.WriteLine("Setting Channel Volume");
    }

    public void SetChannelMute()
    {
        Console.WriteLine("Muting Channel");
    }*/
}
