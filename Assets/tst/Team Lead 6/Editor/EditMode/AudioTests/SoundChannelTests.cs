using NUnit.Framework;
using UnityEngine;

public class SoundChannelTests
{
    [Test]
    public void MusicChannel_Play_SetsLoopAndVolumeFromCue()
    {
        // Arrange: make an AudioSource for the channel to control
        var go = new GameObject("MusicSourceTest");
        var audioSource = go.AddComponent<AudioSource>();

        // Create a cue with a single clip and known volume
        var cue = ScriptableObject.CreateInstance<SoundCue>();

        var clip = AudioClip.Create("music_clip", 44100, 1, 44100, false);
        cue.SetClipsForTesting(new[] { clip });

        // We’ll simulate effective volume = 0.5
        float effectiveVolume = 0.5f;

        // Create the polymorphic channel
        SoundChannelCore channel = new MusicChannelCore(audioSource); // dynamic binding target

        // Act
        channel.Play(cue, effectiveVolume);

        // Assert
        Assert.AreEqual(clip, audioSource.clip, "MusicChannel should assign the clip from the SoundCue.");
        Assert.IsTrue(audioSource.loop, "MusicChannel should set loop = true for music.");
        Assert.AreEqual(effectiveVolume, audioSource.volume, 0.001f, "MusicChannel should use the effective volume.");
    }
}
