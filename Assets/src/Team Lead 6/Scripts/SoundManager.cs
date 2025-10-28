using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    
    public static SoundManager Instance;
    public AudioClip clip;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            PlayBackgroundMusic();
        }
        else
        {
            Destroy(gameObject);
        }
        //Singleton Pattern
        /*if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }*/


        /*if (audioSource == null)
        {
            Debug.LogError("SoundManager requires an AudioSource component");
        }*/
    }

    private void PlayBackgroundMusic()
    {
        if (audioSource != null && clip != null && !audioSource.isPlaying)
        {
            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Background music is missing or already playing");
        }
    }

    public void PlayOneShotSound()
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
            Debug.Log("Playing " + clip.name);
        }
        else
        {
            Debug.LogWarning("SoundClip or AudioSource is missing!");
        }
    }
    public void play(MusicTracks title)
    {
        switch (title)
        {
            case MusicTracks.test:
                Console.WriteLine("Play Music");
                break;
        }

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



    public void SetChannelVolume()
    {
        Console.WriteLine("Setting Channel Volume");
    }

    public void SetChannelMute()
    {
        Console.WriteLine("Muting Channel");
    }
}
