
public static class DialoguaPanelSettings
{
    private static SliderWithUI _typingRateSlider;
    private static DialoguePanel _dialoguePanel;

    public static void Initialize(DialoguePanel dialoguePanel, SliderWithUI typingSpeedSlider)
    {
        _dialoguePanel = dialoguePanel;
        _dialoguePanel.Initialize();
        _typingRateSlider = typingSpeedSlider;
        _typingRateSlider.onValueChanged.AddListener(ChangeTypingSpeed);
        float maxTypeRate = 50f;
        float minTypeRate = 5f;
        _typingRateSlider.maxValue = maxTypeRate;
        _typingRateSlider.minValue = minTypeRate;
        _typingRateSlider.value = (maxTypeRate + minTypeRate) / 2;
        _typingRateSlider.InitializeUI();
        ChangeTypingSpeed(_typingRateSlider.value);
    }

    private static void ChangeTypingSpeed(float typingRate) => _dialoguePanel.SetTypingSpeed(typingRate);
}
