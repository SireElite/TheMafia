using System;
using System.Collections;
using System.Collections.Generic;

public class MainPlayer : Player
{
    public static MainPlayer Instance { get; private set; }

    public static event Action OnMainPlayerVoteStarted;
    public static event Action OnMainPlayerVoteEnded;

    public override void Initialize(int number)
    {
        base.Initialize(number);

        #region Singleton
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            throw new System.Exception($"More than one {name} Instance");
        }
        #endregion
    }

    public override void SetRole(PlayerRole role)
    {
        Role = role;
        ShowRole();
    }

    public override IEnumerator AwakeningCoroutine()
    {
        OnMainPlayerVoteStarted.Invoke();
        yield return StartCoroutine(Role.MakeNightVote());
        OnMainPlayerVoteEnded.Invoke();
    }

    public override IEnumerator DayVoteCoroutine(List<Player> players, Action<Player> onPlayerVoted)
    {
        OnMainPlayerVoteStarted.Invoke();
        yield return StartCoroutine(Role.MakeDayVote(players, onPlayerVoted));
        OnMainPlayerVoteEnded.Invoke();
    }
}
