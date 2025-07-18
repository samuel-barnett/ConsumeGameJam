using UnityEngine;

public class Target : MonoBehaviour
{
    GameObject bulletRef;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (bulletRef == null)
        {
            ParticleManager.sInstance.SpawnParticleAtPosition(ParticleType.ARTILLERY, transform.position);
            Destroy(gameObject);
        }
    }


    public void SetRef(GameObject bullet)
    {
        bulletRef = bullet;
    }


}
