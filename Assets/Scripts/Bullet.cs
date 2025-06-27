using UnityEngine;


[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    Rigidbody rb;

    Team bulletTeam;

    [SerializeField] int damage;

    [SerializeField] float gravityModifier;

    



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
        Tank tank = collision.gameObject.GetComponent<Tank>();
        if (tank && tank.GetTeam() != bulletTeam)
        {
            tank.TakeDamage(damage);
        }

        Destroy(gameObject);
    }


}
