using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools; // for LogAssert

public class SoundManagerTests
{
    private SoundManager CreateSoundManagerWithBasicSetup(
        out AudioSource musicSource,
        out AudioSource sfxSource)
    {
        var go = new GameObject("TestSoundManager");
        musicSource = new GameObject("MusicSource").AddComponent<AudioSource>();
        sfxSource   = new GameObject("SFXSource").AddComponent<AudioSource>();

        var manager = go.AddComponent<SoundManager>();

        // Assign sources via serialized fields using reflection if needed,
        // but if they're [SerializeField] and not readonly, you can just do:
        typeof(SoundManager)
            .GetField("musicSource", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(manager, musicSource);

        typeof(SoundManager)
            .GetField("sfxSource", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(manager, sfxSource);

        // Manually invoke Awake so it initializes channels
        var awake = typeof(SoundManager).GetMethod("Awake",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        awake?.Invoke(manager, null);

        return manager;
    }

    [Test]
    public void PlayMusic_UsesCorrectCueClip()
    {
        var managerGO = new GameObject("SoundManagerTestRoot");
        var manager   = managerGO.AddComponent<SoundManager>();

        var musicSource = new GameObject("MusicSource").AddComponent<AudioSource>();
        typeof(SoundManager)
            .GetField("musicSource", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(manager, musicSource);

        var sfxSource = new GameObject("SFXSource").AddComponent<AudioSource>();
        typeof(SoundManager)
            .GetField("sfxSource", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(manager, sfxSource);

        // Build a simple music library: [MainMenu -> cue]
        var cue = ScriptableObject.CreateInstance<SoundCue>();
        var clip = AudioClip.Create("menu_music", 44100, 1, 44100, false);
        cue.SetClipsForTesting(new[] { clip });

        var musicData = new MusicTrackData
        {
            track = MusicTracks.Main,
            cue   = cue
        };

        var lib = new[] { musicData };
        typeof(SoundManager)
            .GetField("musicLibrary", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(manager, lib);

        // Initialize
        var awake = typeof(SoundManager).GetMethod("Awake",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        awake?.Invoke(manager, null);

        // Act
        manager.play(MusicTracks.Main);

        // Assert
        Assert.AreSame(clip, musicSource.clip,
            "SoundManager.Play(MusicTracks) should pass the correct cue to the music channel, resulting in the correct clip on the AudioSource.");
    }

    [Test]
    public void PlayMusic_LogsWarning_WhenTrackNotFound()
    {
        var managerGO = new GameObject("SoundManagerWarnTest");
        var manager   = managerGO.AddComponent<SoundManager>();

        var musicSource = new GameObject("MusicSource").AddComponent<AudioSource>();
        typeof(SoundManager)
            .GetField("musicSource", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(manager, musicSource);

        var sfxSource = new GameObject("SFXSource").AddComponent<AudioSource>();
        typeof(SoundManager)
            .GetField("sfxSource", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(manager, sfxSource);

        // Empty library
        typeof(SoundManager)
            .GetField("musicLibrary", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(manager, new MusicTrackData[0]);

        var awake = typeof(SoundManager).GetMethod("Awake",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        awake?.Invoke(manager, null);

        // Expect a warning when trying to play a missing track
        LogAssert.Expect(LogType.Warning, "Music Track 'MainMenu' not found in the library. Cannot play.");

        manager.play(MusicTracks.Main);
    }

    [Test]
    public void VolumeSetters_UpdateBackingFields()
    {
        var managerGO = new GameObject("SoundManagerVolumeTest");
        var manager = managerGO.AddComponent<SoundManager>();

        // Use reflection to call the volume setters and check the properties
        manager.SetMasterVolume(0.2f);
        manager.SetMusicVolume(0.5f);
        manager.SetSFXVolume(0.8f);

        Assert.AreEqual(0.2f, manager.MasterVolume, 0.001f);
        Assert.AreEqual(0.5f, manager.MusicVolume, 0.001f);
        Assert.AreEqual(0.8f, manager.SFXVolume, 0.001f);
    }
    
    private SoundManager CreateBareSoundManager(out AudioSource musicSource, out AudioSource sfxSource)
    {
        var root = new GameObject("SoundManagerRoot");
        var manager = root.AddComponent<SoundManager>();

        var musicGO = new GameObject("MusicSource");
        musicSource = musicGO.AddComponent<AudioSource>();

        var sfxGO = new GameObject("SFXSource");
        sfxSource = sfxGO.AddComponent<AudioSource>();

        // Assign via inspector-like access if fields are [SerializeField]
        // Adjust names if yours differ.
        typeof(SoundManager)
            .GetField("musicSource", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(manager, musicSource);

        typeof(SoundManager)
            .GetField("sfxSource", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(manager, sfxSource);

        // Awake should run automatically on AddComponent, but in case it doesn’t, we can manually invoke
        var awake = typeof(SoundManager).GetMethod("Awake",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        awake?.Invoke(manager, null);

        return manager;
    }

    // ---------- Construction & basic existence ----------

    [Test]
    public void SoundManager_CanBeCreated()
    {
        var go = new GameObject("SimpleSoundManager");
        var manager = go.AddComponent<SoundManager>();

        Assert.IsNotNull(manager, "SoundManager component should be added successfully.");
    }

    [Test]
    public void SoundManager_CanBeCreated_WithAudioSources()
    {
        var manager = CreateBareSoundManager(out var musicSource, out var sfxSource);

        Assert.IsNotNull(manager, "SoundManager should not be null after setup.");
        Assert.IsNotNull(musicSource, "MusicSource should not be null.");
        Assert.IsNotNull(sfxSource, "SFXSource should not be null.");
    }

    // ---------- Volume setters: master ----------

    [TestCase(0f)]
    [TestCase(0.5f)]
    [TestCase(1f)]
    public void SoundManager_SetMasterVolume_DoesNotThrow(float value)
    {
        var manager = new GameObject("SM_Master").AddComponent<SoundManager>();

        Assert.DoesNotThrow(() =>
        {
            manager.SetMasterVolume(value);
        }, $"SetMasterVolume({value}) should not throw.");
    }

    // ---------- Volume setters: music ----------

    [TestCase(0f)]
    [TestCase(0.25f)]
    [TestCase(0.75f)]
    public void SoundManager_SetMusicVolume_DoesNotThrow(float value)
    {
        var manager = new GameObject("SM_Music").AddComponent<SoundManager>();

        Assert.DoesNotThrow(() =>
        {
            manager.SetMusicVolume(value);
        }, $"SetMusicVolume({value}) should not throw.");
    }
}
