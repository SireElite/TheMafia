using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class MainPlayersTurnUI : MonoBehaviour
{
    private TextMeshProUGUI _mainPlayerTurnTMP;
    private const string _defaultText = "Your turn";

    private void Awake()
    {
        _mainPlayerTurnTMP = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        MainPlayer.OnMainPlayerVoteStarted += EnableTMP;
        MainPlayer.OnMainPlayerVoteEnded += DisableTMP;
    }

    private void OnDisable()
    {
        MainPlayer.OnMainPlayerVoteStarted -= EnableTMP;
        MainPlayer.OnMainPlayerVoteEnded -= DisableTMP;
    }

    private IEnumerator AnimateTMPCorotuine()
    {
        int dotsCount = 0;
        _mainPlayerTurnTMP.text = _defaultText;

        while(_mainPlayerTurnTMP.enabled)
        {
            yield return new WaitForSeconds(1f);

            if(dotsCount < 3)
            {
                _mainPlayerTurnTMP.text += ".";
                dotsCount++;
            }
            else
            {
                _mainPlayerTurnTMP.text = _defaultText;
                dotsCount = 0;
            }
        }
    }

    private void EnableTMP()
    {
        _mainPlayerTurnTMP.enabled = true;
        Host.OnGameRestart += DisableTMP;
        StartCoroutine(AnimateTMPCorotuine());
    }

    private void DisableTMP()
    {
        Host.OnGameRestart -= DisableTMP;
        _mainPlayerTurnTMP.enabled = false;
    }
}
