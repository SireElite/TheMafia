using System;
using System.Collections;
using System.Collections.Generic;

public abstract class RoleBehaviour
{
    protected List<Player> _possiblePlayersToVote;

    public virtual IEnumerator MakeDayVoteCoroutine(List<Player> playersToVote, Action<Player> onPlayerVoted)
    {
        onPlayerVoted.Invoke(Host.GetRandomPlayerFromList(playersToVote));
        yield return null;
    }

    public virtual IEnumerator MakeNightVoteCoroutine() => throw new Exception("Base MakeNightVoteCoroutine should not be called");
}
