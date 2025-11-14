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

    //Dynamic binding occuring
    private SoundChannelCore Music;
    private SoundChannelCore SFX;

    private void Awake()
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
            if (sfxSource == null)
            {
                Debug.LogError("FATAL: SFX Source not assigned in the Inspector!");
            }
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        Music = new MusicChannelCore(musicSource);
        SFX = new SFXChannelCore(sfxSource);

    }

    public void play(MusicTracks title)
    {
        SoundCue cue = GetMusic(title);
        if (cue = null)
        {
            Debug.LogWarning($"Music track '{title}' not found in the library");
            return;
        }
        Music.Play(cue);        
    }

     
    public void play(SoundEffects title)
    {
        SoundCue cue = GetSFX(title);
        if (cue == null)
        {
            Debug.LogWarning($"Sound Effect '{title}' not found in library");
            return;
        }
        SFX.Play(cue);
    }

    private SoundCue GetMusic(MusicTracks title)
    {
        foreach (MusicTrackData data in musicLibrary)
        {
        if (data.track == title)
            {
                return data.cue;
            }
        }
        return null;
    }

    private SoundCue GetSFX(SoundEffects title)
    {
        foreach (SFXTrackData data in sfxLibrary)
        {
            if (data.effect == title)
            {
                return data.cue;
            }
        }
       return null;
    }
}

