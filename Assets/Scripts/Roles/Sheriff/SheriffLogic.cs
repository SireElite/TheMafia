using System;
using System.Collections.Generic;
using System.Linq;

public static class SheriffLogic
{
    public static Action<bool> OnPlayerChecked;

    public static bool CheckPlayer(Player playerToCheck)
    {
        bool isMafia;

        if(playerToCheck.Role is MafiaRole)
        {
            isMafia = true;
        }
        else
        {
            isMafia = false;
        }

        return isMafia;
    }

    public static List<Player> GetPossiblePlayersToCheck(List<Player> mafiaPlayers, List<Player> civilianPlayers)
    {
        List<Player> playersToCheck = Host.PlayersInGame.ToList();

        foreach(Player mafiaPlayer in mafiaPlayers)
            playersToCheck.Remove(mafiaPlayer);

        foreach(Player civilianPlayer in civilianPlayers)
            playersToCheck.Remove(civilianPlayer);

        return playersToCheck;
    }
}
