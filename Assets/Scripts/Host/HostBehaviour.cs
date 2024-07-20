using System;
using System.Collections.Generic;

public abstract class HostBehaviour
{
    public static Action<HostBehaviour> OnBehaviourEnded;

    protected List<Player> _votedPlayers = new List<Player>();
    protected DialogueEntity _hostDialogueEntity;

    public HostBehaviour(DialogueEntity dialogueEntity) => _hostDialogueEntity = dialogueEntity;

    public abstract void Enter();

    protected void AddVote(Player player)
    {
        player.ReceiveVote();

        if(_votedPlayers.Contains(player) == false)
            _votedPlayers.Add(player);
    }

    protected void EnablePlayersVoteUI(PlayersBackgroundVotesUIColors color)
    {
        foreach(Player player in Host.PlayersInGame)
            player.EnableVotesUI(color);
    }

    protected void DisablePlayersVoteUI()
    {
        foreach(Player player in Host.PlayersInGame)
            player.DisableVotesUI();
    }

    protected void ClearVotes()
    {
        foreach(var player in _votedPlayers)
            player.ClearVotes();

        _votedPlayers.Clear();
    }

    protected Player GetMostVotedPlayer(IList<Player> players)
    {
        Player mostVotedPlayer = null;

        foreach(Player player in players)
        {
            if(mostVotedPlayer == null)
                mostVotedPlayer = player;

            if(player.Votes == mostVotedPlayer.Votes)
            {
                int randomIndex = UnityEngine.Random.Range(0, 2);
                mostVotedPlayer = randomIndex == 0 ? mostVotedPlayer : player;
            }
            else if(player.Votes > mostVotedPlayer.Votes)
            {
                mostVotedPlayer = player;
            }
        }

        return mostVotedPlayer;
    }
}
