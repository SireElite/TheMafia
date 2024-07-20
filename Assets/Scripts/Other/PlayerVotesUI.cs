using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVotesUI : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI _votesTMP;
    [SerializeField] protected Image _backgroundImage;

    private Color _dayColor = new Color(0.41f, 0.47f, 1);
    private Color _nightColor = Color.red;

    public void SetVotesTMP(int votes) => _votesTMP.text = votes.ToString();

    public void DisableSelf() => gameObject.SetActive(false);

    public void EnableSelf(PlayersBackgroundVotesUIColors color)
    {
        switch(color)
        {
            case PlayersBackgroundVotesUIColors.NightColor:
                _backgroundImage.color = _nightColor;
                break;
            case PlayersBackgroundVotesUIColors.DayColor:
                _backgroundImage.color = _dayColor;
                break;
        }

        gameObject.SetActive(true);
    }
}
