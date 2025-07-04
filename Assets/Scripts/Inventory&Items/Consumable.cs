using System.Collections;
using UnityEngine;

public class Consumable : MonoBehaviour
{
    float timeAlive;
    bool bobAndRotate = true;


    float timeElapsed = 0;

    bool activated = false;

    [SerializeField] float rotationSpeed = 10;
    [SerializeField] AnimationCurve bobCurve;
    [SerializeField] float bobMagnitude = 1;

    [SerializeField] float duration;

    MeshRenderer mr;
    SphereCollider sc;

    void Start()
    {
        mr = GetComponent<MeshRenderer>();
        sc = GetComponent<SphereCollider>();

        // randomize rotation offset
        transform.Rotate(new Vector3(0,Random.Range(0,360),0), Space.World);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (bobAndRotate)
        {
            timeAlive += Time.fixedDeltaTime;
            BobAndRotateItem();
        }
        


    }


    void BobAndRotateItem()
    {
        transform.Rotate(new Vector3(0, rotationSpeed * Time.fixedDeltaTime, 0), Space.World);
        transform.position += new Vector3(0, bobCurve.Evaluate(timeAlive) * bobMagnitude, 0);
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
                bobAndRotate = false;
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
                bobAndRotate = false;
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
