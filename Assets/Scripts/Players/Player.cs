using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image), typeof(Button))]
public class Player : DialogueEntity
{
    [SerializeField] protected Image _skullImage;
    [SerializeField] protected TextMeshProUGUI _numberTMP;
    [SerializeField] protected PlayerVotesUI _votesUI;
    [SerializeField] protected TextAsset _dieLinesTextAsset;
    [SerializeField] protected TextAsset _voteLinesTextAsset;

    public static event Action<Player> OnPlayerDied;

    public PlayerRole Role { get; protected set; }
    public int Votes { get; protected set; }
    public int Number { get; protected set; }

    public override string DialogueName { get; protected set; }

    public event Action<Player> OnMainPlayerClicked;

    protected readonly Color _DeadCardColor = new Color(0.55f, 0.55f, 0.55f);

    protected Player _votedPlayer;
    protected Color _normalCardColor;

    protected string[] _voteLines;
    protected string[] _dieLines;

    protected Image _image;
    protected Sprite _cardBackSprite;
    protected Button _buttonForMainPlayerClick;

    public virtual void Initialize(int number)
    {
        _dieLines = _dieLinesTextAsset.text.Split("\n");
        _voteLines = _voteLinesTextAsset.text.Split('\n');

        _image = GetComponent<Image>();
        _buttonForMainPlayerClick = GetComponent<Button>();
        _buttonForMainPlayerClick.onClick.AddListener(HandleMainPlayerButtonClick);
        _cardBackSprite = _image.sprite;
        _normalCardColor = _image.color;
        SetNumber(number);
    }

    public void Restart()
    {
        HideRole();
        ClearVotes();
        Role = null;
        _image.color = _normalCardColor;
        _skullImage.enabled = false;
        _votesUI.DisableSelf();
        ClearVotes();
        StopAllCoroutines();
    }

    public virtual void SetRole(PlayerRole role) => Role = role;

    public void ToggleButton(bool isEnabled) => _buttonForMainPlayerClick.enabled = isEnabled;

    public void EnableVotesUI(PlayersBackgroundVotesUIColors color) => _votesUI.EnableSelf(color);

    public void DisableVotesUI() => _votesUI.DisableSelf();

    public void ShowRole() => _image.sprite = Role.RoleSprite;

    public void HideRole() => _image.sprite = _cardBackSprite;

    public IEnumerator DieCoroutine()
    {
        ShowRole();
        _image.color = _DeadCardColor;
        _skullImage.enabled = true;
        yield return StartCoroutine(TellRandomDieDialogueLineCoroutine());
        OnPlayerDied.Invoke(this);
    }

    public void SetNumber(int number)
    {
        Number = number;
        DialogueName = $"Player {Number}";
        _numberTMP.text = Number.ToString();
    }

    public void ReceiveVote()
    {
        Votes++;
        UpdateVotesTMP();
    }

    public void ClearVotes()
    {
        Votes = 0;
        UpdateVotesTMP();
    }

    public virtual IEnumerator AwakeningCoroutine()
    {
        yield return StartCoroutine(Role.MakeNightVote());
    }

    public virtual IEnumerator DayVoteCoroutine(List<Player> playersToVote, Action<Player> onPlayerVoted)
    {
        playersToVote.Remove(this);
        yield return StartCoroutine(Role.MakeDayVote(playersToVote, (Player votedPlayer) => _votedPlayer = votedPlayer));
        onPlayerVoted.Invoke(_votedPlayer);
        yield return StartCoroutine(TellVotedPlayerCoroutine(_votedPlayer));
    }

    protected void OnDisable() => StopAllCoroutines();

    protected void UpdateVotesTMP() => _votesUI.SetVotesTMP(Votes);

    private void HandleMainPlayerButtonClick() => OnMainPlayerClicked.Invoke(this);

    protected IEnumerator TellRandomDieDialogueLineCoroutine()
    {
        string dialogueLine = GetRandomDialogueLine(_dieLines);
        dialogueLine = TryToReplaceVariable(dialogueLine, RoleVariable, Role.Name);
        yield return StartCoroutine(SayDialogueLineCoroutine(dialogueLine));
    }

    private IEnumerator TellVotedPlayerCoroutine(Player player)
    {
        string dialogueLine = GetRandomDialogueLine(_voteLines);
        dialogueLine = TryToReplaceVariable(dialogueLine, PlayerNumberVariable, player.Number.ToString());
        yield return StartCoroutine(SayDialogueLineCoroutine(dialogueLine));
    }
}
