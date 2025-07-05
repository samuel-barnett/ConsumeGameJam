using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] int maxShieldHealth;
    int currentSheildHealth;

    Team shieldTeam;

    Tank tankImShielding;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentSheildHealth = maxShieldHealth;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet") || other.gameObject.CompareTag("ArtilleryBullet"))
        {
            Bullet bullet = other.GetComponent<Bullet>();
            if (bullet && bullet.GetTeam() != shieldTeam)
            {
                TakeDamage(bullet.GetDamage());
                TextPopUpSpawner.sInstance.ShieldDamagePopUp(bullet.GetDamage(), other.transform.position);

                Destroy(other.gameObject);
            }
        }
    }
    

    public void TakeDamage(int damage)
    {
        currentSheildHealth -= damage;
        if (currentSheildHealth < 0)
        {
            RemoveShield();
        }
    }
    
    void RemoveShield()
    {
        SphereCollider sc = GetComponent<SphereCollider>();
        MeshRenderer mr = GetComponent<MeshRenderer>();
        if (sc)
        {
            sc.enabled = false;
        }
        if (mr)
        {
            mr.enabled = false;
        }
        tankImShielding.SetShielded(false);
    }

    public void SetTank(Tank newTank)
    {
        tankImShielding = newTank;
    }

    public void SetTeam(Team newTeam)
    {
        shieldTeam = newTeam;
    }

    public Team GetTeam()
    {
        return shieldTeam;
    }

}
