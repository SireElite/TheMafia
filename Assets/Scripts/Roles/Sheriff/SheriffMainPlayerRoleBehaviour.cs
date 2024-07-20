using System;
using System.Collections;
using System.Linq;

public class SheriffMainPlayerRoleBehaviour : MainPlayerRoleBehaviour
{
    public static Action<Player, bool> OnMainPlayerSheriffChecked;

    public SheriffMainPlayerRoleBehaviour(MainPlayer mainPlayer) : base(mainPlayer) { }

    public override IEnumerator MakeNightVoteCoroutine()
    {
        _possiblePlayersToVote = Host.PlayersInGame.ToList();
        yield return _mainPlayer.StartCoroutine(StartMainPlayersVoteCoroutine());
        OnMainPlayerSheriffChecked.Invoke(_votedPlayer, SheriffLogic.CheckPlayer(_votedPlayer));
    }
}
