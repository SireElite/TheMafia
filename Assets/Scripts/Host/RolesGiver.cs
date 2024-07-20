using System.Collections.Generic;

public sealed class RolesGiver
{
    private List<Player> _receivedRolePlayers;
    private Player _playerToReceiveRole;
    private RolesCountSO _rolesCountSO;

    public void SetRolesCountSO(RolesCountSO rolesCountSO) => _rolesCountSO = rolesCountSO;

    public List<Player> GiveAwayMafia(List<Player> possiblePlayersToReceiveRole)
    {
        _receivedRolePlayers = new List<Player>();

        for(int i = 0; i < _rolesCountSO.MafiaCount; i++)
        {
            SetPlayerToReceiveRole(possiblePlayersToReceiveRole);
            _playerToReceiveRole.SetRole(new MafiaRole(_playerToReceiveRole));
        }

        //Main player should awake last
        if(MainPlayer.Instance.Role is MafiaRole)
        {
            _receivedRolePlayers.Remove(MainPlayer.Instance);
            _receivedRolePlayers.Add(MainPlayer.Instance);
        }

        return _receivedRolePlayers;
    }

    public List<Player> GiveAwayCitizens(List<Player> possiblePlayersToReceiveRole)
    {
        _receivedRolePlayers = new List<Player>();

        for(int i = 0; i < _rolesCountSO.CitizenCount; i++)
        {
            SetPlayerToReceiveRole(possiblePlayersToReceiveRole);
            _playerToReceiveRole.SetRole(new CitizenRole(_playerToReceiveRole));
        }
 
        return _receivedRolePlayers;
    }

    public List<Player> GiveAwayDoctors(List<Player> possiblePlayersToReceiveRole)
    {
        _receivedRolePlayers = new List<Player>();

        for(int i = 0; i < _rolesCountSO.DoctorCount; i++)
        {
            SetPlayerToReceiveRole(possiblePlayersToReceiveRole);
            _playerToReceiveRole.SetRole(new DoctorRole(_playerToReceiveRole));
        }

        return _receivedRolePlayers;
    }

    public List<Player> GiveAwayProstitutes(List<Player> possiblePlayersToReceiveRole)
    {
        _receivedRolePlayers = new List<Player>();

        for(int i = 0; i < _rolesCountSO.ProstituteCount; i++)
        {
            SetPlayerToReceiveRole(possiblePlayersToReceiveRole);
            _playerToReceiveRole.SetRole(new ProstituteRole(_playerToReceiveRole));
        }

        return _receivedRolePlayers;
    }

    public List<Player> GiveAwaySheriffs(List<Player> possiblePlayersToReceiveRole)
    {
        _receivedRolePlayers = new List<Player>();

        for(int i = 0; i < _rolesCountSO.SheriffCount; i++)
        {
            SetPlayerToReceiveRole(possiblePlayersToReceiveRole);
            _playerToReceiveRole.SetRole(new SheriffRole(_playerToReceiveRole));
        }

        return _receivedRolePlayers;
    }

    private void SetPlayerToReceiveRole(List<Player> possiblePlayersToReceiveRole)
    {
        _playerToReceiveRole = Host.GetRandomPlayerFromList(possiblePlayersToReceiveRole);
        possiblePlayersToReceiveRole.Remove(_playerToReceiveRole);
        _receivedRolePlayers.Add(_playerToReceiveRole);
    }
}
