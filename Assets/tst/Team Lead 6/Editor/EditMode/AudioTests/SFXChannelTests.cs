using NUnit.Framework;
using UnityEngine;

public class SFXChannelCoreTests
{
    [Test]
    public void Play_DoesNotChangeSourceClip()
    {
        var go = new GameObject("SFXSourceClipTest");
        var source = go.AddComponent<AudioSource>();
        var channel = new SFXChannelCore(source);

        var initialClip = AudioClip.Create("initial_clip", 44100, 1, 44100, false);
        source.clip = initialClip;

        var cue = ScriptableObject.CreateInstance<SoundCue>();
        var sfxClip = AudioClip.Create("sfx_clip", 44100, 1, 44100, false);
        cue.SetClipsForTesting(new[] { sfxClip });

        channel.Play(cue, 0.5f);

        Assert.AreSame(initialClip, source.clip,
            "SFXChannelCore should not overwrite AudioSource.clip when using PlayOneShot.");
    }

    [Test]
    public void Play_DoesNotSetLooping()
    {
        var go = new GameObject("SFXLoopTest");
        var source = go.AddComponent<AudioSource>();
        var channel = new SFXChannelCore(source);

        var cue = ScriptableObject.CreateInstance<SoundCue>();
        var clip = AudioClip.Create("sfx_loop_test_clip", 44100, 1, 44100, false);
        cue.SetClipsForTesting(new[] { clip });

        channel.Play(cue, 0.8f);

        Assert.IsFalse(source.loop, "SFXChannelCore should leave loop disabled for SFX.");
    }

    [Test]
    public void Play_HandlesNullCueGracefully()
    {
        var go = new GameObject("SFXNullCueTest");
        var source = go.AddComponent<AudioSource>();
        var channel = new SFXChannelCore(source);

        // Should not throw or crash
        Assert.DoesNotThrow(() => channel.Play(null, 0.5f),
            "SFXChannelCore.Play should handle null cues gracefully.");
    }

    [Test]
    public void Play_HandlesCueWithNoClipsGracefully()
    {
        var go = new GameObject("SFXEmptyCueTest");
        var source = go.AddComponent<AudioSource>();
        var channel = new SFXChannelCore(source);

        var cue = ScriptableObject.CreateInstance<SoundCue>();
        cue.SetClipsForTesting(null);

        Assert.DoesNotThrow(() => channel.Play(cue, 0.5f),
            "SFXChannelCore.Play should handle SoundCues with no clips gracefully.");
    }
}
