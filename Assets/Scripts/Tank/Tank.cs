using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Tank : MonoBehaviour
{
    protected Rigidbody rb;


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
        currentHP -= damage;
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
            //Debug.Log("Speed Limit");
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
            timeSinceLastFire = 0;
        }
    }

    public Vector3 GetVelocity()
    {
        return rb.linearVelocity;
    }

    public float GetTimeSince()
    {
        return timeSinceLastFire;
    }

}
