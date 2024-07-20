using UnityEngine;

public class MafiaRole : PlayerRole
{
    public override string Name => "Mafia";

    public override Sprite RoleSprite => Resources.Load<Sprite>(Name);

    public MafiaRole(Player player) : base(player)
    {
        if(player is MainPlayer mainPlayer)
        {
            _roleBehaviour = new MafiaMainPlayerRoleBehaviour(mainPlayer);
        }
        else
        {
            _roleBehaviour = new MafiaBotPlayerRoleBehaviour(player);
        }

        Host.OnMainPlayerGotRole += ShowRoleIfMainPlayerMafia;
    }

    private void ShowRoleIfMainPlayerMafia(PlayerRole role)
    {
        if(role is MafiaRole)
            _player.ShowRole();

        Host.OnMainPlayerGotRole -= ShowRoleIfMainPlayerMafia;
    }
}
