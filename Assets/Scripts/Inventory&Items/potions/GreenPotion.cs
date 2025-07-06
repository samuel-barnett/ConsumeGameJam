using UnityEngine;

public class GreenPotion : Consumable
{
    [Header("Green Potion")]
    [SerializeField] float speedIncrease;

    public override void ActivateEffect(Tank tank)
    {
        base.ActivateEffect(tank);

        tank.AddSpeedLevel(speedIncrease);
    }


    public override void DeactivateEffect(Tank tank)
    {
        tank.AddSpeedLevel(-speedIncrease);

        base.DeactivateEffect(tank);
    }

}
