using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic; //Required for List<>

[RequireComponent(typeof(AudioSource))]
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
    [SerializeField, Range(0f, 1f)]
    private float masterVolume = 1f;

    [SerializeField, Range(0f, 1f)]
    private float musicVolume = 1f;

    [SerializeField, Range(0f, 1f)]
    private float sfxVolume = 1f;

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



        Music = new MusicChannelCore(musicSource);
        SFX = new SFXChannelCore(sfxSource);

        //UpdateChannelVolumes();
    }

    public void play(MusicTracks title)
    {
        SoundCue cue = GetMusic(title);
        if (cue = null)
        {
            Debug.LogWarning($"Music track '{title}' not found in the library");
            return;
        }

        currentCue = cue;
        float effectiveVolume = Mathf.Clamp01(cue.Volume * masterVolume * musicVolume);
        Music.Play(cue, effectiveVolume);
    }

     
    public void play(SoundEffects title)
    {
        SoundCue cue = GetSFX(title);
        if (cue == null)
        {
            Debug.LogWarning($"Sound Effect '{title}' not found in library");
            return;
        }

        float effectiveVolume = Mathf.Clamp01(cue.Volume * masterVolume * sfxVolume);
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
        //UpdateChannelVolumes();
        ApplyMusicVolume();
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = Mathf.Clamp01(value);
        Debug.Log($"[SoundManager] MusicVolume set to {musicVolume}");
        //UpdateChannelVolumes();
        ApplyMusicVolume();
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = Mathf.Clamp01(value);
        Debug.Log($"[SoundManager] SFXVolume set to {sfxVolume}");
        //UpdateChannelVolumes();
    }
    
    /*public void UpdateChannelVolumes()
    {
        float musicMuliplier = masterVolume * musicVolume;
        float sfxMultiplier = masterVolume * sfxVolume;

        Debug.Log($"[SoundManager] musicMult={musicMuliplier}, sfxMult={sfxMultiplier}");

        Music.SetVolume(musicMuliplier);
        SFX.SetVolume(sfxMultiplier);
    }*/

    private void ApplyMusicVolume()
    {
        if (currentCue == null || musicSource == null) return;
        if (!musicSource.isPlaying) return;

        float effectiveVolume = Mathf.Clamp01(currentCue.Volume * masterVolume * musicVolume);
        musicSource.volume = effectiveVolume;

        Debug.Log($"[SoundManager] Applied music volume = {effectiveVolume}");
    }
}