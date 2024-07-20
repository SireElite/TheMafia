using System;
using System.Collections.Generic;

public static class ProstituteLogic
{
    public static Action<Player> OnPlayerToGiveAlibiVoted;

    private static Dictionary<Player, Player> LastAlibiPlayers = new Dictionary<Player, Player>();

    static ProstituteLogic()
    {
        Host.OnGameRestart += () => LastAlibiPlayers.Clear();
    }

    public static List<Player> GetPossiblePlayersToGiveAlibi(List<Player> possiblePlayersToGiveAlibi, Player user)
    {
        if(LastAlibiPlayers.ContainsKey(user))
            possiblePlayersToGiveAlibi.Remove(LastAlibiPlayers[user]);

        return possiblePlayersToGiveAlibi;
    }

    public static void GiveAlibiToPlayer(Player player, Player user)
    {
        LastAlibiPlayers[user] = player;
        OnPlayerToGiveAlibiVoted.Invoke(player);
    }
}
