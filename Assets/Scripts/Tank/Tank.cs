using UnityEngine;

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
        rb.AddForce((transform.forward * direction).normalized * movementAcceleration * Time.deltaTime);
        EnforceSpeedLimit();
    }

    protected void Steer(float direction)
    {
        transform.Rotate(transform.up * direction * steeringSpeed * Time.deltaTime);
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
        timeSinceLastFire += Time.deltaTime;
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

}
