using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenRole : PlayerRole
{
    public override string Name => "Citizen";

    public override Sprite RoleSprite => Resources.Load<Sprite>(Name);

    public CitizenRole(Player player) : base(player)
    {
        if(player is MainPlayer mainPlayer)
        {
            _roleBehaviour = new CitizenMainPlayerRoleBehaviour(mainPlayer);
        }
        else
        {
            _roleBehaviour = new CitizenBotPlayerRoleBehaviour();
        }
    }
}
