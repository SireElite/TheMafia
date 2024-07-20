using System;
using System.Collections;
using System.Linq;

public class ProstituteBotPlayerRoleBehaviour : BotPlayerRoleBehaviour
{
    public ProstituteBotPlayerRoleBehaviour(Player player) : base(player) { }

    public override IEnumerator MakeNightVoteCoroutine()
    {
        _possiblePlayersToVote = ProstituteLogic.GetPossiblePlayersToGiveAlibi(Host.PlayersInGame.ToList(), _player);
        ProstituteLogic.GiveAlibiToPlayer(Host.GetRandomPlayerFromList(_possiblePlayersToVote), _player);
        yield return null;
    }
}
