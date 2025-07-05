using UnityEngine;


[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    protected Rigidbody rb;

    Team bulletTeam;
    int bulletID;


    [Header("Bullet")]
    [SerializeField] int damage;

    [SerializeField] float explosionRadius;

    [SerializeField] float gravityModifier;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        bulletID = Random.Range(int.MinValue, int.MaxValue);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rb.AddForce(transform.up * gravityModifier);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //GameObject obj = Instantiate(explosionPrefab);
        //Explosion explosion = obj.GetComponent<Explosion>();
        //explosion.Setup(explosionRadius, damage, bulletTeam);
        //explosion.transform.position = transform.position;

        //bool damaage = false;

        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in hits)
        {
            Tank tank = hit.gameObject.GetComponent<Tank>();
            if (tank && tank.GetTeam() != bulletTeam)
            {
                //Debug.DrawLine(transform.position, hit.transform.position, Color.red);
                tank.TakeDamage(damage, bulletID);
                //damaage = true;
            }
        }
        /*
        if (damaage)
        {
            Debug.Break();
        }
        */

        /*
        Shield shield = collision.gameObject.GetComponent<Shield>();
        if (shield && shield.GetTeam() != bulletTeam)
        {
            shield.TakeDamage(damage);
        }
        */

        Destroy(gameObject);
    }


    public void SetTeam(Team newTeam)
    {
        bulletTeam = newTeam;
    }

    public Team GetTeam()
    {
        return bulletTeam;
    }

    public int GetDamage()
    {
        return damage;
    }

    public void AddDamage(int addedDamage)
    {
        damage += addedDamage;
    }
    public void AddRange(float addedRange)
    {
        explosionRadius += addedRange;
    }




}
