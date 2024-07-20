using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DayHostBehaviour : HostBehaviour
{
	private IReadOnlyList<Player> _alibiPlayers;
	private Player _nightKilledPlayer;
    private MonoBehaviour _hostMonoBehaviour;
    private string[] _noOneDiedLines;
    private string[] _noOneVotedOutLines;

    public DayHostBehaviour(IReadOnlyList<Player> alibiPlayers, Player killedPlayer, DialogueEntity dialogueEntity, 
                            string[] noOneDiedLines, string[] noOneVotedOutLines) : base(dialogueEntity)
	{
        _alibiPlayers = alibiPlayers;
        _nightKilledPlayer = killedPlayer;
        _hostDialogueEntity = dialogueEntity;
        _hostMonoBehaviour = _hostDialogueEntity;
        _noOneDiedLines = noOneDiedLines;
        _noOneVotedOutLines = noOneVotedOutLines;
    }

    public override void Enter() => _hostMonoBehaviour.StartCoroutine(StartDayCoroutine());

    private IEnumerator StartDayCoroutine()
    {
        yield return _hostMonoBehaviour.StartCoroutine(_hostDialogueEntity.SayDialogueLineCoroutine("Day begins..."));

        if(_nightKilledPlayer != null)
        {
            yield return _hostMonoBehaviour.StartCoroutine(_hostDialogueEntity.SayDialogueLineCoroutine($"Today we lost..."));
            yield return _hostMonoBehaviour.StartCoroutine(_hostDialogueEntity.SayDialogueLineCoroutine($"Player number {_nightKilledPlayer.Number}."));
            yield return _hostMonoBehaviour.StartCoroutine(_nightKilledPlayer.DieCoroutine());

            if(Host.IsGameEnded == false)
                _hostMonoBehaviour.StartCoroutine(VotingCoroutine());
        }
        else
        {
            string randomNoOneDiedLine = _hostDialogueEntity.GetRandomDialogueLine(_noOneDiedLines);
            yield return _hostMonoBehaviour.StartCoroutine(_hostDialogueEntity.SayDialogueLineCoroutine(randomNoOneDiedLine));
            _hostMonoBehaviour.StartCoroutine(VotingCoroutine());
        }
    }

    private IEnumerator VotingCoroutine()
    {
        yield return _hostMonoBehaviour.StartCoroutine(_hostDialogueEntity.SayDialogueLineCoroutine("We can start voting."));

        EnablePlayersVoteUI(PlayersBackgroundVotesUIColors.DayColor);

        Player votedPlayer = null;

        foreach(Player votingPlayer in Host.PlayersInGame)
        {
            yield return _hostMonoBehaviour.StartCoroutine(votingPlayer.DayVoteCoroutine(Host.PlayersInGame.ToList(), (onPlayerVoted) => votedPlayer = onPlayerVoted));
            AddVote(votedPlayer);
        }

        Player mostVotedPlayer = GetMostVotedPlayer(_votedPlayers.AsReadOnly());
        yield return new WaitForSeconds(0.3f);

        DisablePlayersVoteUI();

        if(_alibiPlayers.Contains(mostVotedPlayer) == false)
        {
            yield return _hostMonoBehaviour.StartCoroutine(mostVotedPlayer.DieCoroutine());
        }
        else
        {
            string noOneVotedOutLine = _hostDialogueEntity.GetRandomDialogueLine(_noOneVotedOutLines);
            noOneVotedOutLine = _hostDialogueEntity.TryToReplaceVariable(noOneVotedOutLine, DialogueEntity.PlayerNumberVariable, mostVotedPlayer.Number.ToString());
            yield return _hostMonoBehaviour.StartCoroutine(_hostDialogueEntity.SayDialogueLineCoroutine(noOneVotedOutLine));
        }

        ClearVotes();
        _alibiPlayers = null;
        yield return new WaitForSeconds(1f);

        if(Host.IsGameEnded == false)
            OnBehaviourEnded.Invoke(this);
    }
}
