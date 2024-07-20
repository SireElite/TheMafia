using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public abstract class PlayerRole
{
    public abstract Sprite RoleSprite { get; }
    public abstract string Name { get; }

    protected Player _player;
    protected RoleBehaviour _roleBehaviour;

    public PlayerRole(Player player) => _player = player;

    public IEnumerator MakeDayVote(List<Player> players, Action<Player> onPlayerVoted)
    {
        yield return _player.StartCoroutine(_roleBehaviour.MakeDayVoteCoroutine(players, onPlayerVoted));
    }

    public IEnumerator MakeNightVote()
    {
        yield return _player.StartCoroutine(_roleBehaviour.MakeNightVoteCoroutine());
    }
}
