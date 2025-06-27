using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Tank : MonoBehaviour
{
    protected Rigidbody rb;


    int currentHP;

    [Header("Tank - Refs")]
    [SerializeField] protected GameObject swivel;
    [SerializeField] protected GameObject barrel;
    [SerializeField] protected GameObject head;


    [Header("Tank - Prefabs")]
    [SerializeField] GameObject bulletPrefab;


    [Header("Tank - Adjustable Variables")]
    [SerializeField] int maxHP;
    [SerializeField] float projectileForce;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentHP = maxHP;

        SetupEntity();
    }

    protected virtual void SetupEntity()
    {

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









    protected void SetSwivelRotation(Vector3 positionToLookAt)
    {
        Vector3 lookTo = positionToLookAt;
        lookTo.y = swivel.transform.position.y;
        swivel.transform.LookAt(lookTo);

    }

    protected void ShootProjectile()
    {
        GameObject obj = Instantiate(bulletPrefab);
        obj.transform.position = barrel.transform.position;
        obj.GetComponent<Rigidbody>().AddForce(barrel.transform.up * projectileForce);

    }

    public Vector3 GetBarrelPosition()
    {
        return barrel.transform.position;
    }

}
