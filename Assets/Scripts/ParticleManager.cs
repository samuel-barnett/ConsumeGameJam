using System.Collections;
using UnityEngine;

public enum ParticleType
{
    SHOOT,
    EXPLODE,
    BIG_EXPLODE,
    ARTILLERY,
    BIG_ARTILLERY,
    TANK_DEATH,
    FIRE
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
    [SerializeField] GameObject tankDeathParticle;
    [SerializeField] GameObject fireParticle;

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
        bool destroyParticleWhenDone = false;

        switch (type)
        {
            case ParticleType.SHOOT:
                prefab = shootParticle;
                destroyParticleWhenDone = true;
                break;
            case ParticleType.EXPLODE:
                prefab = explosionParticle;
                destroyParticleWhenDone = true;
                break;
            case ParticleType.BIG_EXPLODE:
                prefab = bixExplosionParticle;
                destroyParticleWhenDone = true;
                break;
            case ParticleType.ARTILLERY:
                prefab = artilleryParticle;
                destroyParticleWhenDone = true;
                break;
            case ParticleType.BIG_ARTILLERY:
                prefab = bigArtilleryParticle;
                destroyParticleWhenDone = true;
                break;
            case ParticleType.TANK_DEATH:
                prefab = tankDeathParticle;
                destroyParticleWhenDone = true;
                break;
            case ParticleType.FIRE:
                prefab = fireParticle;
                destroyParticleWhenDone = false;
                break;
            default:
                return null;
        }

        GameObject obj = null;
        if (prefab)
        {
            obj = Instantiate(prefab);
            if (destroyParticleWhenDone)
            {
                StartCoroutine(DeleteWhenDone(obj));
            }
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
