using System.Collections;
using UnityEngine;

public enum ParticleType
{
    SHOOT,
    EXPLODE,
    BIG_EXPLODE,
    ARTILLERY,
    BIG_ARTILLERY
}


public class ParticleManager : MonoBehaviour
{
    public static ParticleManager sInstance { get; private set; }

    [Header("Tank Particles")]
    [SerializeField] GameObject shootParticle;
    [SerializeField] GameObject explosionParticle;
    [SerializeField] GameObject bixExplosionParticle;
    [SerializeField] GameObject artilleryParticle;
    [SerializeField] GameObject bigArtilleryParticle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (sInstance != null && sInstance != this)
        {
            Destroy(this);
        }
        else
        {
            sInstance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject SpawnParticleAtPosition(ParticleType type, Vector3 position)
    {
        GameObject prefab = null;

        switch (type)
        {
            case ParticleType.SHOOT:
                prefab = shootParticle;
                break;
            case ParticleType.EXPLODE:
                prefab = explosionParticle;
                break;
            case ParticleType.BIG_EXPLODE:
                prefab = bixExplosionParticle;
                break;
            case ParticleType.ARTILLERY:
                prefab = artilleryParticle;
                break;
            case ParticleType.BIG_ARTILLERY:
                prefab = bigArtilleryParticle;
                break;
            default:
                return null;
        }

        GameObject obj = null;
        if (prefab)
        {
            obj = Instantiate(prefab);
            StartCoroutine(DeleteWhenDone(obj));
            obj.transform.position = position;
        }
        return obj;
    }

    IEnumerator DeleteWhenDone(GameObject particleObject)
    {
        ParticleSystem system = particleObject.GetComponent<ParticleSystem>();

        yield return new WaitForSeconds(system.main.duration);
        Destroy(particleObject);
    }


}
