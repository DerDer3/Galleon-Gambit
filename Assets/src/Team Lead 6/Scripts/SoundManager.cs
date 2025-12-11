using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic; //Required for List<>

[RequireComponent(typeof(AudioSource))]

/*
 * CDA Game
 * QR For desktop/ mobile
 * Teammates know attendance expectations
*/ 
public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance; //{ get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;


    [Header("Audio Library Mapping")]
    public List<MusicTrackData> musicLibrary = new List<MusicTrackData>();


    [Header("SFX Library Mapping")]
    public List<SFXTrackData> sfxLibrary = new List<SFXTrackData>();


    [Header("Volume Controls")]
    [SerializeField, Range(0f, 1f)] private float masterVolume = 1f;

    [SerializeField, Range(0f, 1f)] private float musicVolume = 1f;

    [SerializeField, Range(0f, 1f)] private float sfxVolume = 1f;

    public float MasterVolume => masterVolume;
    public float MusicVolume  => musicVolume;
    public float SFXVolume    => sfxVolume;

    //Dynamic binding occuring
    private SoundChannelCore Music;
    private SoundChannelCore SFX;
    private SoundCue currentCue;

    private void Awake()
    {
        //Singleton Pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (musicSource == null)
            musicSource = gameObject.AddComponent<AudioSource>();
        if (sfxSource == null)
            sfxSource = gameObject.AddComponent<AudioSource>();

        musicSource.volume = 1f;
        sfxSource.volume = 1f;

        Music = new MusicChannelCore(musicSource);
        SFX = new SFXChannelCore(sfxSource);
    }

    private void Update()
    {
        if(musicSource != null)
        {
            float cueVol = currentCue != null ? currentCue.Volume : 1f;
            float effectiveVolume = Mathf.Clamp01(cueVol * masterVolume * musicVolume);
            musicSource.volume = effectiveVolume;
        }
    }

    public void play(MusicTracks title)
    {
        SoundCue cue = GetMusic(title);
        if (cue == null)
        {
            Debug.LogWarning($"Music track '{title}' not found in the library");
            return;
        }

        if(Music == null)
        {
            Debug.LogError("Music channel doesn't exist");
        }

        currentCue = cue;
        float effectiveVolume = Mathf.Clamp01(cue.Volume * masterVolume * musicVolume);
        Music.Play(cue, effectiveVolume);

        if (musicSource != null)
        {
            musicSource.volume = effectiveVolume;
        }
    }

     
    public void play(SoundEffects title)
    {
        SoundCue cue = GetSFX(title);
        if (cue == null)
        {
            Debug.LogWarning($"Sound Effect '{title}' not found in library");
            return;
        }

        AudioClip clip = cue.GetRandomClip();
        if(clip = null)
        {
            Debug.LogWarning($"SFX '{title}' has no clips assigned.");
            return;
        }

        float effectiveVolume = Mathf.Clamp01(cue.Volume * masterVolume * sfxVolume);
        
        if(sfxSource == null)
        {
            Debug.LogError("SFX Source is not assigned.");
            return;
        }
        SFX.Play(cue, effectiveVolume);

        Debug.Log($"Playing SFX: {title} at volume {effectiveVolume}");
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

    public void SetMasterVolume(float value)
    {
        masterVolume = Mathf.Clamp01(value);
        Debug.Log($"[SoundManager] MasterVolume set to {masterVolume}");

        if(musicSource != null)
        {
            float cueVol = currentCue != null ? currentCue.Volume : 1f;
            float effectiveVolume = Mathf.Clamp01(cueVol * masterVolume * musicVolume);
            musicSource.volume = effectiveVolume;
        }                
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = Mathf.Clamp01(value);
        Debug.Log($"[SoundManager] MusicVolume set to {musicVolume}");
        
        if(musicSource != null)
        {
            float cueVol = currentCue != null ? currentCue.Volume : 1f;
            float effectiveVolume = Mathf.Clamp01(cueVol * masterVolume * musicVolume);
            musicSource.volume = effectiveVolume;
        }
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = Mathf.Clamp01(value);
        Debug.Log($"[SoundManager] SFXVolume set to {sfxVolume}");
        //UpdateChannelVolumes();
    }

    private void ApplyMusicVolume()
    {
        if (musicSource == null) return;

        if (currentCue == null)
        {
            musicSource.volume = Mathf.Clamp01(masterVolume * musicVolume);
            return;
        }
        /*if (currentCue == null || musicSource == null) return;
        if (!musicSource.isPlaying) return;*/

        float effectiveVolume = Mathf.Clamp01(currentCue.Volume * masterVolume * musicVolume);
        musicSource.volume = effectiveVolume;

        Debug.Log($"[SoundManager] Applied music volume = {effectiveVolume}");
    }
}