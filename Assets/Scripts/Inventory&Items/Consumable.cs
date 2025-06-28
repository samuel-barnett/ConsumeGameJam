using System.Collections;
using UnityEngine;

public class Consumable : MonoBehaviour
{
    float timeElapsed = 0;

    bool activated = false;

    [SerializeField] float duration;

    MeshRenderer mr;
    SphereCollider sc;

    void Start()
    {
        mr = GetComponent<MeshRenderer>();
        sc = GetComponent<SphereCollider>();

    }

    // Update is called once per frame
    void Update()
    {



    }

    private void OnCollisionEnter(Collision collision)
    {
        Tank tank = collision.gameObject.GetComponent<Tank>();
        if (tank != null)
        {
            bool giveSuccess = tank.TryGiveConsumable(this);

            if (giveSuccess)
            {
                mr.enabled = false;
                sc.enabled = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Tank tank = other.gameObject.GetComponent<Tank>();
        if (tank != null)
        {
            bool giveSuccess = tank.TryGiveConsumable(this);

            if (giveSuccess)
            {
                mr.enabled = false;
                sc.enabled = false;
            }
        }
    }

    public virtual void ActivateEffect()
    {
        Debug.Log("Effect Begin");
        activated = true;
        StartCoroutine(SetTimerForDuration());




    }

    IEnumerator SetTimerForDuration()
    {

        while (timeElapsed < duration)
        {
            timeElapsed += Time.fixedDeltaTime;

            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        
        DeactivateEffect();
    }


    public virtual void DeactivateEffect()
    {
        Debug.Log("Effect Over");



        Destroy(gameObject);
    }

    public bool GetActivated()
    {
        return activated;
    }

}
