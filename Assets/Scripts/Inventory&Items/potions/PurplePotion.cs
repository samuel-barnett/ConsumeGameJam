using UnityEngine;

public class PurplePotion : Consumable
{
    [Header("Purple Potion")]
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
