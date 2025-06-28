using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public enum Team
{
    PLAYER,
    ENEMY
}


[RequireComponent(typeof(Rigidbody))]
public class Tank : MonoBehaviour
{
    protected Rigidbody rb;

    protected Team team;

    int currentHP;
    float timeSinceLastFire;

    public List<Consumable> consumables = new List<Consumable>();
    int currentConsumable = 0;

    [Header("Tank - Refs")]
    [SerializeField] protected GameObject swivel;
    [SerializeField] protected GameObject barrel;
    [SerializeField] protected GameObject head;

    [Header("Tank - Prefabs")]
    [SerializeField] GameObject bulletPrefab;


    [Header("Tank - Adjustable Variables")]
    [SerializeField] int maxHP;
    [SerializeField] float movementAcceleration;
    [SerializeField] float speedLimit;
    [SerializeField] float steeringSpeed;
    [SerializeField] int inventorySize = 4;


    [SerializeField] float fireRate = 1;
    [SerializeField] float projectileForce;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetupEntity();
    }

    protected virtual void SetupEntity()
    {
        rb = GetComponent<Rigidbody>();
        currentHP = maxHP;
        timeSinceLastFire = fireRate;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        //Debug.Log("Taking " + damage + " damage");
        currentHP -= damage;
        TextPopUpSpawner.sInstance.DamagePopUp(damage, transform.position + (Vector3.up * 4));
        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    public void HealDamage(int healAmount)
    {
        currentHP += healAmount;
        TextPopUpSpawner.sInstance.HealPopUp(healAmount, transform.position + (Vector3.up * 4));
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }

    protected void Drive(float direction)
    {
        Vector3 moveTo = transform.position + (movementAcceleration * Time.fixedDeltaTime * (transform.forward * direction).normalized);
        rb.MovePosition(moveTo);
        //rb.AddForce((transform.forward * direction).normalized * movementAcceleration);
        EnforceSpeedLimit();
    }

    protected void Steer(float direction)
    {
        Quaternion rotateTo = transform.rotation * Quaternion.Euler(direction * steeringSpeed * Time.fixedDeltaTime * transform.up);
        rb.MoveRotation(rotateTo);
        //transform.Rotate(transform.up * direction * steeringSpeed);
    }

    protected void EnforceSpeedLimit()
    {
        if (rb.linearVelocity.magnitude > speedLimit)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * speedLimit;
        }
    }

    protected void SetSwivelRotation(Vector3 positionToLookAt)
    {
        Vector3 lookTo = positionToLookAt;
        lookTo.y = swivel.transform.position.y;
        swivel.transform.LookAt(lookTo);
    }

    protected void AddTimeSinceLastShot()
    {
        timeSinceLastFire += Time.fixedDeltaTime;
    }


    protected void TryShootProjectile()
    {
        if (timeSinceLastFire >= fireRate)
        {
            GameObject obj = Instantiate(bulletPrefab);
            obj.transform.position = barrel.transform.position;
            obj.GetComponent<Rigidbody>().AddForce(barrel.transform.up * projectileForce);
            Bullet b = obj.GetComponent<Bullet>();
            b.SetTeam(team);
            timeSinceLastFire = 0;
        }
    }

    public bool TryGiveConsumable(Consumable consumable)
    {
        if (InventoryFull())
        {
            return false;
        }

        consumables.Add(consumable);


        return true;
    }

    bool InventoryFull()
    {
        if (consumables.Count >= inventorySize)
        {
            return true;
        }
        return false;
    }

    public void CycleInventory(int amount)
    {
        if (amount > 0)
        {
            currentConsumable = (currentConsumable >= consumables.Count - 1) ? 0 : currentConsumable += 1;
        }
        else
        {
            currentConsumable = (currentConsumable <= 0) ? consumables.Count - 1 : currentConsumable -= 1;
        }
    }

    public bool TryUseConsumable()
    {
        if (currentConsumable < consumables.Count && consumables[currentConsumable] && !consumables[currentConsumable].GetActivated())
        {
            consumables[currentConsumable].ActivateEffect();
            consumables.RemoveAt(0);
            currentConsumable--;
            if (currentConsumable < 0)
            {
                currentConsumable = 0;
            }
            return true;
        }


        return false;
    }


    public Team GetTeam()
    {
        return team;
    }

    public GameObject GetBarrel()
    {
        return barrel;
    }

    public float GetTimeSince()
    {
        return timeSinceLastFire;
    }

    public GameObject GetItemAtIndex(int index)
    {
        if (index < consumables.Count)
        {
            return consumables[index].gameObject;
        }
        return null;
    }

    public int GetCurrentItem()
    {
        return currentConsumable;
    }

    public int GetItemsHeldCount()
    {
        return consumables.Count;
    }

    public int GetInventorySize()
    {
        return inventorySize;
    }


}
