using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class MainPlayerRoleBehaviour : RoleBehaviour
{
	protected MainPlayer _mainPlayer;
    protected bool _isMainPlayerVoting;
    protected Player _votedPlayer;

    public MainPlayerRoleBehaviour(MainPlayer mainPlayer) => _mainPlayer = mainPlayer;

    public override IEnumerator MakeDayVoteCoroutine(List<Player> playersToVote, Action<Player> onPlayerVoted)
    {
        _possiblePlayersToVote = Host.PlayersInGame.ToList();
        yield return _mainPlayer.StartCoroutine(StartMainPlayersVoteCoroutine());
        onPlayerVoted.Invoke(_votedPlayer);
    }

    protected IEnumerator StartMainPlayersVoteCoroutine()
    {
        _isMainPlayerVoting = true;

        foreach(Player player in _possiblePlayersToVote)
        {
            player.OnMainPlayerClicked += EndMainPlayersVote;
            player.ToggleButton(true);
        }

        yield return new WaitWhile(() => _isMainPlayerVoting);
    }

    protected void EndMainPlayersVote(Player votedPlayer)
    {
        foreach(Player player in _possiblePlayersToVote)
        {
            player.OnMainPlayerClicked -= EndMainPlayersVote;
            player.ToggleButton(false);
        }

        _votedPlayer = votedPlayer;
        _isMainPlayerVoting = false;
    }
}
