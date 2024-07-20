using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Button), typeof(Image))]
public class SettingsToggleButton : MonoBehaviour
{
    [SerializeField] private Color ClosedSettingsButtonColor;
    [SerializeField] private Color OpenedSettingsButtonColor;

    public event Action OnSettingsShouldOpen;
    public event Action OnSettingsShouldClose;

    private Image _image;
    private bool _areSettingsOpened;

    private void Awake()
    {
        _image = GetComponent<Image>();
        Button button = GetComponent<Button>();
        button.onClick.AddListener(ToggleSettings);
    }

    private void ToggleSettings()
    {
        if(_areSettingsOpened)
        {
            _image.color = ClosedSettingsButtonColor;
            OnSettingsShouldClose.Invoke();
        }
        else
        {
            _image.color = OpenedSettingsButtonColor;
            OnSettingsShouldOpen.Invoke();
        }

        _areSettingsOpened = !_areSettingsOpened;
    }
}
