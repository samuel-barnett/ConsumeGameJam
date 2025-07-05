using UnityEngine;

public class GreenPotion : Consumable
{
    [Header("Green Potion")]
    [SerializeField] float ok;

    public override void ActivateEffect(Tank tank)
    {
        base.ActivateEffect(tank);


    }


    public override void DeactivateEffect(Tank tank)
    {

        base.DeactivateEffect(tank);
    }

}
