using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightHostBehaviour : HostBehaviour
{
    public List<Player> AlibiPlayers { get; private set; } = new List<Player>();
    public Player KilledPlayer { get; private set; }

    private List<Player> _healedPlayers = new List<Player>();
    private Player _mafiaVotedPlayer;
    private MonoBehaviour _hostMonoBehaviour;

    public NightHostBehaviour(DialogueEntity dialogueEntity) : base(dialogueEntity)
    {
        _hostMonoBehaviour = _hostDialogueEntity;
        Host.OnGameRestart += PrepareForBehaviourEnd;
    }

    public override void Enter()
    {
        MafiaLogic.OnMafiaPlayerVoted += AddVote;
        DoctorLogic.OnPlayerToHealVoted += HealPlayer;
        ProstituteLogic.OnPlayerToGiveAlibiVoted += GiveAlibi;
        _hostMonoBehaviour.StartCoroutine(AwakeRolesCoroutine());
    }

    private void PrepareForBehaviourEnd()
    {
        MafiaLogic.OnMafiaPlayerVoted -= AddVote;
        DoctorLogic.OnPlayerToHealVoted -= HealPlayer;
        ProstituteLogic.OnPlayerToGiveAlibiVoted -= GiveAlibi;
        Host.OnGameRestart -= PrepareForBehaviourEnd;
        ClearVotes();
    }

    private IEnumerator AwakeRolesCoroutine()
    {
        yield return _hostMonoBehaviour.StartCoroutine(_hostDialogueEntity.SayDialogueLineCoroutine("Night begins..."));
        yield return new WaitForSeconds(1f);

        yield return _hostMonoBehaviour.StartCoroutine(AwakeMafiaCoroutine());

        if(Host.Doctors.Count > 0)
            yield return _hostMonoBehaviour.StartCoroutine(AwakeDoctorsCoroutine());

        if(Host.Sheriffs.Count > 0)
            yield return _hostMonoBehaviour.StartCoroutine(AwakeSheriffsCoroutine());

        if(Host.Prostitutes.Count > 0)
            yield return _hostMonoBehaviour.StartCoroutine(AwakeProstitutesCoroutine());

        EndBehaviour();
    }

    private IEnumerator AwakeMafiaCoroutine()
    {
        yield return _hostMonoBehaviour.StartCoroutine(_hostDialogueEntity.SayDialogueLineCoroutine("Mafia wakes up..."));

        if(MainPlayer.Instance.Role is MafiaRole)
            EnablePlayersVoteUI(PlayersBackgroundVotesUIColors.NightColor);

        foreach(Player mafiaPlayer in Host.Mafia)
            yield return mafiaPlayer.StartCoroutine(mafiaPlayer.AwakeningCoroutine());

        DisablePlayersVoteUI();
        _mafiaVotedPlayer = GetMostVotedPlayer(_votedPlayers);
    }

    private IEnumerator AwakeDoctorsCoroutine()
    {
        yield return _hostMonoBehaviour.StartCoroutine(_hostDialogueEntity.SayDialogueLineCoroutine("Doctor wakes up..."));

        foreach(Player doctorPlayer in Host.Doctors)
            yield return doctorPlayer.StartCoroutine(doctorPlayer.AwakeningCoroutine());
    }

    private IEnumerator AwakeSheriffsCoroutine()
    {
        yield return _hostMonoBehaviour.StartCoroutine(_hostDialogueEntity.SayDialogueLineCoroutine("Sheriff wakes up..."));

        foreach(Player sheriffPlayer in Host.Sheriffs)
            yield return sheriffPlayer.StartCoroutine(sheriffPlayer.AwakeningCoroutine());
    }

    private IEnumerator AwakeProstitutesCoroutine()
    {
        yield return _hostMonoBehaviour.StartCoroutine(_hostDialogueEntity.SayDialogueLineCoroutine("Prostitute wakes up..."));

        foreach(Player prostitutePlayer in Host.Prostitutes)
            yield return prostitutePlayer.StartCoroutine(prostitutePlayer.AwakeningCoroutine());
    }

    private void SetKilledPlayer()
    {
        if(_healedPlayers.Contains(_mafiaVotedPlayer) == false)
            KilledPlayer = _mafiaVotedPlayer;
    }

    private void HealPlayer(Player playerToHeal) => _healedPlayers.Add(playerToHeal);

    private void GiveAlibi(Player playerToGiveAlibi) => AlibiPlayers.Add(playerToGiveAlibi);

    private void EndBehaviour()
    {
        PrepareForBehaviourEnd();
        SetKilledPlayer();
        OnBehaviourEnded.Invoke(this);
    }
}
