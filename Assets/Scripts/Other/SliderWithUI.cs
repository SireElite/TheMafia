using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SliderWithUI : Slider
{
    [SerializeField] private TextMeshProUGUI _valueTMP;

    public void InitializeUI()
    {
        ChangeValueTMP(value);
        onValueChanged.AddListener(ChangeValueTMP);
    }

    protected void ChangeValueTMP(float sliderValue)
    {
        float valuePercentage = ((sliderValue - (minValue)) / (maxValue - minValue)) * 100f;
        _valueTMP.text = $"{Mathf.Round(valuePercentage)}%";
    }
}
