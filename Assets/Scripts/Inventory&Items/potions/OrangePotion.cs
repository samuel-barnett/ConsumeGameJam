using UnityEngine;

public class OrangePotion : Consumable
{
    [Header("Orange Potion")]
    [SerializeField] int damageIncrease;
    [SerializeField] float rangeIncrease;

    public override void ActivateEffect(Tank tank)
    {
        base.ActivateEffect(tank);

        Bullet bulletPrefabToChange = tank.GetBulletPrefab().GetComponent<Bullet>();
        bulletPrefabToChange.AddDamage(damageIncrease);
        bulletPrefabToChange.AddRange(rangeIncrease);
    }


    public override void DeactivateEffect(Tank tank)
    {
        Bullet bulletPrefabToChange = tank.GetBulletPrefab().GetComponent<Bullet>();
        bulletPrefabToChange.AddDamage(-damageIncrease);
        bulletPrefabToChange.AddRange(-rangeIncrease);
        base.DeactivateEffect(tank);
    }

}
