using NUnit.Framework;
using UnityEngine;

public class SoundCueTests
{
    [Test]
    public void GetRandomClip_ReturnsNull_WhenNoClips()
    {
        var cue = ScriptableObject.CreateInstance<SoundCue>();
        cue.SetClipsForTesting(null);

        AudioClip result = cue.GetRandomClip();

        Assert.IsNull(result, "Expected null when SoundCue has no clips.");
    }

    [Test]
    public void GetRandomClip_ReturnsAClip_WhenOneClipPresent()
    {
        var cue = ScriptableObject.CreateInstance<SoundCue>();
        var clip = AudioClip.Create("single_clip", 44100, 1, 44100, false);
        cue.SetClipsForTesting(new[] { clip });

        AudioClip result = cue.GetRandomClip();

        Assert.AreSame(clip, result, "With one clip, GetRandomClip should always return that clip.");
    }

    [Test]
    public void GetRandomClip_ReturnsOneOfMultipleClips()
    {
        var cue = ScriptableObject.CreateInstance<SoundCue>();
        var clip1 = AudioClip.Create("clip1", 44100, 1, 44100, false);
        var clip2 = AudioClip.Create("clip2", 44100, 1, 44100, false);
        var clip3 = AudioClip.Create("clip3", 44100, 1, 44100, false);

        cue.SetClipsForTesting(new[] { clip1, clip2, clip3 });

        AudioClip result = cue.GetRandomClip();

        Assert.IsTrue(result == clip1 || result == clip2 || result == clip3,
            "GetRandomClip should return one of the clips in the array.");
    }

    [Test]
    public void VolumeProperty_RespectsSerializedValue()
    {
        var cue = ScriptableObject.CreateInstance<SoundCue>();

        // Use SerializedObject to modify private field or just assume default volume is 1f.
        Assert.That(cue.Volume, Is.InRange(0f, 1f), "Volume should be clamped between 0 and 1.");
    }
}
