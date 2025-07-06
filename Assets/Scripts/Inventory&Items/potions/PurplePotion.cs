using UnityEngine;

public class PurplePotion : Consumable
{
    [Header("Purple Potion")]
    [SerializeField] float ok;

    public override void ActivateEffect(Tank tank)
    {
        base.ActivateEffect(tank);

        tank.SetGhostMode(1);
    }


    public override void DeactivateEffect(Tank tank)
    {
        tank.SetGhostMode(-1);

        base.DeactivateEffect(tank);
    }


}
