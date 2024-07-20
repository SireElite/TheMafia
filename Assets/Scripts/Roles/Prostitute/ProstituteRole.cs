using UnityEngine;

public class ProstituteRole : PlayerRole
{
    public override string Name => "Prostitute";

    public override Sprite RoleSprite => Resources.Load<Sprite>(Name);

    public ProstituteRole(Player player) : base(player)
    {
        if(player is MainPlayer mainPlayer)
        {
            _roleBehaviour = new ProstituteMainPlayerRoleBehaviour(mainPlayer);
        }
        else
        {
            _roleBehaviour = new ProstituteBotPlayerRoleBehaviour(player);
        }
    }
}
