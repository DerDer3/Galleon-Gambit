using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Sound Cue")]
public class SoundCue : ScriptableObject
{
    [SerializeField] private AudioClip[] clips;
    [SerializeField, Range(0f, 1f)] private float volume = 1f;
    [SerializeField] private bool loop = false;

    public float Volume => volume;
    public bool Loop => loop;

    public AudioClip GetRandomClip()
    {
        if (clips == null || clips.Length == 0) return null;
        int index = UnityEngine.Random.Range(0, clips.Length);
        return clips[index];
    }
}
