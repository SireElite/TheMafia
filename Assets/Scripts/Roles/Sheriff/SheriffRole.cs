using UnityEngine;

public class SheriffRole : PlayerRole
{
    public override string Name => "Sheriff";

    public override Sprite RoleSprite => Resources.Load<Sprite>(Name);

    public SheriffRole(Player player) : base(player)
    {
        if(player is MainPlayer mainPlayer)
        {
            _roleBehaviour = new SheriffMainPlayerRoleBehaviour(mainPlayer);
        }
        else
        {
            _roleBehaviour = new SheriffBotPlayerRoleBehaviour(player);
        }
    }
}
