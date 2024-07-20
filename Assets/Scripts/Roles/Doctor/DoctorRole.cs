using UnityEngine;

public class DoctorRole : PlayerRole
{
    public override string Name => "Doctor";

    public override Sprite RoleSprite => Resources.Load<Sprite>(Name);

    public DoctorRole(Player player) : base(player) 
    { 
        if(player is MainPlayer mainPlayer)
        {
            _roleBehaviour = new DoctorMainPlayerRoleBehaviour(mainPlayer); 
        }
        else
        {
            _roleBehaviour = new DoctorBotPlayerRoleBehaviour(player);
        }
    }
}
