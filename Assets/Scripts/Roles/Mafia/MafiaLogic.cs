using System;
using System.Collections.Generic;
using System.Linq;

public static class MafiaLogic
{
    public static Action<Player> OnMafiaPlayerVoted;

    public static void VotePlayer(Player player) => OnMafiaPlayerVoted.Invoke(player);

    public static List<Player> GetPlayersToVote() => Host.TownCivilians.ToList();
}
