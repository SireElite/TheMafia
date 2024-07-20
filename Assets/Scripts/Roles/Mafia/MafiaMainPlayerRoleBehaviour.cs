using System.Collections;
using System.Linq;

public class MafiaMainPlayerRoleBehaviour : MainPlayerRoleBehaviour
{
    public MafiaMainPlayerRoleBehaviour(MainPlayer mainPlayer) : base(mainPlayer) { }

    public override IEnumerator MakeNightVoteCoroutine()
    {
        _possiblePlayersToVote = Host.PlayersInGame.ToList();
        yield return _mainPlayer.StartCoroutine(StartMainPlayersVoteCoroutine());
        MafiaLogic.VotePlayer(_votedPlayer);
    }
}
