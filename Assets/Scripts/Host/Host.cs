using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;

public class Host : DialogueEntity
{
    [SerializeField] private Player _playerPrefab;
    [SerializeField] private MainPlayer _mainPlayer;
    [SerializeField] private GridLayoutGroup _playerGridLayour;
    [SerializeField] private Button _restartButton;
    [SerializeField] private TextAsset _noOneVotedOutLinesTextAsset;
    [SerializeField] private TextAsset _noOneDiedLinesTextAsset;

    public static IReadOnlyList<Player> PlayersInGame => _playersInGame;
    public static IReadOnlyList<Player> TownCivilians => _townCivilians;
    public static IReadOnlyList<Player> Prostitutes => _prostitutes;
    public static IReadOnlyList<Player> Sheriffs => _sheriffs;
    public static IReadOnlyList<Player> Doctors => _doctors;
    public static IReadOnlyList<Player> Citizens => _citizens;
    public static IReadOnlyList<Player> Mafia => _mafias;

    public static HostBehaviour CurrentBehaviour { get; private set; }

    public static bool IsGameEnded;

    public static event Action<PlayerRole> OnMainPlayerGotRole;
    public static event Action OnGameRestart;

    public override string DialogueName { get; protected set; } = "Host";

    private static List<Player> _playersInGame = new List<Player>();
    private static List<Player> _mafias  = new List<Player>();
    private static List<Player> _citizens = new List<Player>();
    private static List<Player> _prostitutes = new List<Player>();
    private static List<Player> _doctors = new List<Player>();
    private static List<Player> _sheriffs = new List<Player>();
    private static List<Player> _townCivilians = new List<Player>();

    private string[] _noOneDiedLines;
    private string[] _noOneVotedOutLines;
    private RolesCountSO _currentRolesCountSO;
    private RolesGiver _rolesGiver = new RolesGiver();
    private List<Player> _totalBotPlayers = new List<Player>();

    private void Initialize(RolesCountSO rolesCountSO)
    {
        _noOneDiedLines = _noOneDiedLinesTextAsset.text.Split("\n");
        _noOneVotedOutLines = _noOneVotedOutLinesTextAsset.text.Split("\n");

        _currentRolesCountSO = rolesCountSO;
        _mainPlayer.Initialize(_currentRolesCountSO.RolesCount);
        PlayersCountToggleGroup.OnFirstPlayersCountSOAssigned -= Initialize;

        InstantiateBotPlayers(_currentRolesCountSO.RolesCount - 1);
        StartGame();
    }

    private void OnEnable()
    {
        HostBehaviour.OnBehaviourEnded += GoToNextBehaviour;
        Player.OnPlayerDied += RemovePlayerFromList;
        PlayersCountToggleGroup.OnFirstPlayersCountSOAssigned += Initialize;
        RolesCountSO_Toggle.OnToggleActivated += ChangePlayersCount;
        _restartButton.onClick.AddListener(RestartGame);
    }

    private void InstantiateBotPlayers(int count)
    {
        for(int i = 1; i <= count; i++)
        {
            GameObject playerGO = Instantiate(_playerPrefab.gameObject, _playerGridLayour.gameObject.transform);
            playerGO.SetActive(false);
            Player player = playerGO.GetComponent<Player>();
            _totalBotPlayers.Add(player);
            player.Initialize(_totalBotPlayers.Count);
            playerGO.name = $"{_playerPrefab.name} ({_totalBotPlayers.Count})";
        }
    }

    private void StartGame()
    {
        ClearLists();
        EnablePlayersInGame();
        GiveAwayRoles();
        ChangeCurrentBehaviour(new NightHostBehaviour(this));
    }

    private void EnablePlayersInGame()
    {
        foreach(Player player in _totalBotPlayers)
            player.gameObject.SetActive(false);

        for(int i = 0; i < _currentRolesCountSO.RolesCount - 1; i++)
        {
            _totalBotPlayers[i].Restart();
            _totalBotPlayers[i].gameObject.SetActive(true);
            _playersInGame.Add(_totalBotPlayers[i]);
        }

        _mainPlayer.Restart();
        _playersInGame.Add(_mainPlayer);
    }

