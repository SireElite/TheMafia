using System;
using System.Collections;
using System.Collections.Generic;

public class MafiaBotPlayerRoleBehaviour : BotPlayerRoleBehaviour
{
    public MafiaBotPlayerRoleBehaviour(Player player) : base(player) { }

    public override IEnumerator MakeDayVoteCoroutine(List<Player> playersToVote, Action<Player> onPlayerVoted)
    {
        onPlayerVoted.Invoke(GetRandomPlayerToVote());
        yield return null;
    }

    public override IEnumerator MakeNightVoteCoroutine()
    {
        MafiaLogic.VotePlayer(GetRandomPlayerToVote());
        yield return null;
    }

    private Player GetRandomPlayerToVote()
    {
        return Host.GetRandomPlayerFromList(MafiaLogic.GetPlayersToVote());
    }
}
