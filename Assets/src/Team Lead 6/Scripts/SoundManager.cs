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

    [Header("Audio Library Mapping")]
    public List<MusicTrackData> musicLibrary = new List<MusicTrackData>();

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
            if(musicSource == null)
            {
                Debug.LogError("FATAL: Music Source not assigned in the Inspector!");
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
        switch (title)
        {
            case SoundEffects.test:
                Console.WriteLine("Play SFX");
                break;
        }
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

    /*public void SetChannelVolume()
    {
        Console.WriteLine("Setting Channel Volume");
    }

    public void SetChannelMute()
    {
        Console.WriteLine("Muting Channel");
    }*/
}
