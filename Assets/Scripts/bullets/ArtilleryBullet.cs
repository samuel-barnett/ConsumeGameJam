using UnityEngine;

public class ArtilleryBullet : Bullet
{
    [Header("Artillery Bullet")]
    [SerializeField] float minTargetRadius;
    [SerializeField] float maxTargetRadius;

    [SerializeField] float elevationToStart;
    bool passedThreshold = false;

    [SerializeField] GameObject targetPrefab;

    [SerializeField] float timeToDescend;
    float timePassed;

    // Update is called once per frame
    void Update()
    {
        if (!passedThreshold && transform.position.y >= elevationToStart)
        {
            passedThreshold = true;
            GoDown();
        }
        else if (passedThreshold)
        {
            timePassed += Time.deltaTime;
            if (timePassed >= timeToDescend)
            {
                rb.isKinematic = false;
            }
            else
            {

            }
        }
    }


    void GoDown()
    {
        Vector3 targetPosition = DecideTarget();

        GameObject target = Instantiate(targetPrefab);
        target.transform.position = targetPosition;

        Target targetScript = target.GetComponent<Target>();
        if (targetScript != null )
        {
            targetScript.SetRef(this.gameObject);
        }

        targetPosition.y = elevationToStart;
        transform.position = targetPosition;
        rb.isKinematic = true;
    }

    Vector3 DecideTarget()
    {
        Vector3 result = PlayerController.sInstance.transform.position;
        Vector3 randVec = new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)).normalized * (Random.Range(minTargetRadius, maxTargetRadius));
        result += randVec;

        return result;
    }

}
