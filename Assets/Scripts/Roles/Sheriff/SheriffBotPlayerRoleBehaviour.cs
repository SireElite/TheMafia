using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SheriffBotPlayerRoleBehaviour : BotPlayerRoleBehaviour
{
    private List<Player> _uncheckedPlayers = new List<Player>();
    private List<Player> _mafias = new List<Player>();
    private List<Player> _civilians = new List<Player>();

    public SheriffBotPlayerRoleBehaviour(Player player) : base(player)
    {
        _uncheckedPlayers = Host.PlayersInGame.ToList();
        _uncheckedPlayers.Remove(player);
        Player.OnPlayerDied += TryToRemoveFromList;
        Host.OnGameRestart += PrepareToRestart;
    }

    public override IEnumerator MakeNightVoteCoroutine()
    {
        Player randomPlayerToCheck = Host.GetRandomPlayerFromList(_uncheckedPlayers);

        if(SheriffLogic.CheckPlayer(randomPlayerToCheck))
        {
            _mafias.Add(randomPlayerToCheck);
        }
        else
        {
            _civilians.Add(randomPlayerToCheck);
        }

        _uncheckedPlayers.Remove(randomPlayerToCheck);
        yield return null;
    }

    public override IEnumerator MakeDayVoteCoroutine(List<Player> playersToVote, Action<Player> onPlayerVoted)
    {
        if(_mafias.Count > 0)
        {
            onPlayerVoted.Invoke(Host.GetRandomPlayerFromList(_mafias));
        }
        else if(_civilians.Count > 0)
        {
            foreach(Player civilian in _civilians)
                playersToVote.Remove(civilian);

            onPlayerVoted.Invoke(Host.GetRandomPlayerFromList(playersToVote));
        }

        yield return null;
    }

    private void TryToRemoveFromList(Player diedPlayer)
    {
        _uncheckedPlayers.Remove(diedPlayer);
        _mafias.Remove(diedPlayer);
        _civilians.Remove(diedPlayer);
    }

    private void PrepareToRestart()
    {
        Player.OnPlayerDied -= TryToRemoveFromList;
        Host.OnGameRestart -= PrepareToRestart;
    }
}
