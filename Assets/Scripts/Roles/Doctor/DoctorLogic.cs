using System.Collections.Generic;
using System;

public static class DoctorLogic
{
    public static Action<Player> OnPlayerToHealVoted;

    private static Dictionary<Player, Player> LastHealedPlayers = new Dictionary<Player, Player>();

    static DoctorLogic()
    {
        Host.OnGameRestart += () => LastHealedPlayers.Clear();
    }

    public static List<Player> GetPossiblePlayersToHeal(List<Player> possibleToHealPlayers, Player user)
    {
        if(LastHealedPlayers.ContainsKey(user))
            possibleToHealPlayers.Remove(LastHealedPlayers[user]);

        return possibleToHealPlayers;
    }

    public static void HealPlayer(Player player, Player user)
    {
        LastHealedPlayers[user] = player;
        OnPlayerToHealVoted.Invoke(player);
    }
}
