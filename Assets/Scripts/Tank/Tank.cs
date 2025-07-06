using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

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

    List<int> damageToMe = new List<int>();

    protected bool controlEnabled = true;

    // abilities
    bool canDrink;
    bool shielded;
    int explosiveLevel;
    float speedLevel = 1;
    int ghostMode;

    protected List<Consumable> consumables = new List<Consumable>();
    protected int currentConsumable = 0;

    [Header("Tank - Mesh Refs")]
    [SerializeField] protected Mesh headMouthClosed;
    [SerializeField] protected Mesh headMouthOpen;

    [Header("Tank - Refs")]
    [SerializeField] protected GameObject swivel;
    [SerializeField] protected GameObject barrel;
    [SerializeField] protected GameObject head;
    [SerializeField] protected GameObject mouthPour;

    [Header("Tank - Prefabs")]
    [SerializeField] GameObject bulletPrefab;


    [Header("Tank - Adjustable Variables")]
    [SerializeField] int maxHP;
    [SerializeField] float movementAcceleration;
    [SerializeField] float speedLimit;
    [SerializeField] float steeringSpeed;
    [SerializeField] int inventorySize = 4;
    [SerializeField] float drinkSpeed = 1;

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
        canDrink = true;

        if (this is PlayerController)
        {
            HPBar.sInstance.UpdateHealthDisplay(currentHP, maxHP);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage, int damageID)
    {
        if (damageToMe.Contains(damageID))
        {
            return;
        }
        else
        {
            damageToMe.Add(damageID);
        }

        if (shielded || ghostMode > 0)
        {
            return;
        }

        //Debug.Log("Taking " + damage + " damage");
        currentHP -= damage;
        TextPopUpSpawner.sInstance.DamagePopUp(damage, transform.position + (Vector3.up * 4));

        if (this is PlayerController)
        {
            HPBar.sInstance.UpdateHealthDisplay(currentHP, maxHP);
        }

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        ParticleManager.sInstance.SpawnParticleAtPosition(ParticleType.TANK_DEATH, transform.position);

        if (this is EnemyTankBehavior)
        {
            // drop items
            GameObject item = ItemsManager.sInstance.SpawnRandomItem();
            item.transform.position = transform.position;
            WinTracker.sInstance.RemoveTankFromTracker(this as EnemyTankBehavior);
            Destroy(gameObject);
        }
        else if (this is PlayerController)
        {
            // go to gameover
            MenuButtons.sInstance.gameObject.SetActive(true);
            controlEnabled = false;
            head.gameObject.SetActive(false);
            ParticleManager.sInstance.SpawnParticleAtPosition(ParticleType.FIRE, head.transform.position);
        }

        
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
        Vector3 moveTo = transform.position + (movementAcceleration * speedLevel * Time.fixedDeltaTime * (transform.forward * direction).normalized);
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
            obj.GetComponent<Rigidbody>().AddForce(barrel.transform.forward * projectileForce);
            Bullet b = obj.GetComponent<Bullet>();
            b.SetTeam(team);
            if (explosiveLevel > 0)
            {
                b.AddDamage(explosiveLevel * 3);
                b.AddRange(explosiveLevel * 2);
            }
            GameObject shootParticle = ParticleManager.sInstance.SpawnParticleAtPosition(ParticleType.SHOOT, barrel.transform.position + (barrel.transform.forward * 4));
            shootParticle.transform.rotation = barrel.transform.rotation;

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
        if (currentConsumable < consumables.Count && consumables[currentConsumable] && !consumables[currentConsumable].GetActivated() && canDrink)
        {
            Consumable current = consumables[currentConsumable];
            MeshRenderer consumableMR = current.gameObject.GetComponent<MeshRenderer>();
            StartCoroutine(OpenMouth(consumableMR));

            current.ActivateEffect(this);
            current.transform.SetParent(mouthPour.transform);
            current.transform.localPosition = Vector3.zero;
            current.transform.localRotation = Quaternion.identity;
            current.transform.Rotate(new Vector3(30, 180, -90));

            consumables.RemoveAt(currentConsumable);
            currentConsumable--;
            if (currentConsumable < 0)
            {
                currentConsumable = 0;
            }
            return true;
        }


        return false;
    }

    IEnumerator OpenMouth(MeshRenderer consumableMR)
    {
        MeshFilter mf = head.GetComponent<MeshFilter>();

        mf.mesh = headMouthOpen;
        consumableMR.enabled = true;
        canDrink = false;
        yield return new WaitForSeconds(drinkSpeed);
        mf.mesh = headMouthClosed;
        consumableMR.enabled = false;
        canDrink = true;
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

    public void SetControlEnabled(bool newControl)
    {
        controlEnabled = newControl;
    }

    public bool GetControlEnabled()
    {
        return controlEnabled;
    }

    // abilities
    public void SetShielded(bool newShielded)
    {
        shielded = newShielded;
    }
    public void SetGhostMode(int ghostModeAdditive)
    {
        ghostMode += ghostModeAdditive;
        if (ghostMode > 0)
        {
            MeshRenderer tankMR = GetComponent<MeshRenderer>();
            MeshRenderer turretMR = swivel.GetComponent<MeshRenderer>();
            MeshRenderer barrelMR = barrel.GetComponent<MeshRenderer>();
            MeshRenderer headMR = head.GetComponent<MeshRenderer>();

            tankMR.material = ItemsManager.sInstance.GetGhostMaterial();
            turretMR.material = ItemsManager.sInstance.GetGhostMaterial();
            barrelMR.material = ItemsManager.sInstance.GetGhostMaterial();
            headMR.material = ItemsManager.sInstance.GetGhostMaterial();

            BoxCollider bc = GetComponent<BoxCollider>();
            SphereCollider sc = GetComponent<SphereCollider>();
            bc.isTrigger = true;
            sc.isTrigger = true;

            rb.useGravity = false;
        }
        else
        {
            MeshRenderer tankMR = GetComponent<MeshRenderer>();
            MeshRenderer turretMR = swivel.GetComponent<MeshRenderer>();
            MeshRenderer barrelMR = barrel.GetComponent<MeshRenderer>();
            MeshRenderer headMR = head.GetComponent<MeshRenderer>();

            tankMR.material = ItemsManager.sInstance.GetNormalMaterial();
            turretMR.material = ItemsManager.sInstance.GetNormalMaterial();
            barrelMR.material = ItemsManager.sInstance.GetNormalMaterial();
            headMR.material = ItemsManager.sInstance.GetNormalMaterial();

            BoxCollider bc = GetComponent<BoxCollider>();
            SphereCollider sc = GetComponent<SphereCollider>();
            bc.isTrigger = false;
            sc.isTrigger = false;

            //rb.useGravity = true;
        }
    }

    public void AddExplosiveRounds(int explosiveLevelAdditive)
    {
        explosiveLevel += explosiveLevelAdditive;
    }

    public void AddSpeedLevel(float speedLevelAdditive)
    {
        speedLevel += speedLevelAdditive;
    }


}
