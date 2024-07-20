using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayersCountToggleGroup : ToggleGroup
{
    [SerializeField] private RolesCountSO_Toggle[] _toggles;

    public static Action<RolesCountSO> OnFirstPlayersCountSOAssigned;

    protected override void Start()
    {
        base.Start();
        RolesCountSO_Toggle lastToggle = _toggles[_toggles.Length - 1];
        lastToggle.isOn = true;
        lastToggle.interactable = false;
        OnFirstPlayersCountSOAssigned.Invoke(lastToggle.RolesCountSO);

        foreach(RolesCountSO_Toggle toggle in _toggles)
        {
            toggle.group = this;
            toggle.onValueChanged.AddListener((bool b) => HandleChangedValue(toggle));
        }
    }

    private void HandleChangedValue(RolesCountSO_Toggle toggle)
    {
        if(toggle.isOn)
        {
            toggle.interactable = false;
            toggle.TriggerActivatedEvent();
        }
        else
        {
            toggle.interactable = true;
        }
    }
}
