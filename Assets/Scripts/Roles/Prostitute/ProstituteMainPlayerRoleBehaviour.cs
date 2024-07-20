using System;
using System.Collections;
using System.Linq;

public class ProstituteMainPlayerRoleBehaviour : MainPlayerRoleBehaviour
{
    public ProstituteMainPlayerRoleBehaviour(MainPlayer mainPlayer) : base(mainPlayer) { }

    public override IEnumerator MakeNightVoteCoroutine()
    {
        _possiblePlayersToVote = ProstituteLogic.GetPossiblePlayersToGiveAlibi(Host.PlayersInGame.ToList(), _mainPlayer);
        yield return _mainPlayer.StartCoroutine(StartMainPlayersVoteCoroutine());
        ProstituteLogic.GiveAlibiToPlayer(_votedPlayer, _mainPlayer);
    }
}
