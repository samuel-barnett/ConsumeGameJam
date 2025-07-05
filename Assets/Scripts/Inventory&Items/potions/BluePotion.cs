using UnityEngine;

public class BluePotion : Consumable
{
    [Header("Blue Potion")]
    [SerializeField] GameObject shieldPrefab;

    GameObject shieldInstance;

    public override void ActivateEffect(Tank tank)
    {
        base.ActivateEffect(tank);

        shieldInstance = Instantiate(shieldPrefab);
        Shield shield = shieldInstance.GetComponent<Shield>();
        shield.SetTank(tank);
        shieldInstance.transform.SetParent(PlayerController.sInstance.transform);
        shieldInstance.transform.localPosition = Vector3.zero;
        tank.SetShielded(true);
    }


    public override void DeactivateEffect(Tank tank)
    {
        Destroy(shieldInstance);
        tank.SetShielded(false);

        base.DeactivateEffect(tank);
    }



}
