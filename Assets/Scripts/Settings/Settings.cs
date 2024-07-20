using UnityEngine;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private GameObject _settingsUI;
    [SerializeField] private SettingsToggleButton _settingsToggleButton;

    [Header("AudioSettings")]
    [SerializeField] private SliderWithUI _musicVolumeSlider;
    [SerializeField] private SliderWithUI _soundVolumeSlider;
    [SerializeField] private AudioMixer _mainAudioMixer;

    [Header("DialoguePanelSettings")]
    [SerializeField] private SliderWithUI _typingRateSlider;
    [SerializeField] private DialoguePanel _dialoguePanel;

    private void Awake()
    {
        AudioSettings.Initialize(_mainAudioMixer, _musicVolumeSlider, _soundVolumeSlider);
        DialoguaPanelSettings.Initialize(_dialoguePanel, _typingRateSlider);
    }

    private void OnEnable()
    {
        _settingsToggleButton.OnSettingsShouldOpen += EnableSettingsUI;
        _settingsToggleButton.OnSettingsShouldClose += DisableSettingsUI;
    }

    private void OnDisable()
    {
        _settingsToggleButton.OnSettingsShouldOpen -= EnableSettingsUI;
        _settingsToggleButton.OnSettingsShouldClose -= DisableSettingsUI;
    }

    private void EnableSettingsUI() => _settingsUI.SetActive(true);

    private void DisableSettingsUI() => _settingsUI.SetActive(false);
}
