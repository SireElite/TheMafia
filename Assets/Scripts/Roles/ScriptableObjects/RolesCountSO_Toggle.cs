using System;
using UnityEngine;
using UnityEngine.UI;

public class RolesCountSO_Toggle : Toggle
{
    [SerializeField] private RolesCountSO _rolesCountSO;

    public static event Action<RolesCountSO> OnToggleActivated;

    public RolesCountSO RolesCountSO => _rolesCountSO;

    public void TriggerActivatedEvent() => OnToggleActivated.Invoke(_rolesCountSO);
}
