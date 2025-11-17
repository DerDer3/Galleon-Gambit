using NUnit.Framework;
using UnityEngine;

public class MusicChannelCoreTests
{
    private SoundCue CreateCueWithVolume(float volume, bool loop = true)
    {
        var cue = ScriptableObject.CreateInstance<SoundCue>();
        var clip = AudioClip.Create("music_clip", 44100, 1, 44100, false);
        cue.SetClipsForTesting(new[] { clip });

        // We can't set private volume directly, but effectiveVolume is passed separately,
        // so here we focus on how Play affects the AudioSource.
        return cue;
    }

    [Test]
    public void Play_AssignsClipAndStartsPlayback()
    {
        var go = new GameObject("MusicSourceTest");
        var source = go.AddComponent<AudioSource>();
        var channel = new MusicChannelCore(source);

        var cue = ScriptableObject.CreateInstance<SoundCue>();
        var clip = AudioClip.Create("music_clip", 44100, 1, 44100, false);
        cue.SetClipsForTesting(new[] { clip });

        float effectiveVolume = 0.5f;

        channel.Play(cue, effectiveVolume);

        Assert.AreSame(clip, source.clip, "MusicChannelCore should assign the clip from the SoundCue.");
        Assert.IsTrue(source.isPlaying, "MusicChannelCore should start playback.");
    }

    [Test]
    public void Play_SetsLoopTrueForMusic()
    {
        var go = new GameObject("MusicSourceLoopTest");
        var source = go.AddComponent<AudioSource>();
        var channel = new MusicChannelCore(source);

        var cue = ScriptableObject.CreateInstance<SoundCue>();
        var clip = AudioClip.Create("music_loop_clip", 44100, 1, 44100, false);
        cue.SetClipsForTesting(new[] { clip });

        channel.Play(cue, 0.7f);

        Assert.IsTrue(source.loop, "MusicChannelCore should set loop = true.");
    }

    [Test]
    public void Play_UsesEffectiveVolume()
    {
        var go = new GameObject("MusicSourceVolumeTest");
        var source = go.AddComponent<AudioSource>();
        var channel = new MusicChannelCore(source);

        var cue = ScriptableObject.CreateInstance<SoundCue>();
        var clip = AudioClip.Create("music_volume_clip", 44100, 1, 44100, false);
        cue.SetClipsForTesting(new[] { clip });

        float effectiveVolume = 0.42f;

        channel.Play(cue, effectiveVolume);

        Assert.That(source.volume, Is.EqualTo(effectiveVolume).Within(0.001f),
            "MusicChannelCore should assign effective volume to the AudioSource.");
    }

    [Test]
    public void DynamicBinding_CallsMusicOverrideNotBase()
    {
        var go = new GameObject("DynamicBindingMusicTest");
        var source = go.AddComponent<AudioSource>();

        // IMPORTANT: reference typed as base class
        SoundChannelCore channel = new MusicChannelCore(source);

        var cue = ScriptableObject.CreateInstance<SoundCue>();
        var clip = AudioClip.Create("music_db_clip", 44100, 1, 44100, false);
        cue.SetClipsForTesting(new[] { clip });

        float effectiveVolume = 0.33f;

        channel.Play(cue, effectiveVolume);

        // We assert music-specific behavior: loop must be true for music
        Assert.IsTrue(source.loop,
            "When calling Play via a SoundChannelCore reference, the MusicChannelCore override should execute (loop = true).");
    }

    [Test]
    public void MusicChannelCore_CanBeConstructed()
    {
        var go = new GameObject("MusicSource_ForConstruction");
        var source = go.AddComponent<AudioSource>();

        SoundChannelCore channel = new MusicChannelCore(source);

        Assert.IsNotNull(channel, "MusicChannelCore should be constructible with a valid AudioSource.");
    }

    [Test]
    public void MusicChannelCore_Play_DoesNotThrow_WithNullCue()
    {
        var go = new GameObject("MusicSource_NullCue");
        var source = go.AddComponent<AudioSource>();
        SoundChannelCore channel = new MusicChannelCore(source);

        Assert.DoesNotThrow(() =>
        {
            channel.Play(null, 0.5f);
        }, "MusicChannelCore.Play should handle null cues without throwing.");
    }

    [Test]
    public void MusicChannelCore_Play_DoesNotThrow_WithEmptyCue()
    {
        var go = new GameObject("MusicSource_EmptyCue");
        var source = go.AddComponent<AudioSource>();
        SoundChannelCore channel = new MusicChannelCore(source);

        var cue = ScriptableObject.CreateInstance<SoundCue>();
        cue.SetClipsForTesting(new AudioClip[0]);

        Assert.DoesNotThrow(() =>
        {
            channel.Play(cue, 0.5f);
        }, "MusicChannelCore.Play should handle cues with no clips without throwing.");
    }

    [Test]
    public void MusicChannelCore_Play_DoesNotThrow_WithValidCue()
    {
        var go = new GameObject("MusicSource_ValidCue");
        var source = go.AddComponent<AudioSource>();
        SoundChannelCore channel = new MusicChannelCore(source);

        var cue = ScriptableObject.CreateInstance<SoundCue>();
        var clip = AudioClip.Create("music_clip", 44100, 1, 44100, false);
        cue.SetClipsForTesting(new[] { clip });

        Assert.DoesNotThrow(() =>
        {
            channel.Play(cue, 0.75f);
        }, "MusicChannelCore.Play should not throw with a valid cue and volume.");
    }

    // ---------- SFXChannelCore ----------

    [Test]
    public void SFXChannelCore_CanBeConstructed()
    {
        var go = new GameObject("SFXSource_ForConstruction");
        var source = go.AddComponent<AudioSource>();

        SoundChannelCore channel = new SFXChannelCore(source);

        Assert.IsNotNull(channel, "SFXChannelCore should be constructible with a valid AudioSource.");
    }

    [Test]
    public void SFXChannelCore_Play_DoesNotThrow_WithNullCue()
    {
        var go = new GameObject("SFXSource_NullCue");
        var source = go.AddComponent<AudioSource>();
        SoundChannelCore channel = new SFXChannelCore(source);

        Assert.DoesNotThrow(() =>
        {
            channel.Play(null, 0.5f);
        }, "SFXChannelCore.Play should handle null cues without throwing.");
    }

    [Test]
    public void SFXChannelCore_Play_DoesNotThrow_WithEmptyCue()
    {
        var go = new GameObject("SFXSource_EmptyCue");
        var source = go.AddComponent<AudioSource>();
        SoundChannelCore channel = new SFXChannelCore(source);

        var cue = ScriptableObject.CreateInstance<SoundCue>();
        cue.SetClipsForTesting(new AudioClip[0]);

        Assert.DoesNotThrow(() =>
        {
            channel.Play(cue, 0.5f);
        }, "SFXChannelCore.Play should handle cues with no clips without throwing.");
    }

    [Test]
    public void SFXChannelCore_Play_DoesNotThrow_WithValidCue()
    {
        var go = new GameObject("SFXSource_ValidCue");
        var source = go.AddComponent<AudioSource>();
        SoundChannelCore channel = new SFXChannelCore(source);

        var cue = ScriptableObject.CreateInstance<SoundCue>();
        var clip = AudioClip.Create("sfx_clip", 44100, 1, 44100, false);
        cue.SetClipsForTesting(new[] { clip });

        Assert.DoesNotThrow(() =>
        {
            channel.Play(cue, 1.0f);
        }, "SFXChannelCore.Play should not throw with a valid cue and volume.");
    }
}
