using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SheriffHintUI : MonoBehaviour
{
    [SerializeField] private GameObject _UI;
    [SerializeField] private TextMeshProUGUI _civiliansNumbersTMP;
    [SerializeField] private TextMeshProUGUI _mafiaNumbersTMP;

    private List<Player> _checkedPlayers = new List<Player>();

    private void OnEnable()
    {
        Host.OnMainPlayerGotRole += CheckIfMainPlayerSheriff;
    }

    private void OnDisable()
    {
        Host.OnMainPlayerGotRole -= CheckIfMainPlayerSheriff;
    }

    private void CheckIfMainPlayerSheriff(PlayerRole role)
    {
        if(role is SheriffRole)
            EnableUI();
    }

    private void EnableUI()
    {
        _UI.SetActive(true);
        Host.OnGameRestart += DisableUI;
        SheriffMainPlayerRoleBehaviour.OnMainPlayerSheriffChecked += DisplayHint;
        _mafiaNumbersTMP.text = string.Empty;
        _civiliansNumbersTMP.text = string.Empty;
    }

    private void DisableUI()
    {
        SheriffMainPlayerRoleBehaviour.OnMainPlayerSheriffChecked -= DisplayHint;
        Host.OnGameRestart -= DisableUI;
        _UI.SetActive(false);
    }

    private void DisplayHint(Player checkedPlayer, bool checkedPlayerIsMafia)
    {
        if(_checkedPlayers.Contains(checkedPlayer))
            return;

        if(checkedPlayerIsMafia)
        {
            _mafiaNumbersTMP.text = _mafiaNumbersTMP.text + $"{checkedPlayer.Number}; ";
        }
        else
        {
            _civiliansNumbersTMP.text = _civiliansNumbersTMP.text + $"{checkedPlayer.Number}; ";
        }

        _checkedPlayers.Add(checkedPlayer);
    }
}