    private void ChangePlayersCount(RolesCountSO roomSO)
    {
        _currentRolesCountSO = roomSO;
        IsGameEnded = false;

        OnGameRestart.Invoke();

        //-1 because we also have mainPlayer (not a bot)
        if(_totalBotPlayers.Count < _currentRolesCountSO.RolesCount - 1)
        {
            int neededBotPlayersCount = (_currentRolesCountSO.RolesCount - 1) - _totalBotPlayers.Count;
            InstantiateBotPlayers(neededBotPlayersCount);
        }

        _mainPlayer.SetNumber(_currentRolesCountSO.RolesCount);

        StopAllCoroutines();
        StartGame();
    }

    private void RestartGame()
    {
        IsGameEnded = false;
        OnGameRestart.Invoke();
        StopAllCoroutines();
        StartGame();
    }

    private void ClearLists()
    {
        _playersInGame.Clear();
        _mafias.Clear();
        _townCivilians.Clear();
        _citizens.Clear();
        _prostitutes.Clear();
        _doctors.Clear();
        _sheriffs.Clear();
    }

    private void GiveAwayRoles()
    {
        List<Player> playersToGiveRole = new List<Player>(PlayersInGame);

        _rolesGiver.SetRolesCountSO(_currentRolesCountSO);

        _mafias = _rolesGiver.GiveAwayMafia(playersToGiveRole);
        _doctors = _rolesGiver.GiveAwayDoctors(playersToGiveRole);
        _sheriffs = _rolesGiver.GiveAwaySheriffs(playersToGiveRole);    
        _prostitutes = _rolesGiver.GiveAwayProstitutes(playersToGiveRole);
        _citizens = _rolesGiver.GiveAwayCitizens(playersToGiveRole);

        OnMainPlayerGotRole?.Invoke(_mainPlayer.Role);

        _townCivilians = _townCivilians.Concat(_doctors).Concat(_prostitutes).Concat(_citizens).Concat(_sheriffs).ToList();
    }

    public static Player GetRandomPlayerFromList(IReadOnlyList<Player> players)
    {
        int randomIndex = UnityEngine.Random.Range(0, players.Count);
        return players[randomIndex];
    }

    private void ChangeCurrentBehaviour(HostBehaviour behaviour)
    {
        CurrentBehaviour = behaviour;
        CurrentBehaviour.Enter();
    }

    private void GoToNextBehaviour(HostBehaviour endedBehaviour)
    {
        NightHostBehaviour nightBehaviour;

        switch(endedBehaviour)
        {
            case NightHostBehaviour:
                nightBehaviour = (NightHostBehaviour)endedBehaviour;
                DayHostBehaviour dayBehaviour = new DayHostBehaviour(nightBehaviour.AlibiPlayers, 
                                                                     nightBehaviour.KilledPlayer, this, _noOneDiedLines, _noOneVotedOutLines);
                ChangeCurrentBehaviour(dayBehaviour);
                break;

            case DayHostBehaviour:
                nightBehaviour = new NightHostBehaviour(this);
                ChangeCurrentBehaviour(nightBehaviour);
                break;
        }
    }

    private void RemovePlayerFromList(Player player)
    {
        switch(player.Role)
        {
            case CitizenRole:
                _citizens.Remove(player);
                _townCivilians.Remove(player);
                break;

            case MafiaRole:
                _mafias.Remove(player);
                break;

            case DoctorRole:
                _doctors.Remove(player);
                _townCivilians.Remove(player);
                break;

            case ProstituteRole:
                _prostitutes.Remove(player);
                _townCivilians.Remove(player);
                break;

            case SheriffRole:
                _sheriffs.Remove(player);
                _townCivilians.Remove(player);
                break;
        }

        _playersInGame.Remove(player);
        CheckForGameEnd();
    }

    private void CheckForGameEnd()
    {
        if(_mafias.Count >= _townCivilians.Count)
        {
            IsGameEnded = true;
            StartCoroutine(SayDialogueLineCoroutine("Mafia win."));
            EndGame();
        }
        else if(_mafias.Count == 0)
        {
            IsGameEnded = true;
            StartCoroutine(SayDialogueLineCoroutine("Civilians win."));
            EndGame();
        }
    }

    private void EndGame()
    {
        foreach(var player in _playersInGame)
            player.ShowRole();

        CurrentBehaviour = null;
    }

    private void OnDisable()
    {
        HostBehaviour.OnBehaviourEnded -= GoToNextBehaviour;
        Player.OnPlayerDied -= RemovePlayerFromList;
        RolesCountSO_Toggle.OnToggleActivated -= ChangePlayersCount;
        _restartButton.onClick.RemoveListener(RestartGame);
    }
}
