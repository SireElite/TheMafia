using UnityEngine.Audio;

public static class AudioSettings
{
    private const string MUSIC_VOLUME = "MusicVolume";
    private const string SOUND_VOLUME = "SoundVolume";

    private static AudioMixer _mainAudioMixer;
    private static SliderWithUI _musicVolumeSlider;
    private static SliderWithUI _soundVolumeSlider;

    public static void Initialize(AudioMixer mainAudioMixer, SliderWithUI musicVolumeSlider, SliderWithUI soundVolumeSlider)
    {
        _mainAudioMixer = mainAudioMixer;
        _musicVolumeSlider = musicVolumeSlider;
        _soundVolumeSlider = soundVolumeSlider;

        _soundVolumeSlider.onValueChanged.AddListener(SetSoundVolume);
        _musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);

        _musicVolumeSlider.InitializeUI();
        _soundVolumeSlider.InitializeUI();

        SetStartValues();
    }

    public static void SetStartValues()
    {
        float maxAudioMixerValue = 0;
        float minAudioMixerValue = -80;
        float startVolumeValue = -24;

        _musicVolumeSlider.maxValue = maxAudioMixerValue;
        _soundVolumeSlider.maxValue = maxAudioMixerValue;

        _musicVolumeSlider.minValue = minAudioMixerValue;
        _soundVolumeSlider.minValue = minAudioMixerValue;

        _musicVolumeSlider.value = startVolumeValue;
        _soundVolumeSlider.value = startVolumeValue;
    }

    public static void SetMusicVolume(float value)
    {
        _mainAudioMixer.SetFloat(MUSIC_VOLUME, value);
    }

    public static void SetSoundVolume(float value)
    {
        _mainAudioMixer.SetFloat(SOUND_VOLUME, value);
    }
}
