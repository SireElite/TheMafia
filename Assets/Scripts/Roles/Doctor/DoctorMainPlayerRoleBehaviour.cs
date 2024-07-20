using System;
using System.Collections;
using System.Linq;

public class DoctorMainPlayerRoleBehaviour : MainPlayerRoleBehaviour
{
    public DoctorMainPlayerRoleBehaviour(MainPlayer mainPlayer) : base(mainPlayer) { }

    public override IEnumerator MakeNightVoteCoroutine()
    {
        _possiblePlayersToVote = DoctorLogic.GetPossiblePlayersToHeal(Host.PlayersInGame.ToList(), _mainPlayer);
        yield return _mainPlayer.StartCoroutine(StartMainPlayersVoteCoroutine());
        DoctorLogic.HealPlayer(_votedPlayer, _mainPlayer);
    }
}
