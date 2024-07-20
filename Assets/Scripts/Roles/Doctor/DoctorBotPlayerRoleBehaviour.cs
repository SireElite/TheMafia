using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DoctorBotPlayerRoleBehaviour : BotPlayerRoleBehaviour
{
    public DoctorBotPlayerRoleBehaviour(Player player) : base(player) { }

    public override IEnumerator MakeNightVoteCoroutine()
    {
        _possiblePlayersToVote = DoctorLogic.GetPossiblePlayersToHeal(Host.PlayersInGame.ToList(), _player);
        DoctorLogic.HealPlayer(Host.GetRandomPlayerFromList(_possiblePlayersToVote), _player);
        yield return null;
    }
}
