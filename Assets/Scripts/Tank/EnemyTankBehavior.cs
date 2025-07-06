using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

public enum EnemyMoveState
{
    STANDBY,
    DECIDE_DESTINATION,
    MOVING,
    WAIT,
    DRINK_POTION
}


public class EnemyTankBehavior : Tank
{
    Vector3 destination;

    EnemyMoveState currentState = EnemyMoveState.DECIDE_DESTINATION;

    [Header("EnemyTankBehavior - Adjustable Variables")]
    [SerializeField] float randomWaitMinTime;
    [SerializeField] float randomWaitMaxTime;

    [SerializeField] float randomDestinationMinDistance;
    [SerializeField] float randomDestinationMaxDistance;


    protected override void SetupEntity()
    {
        base.SetupEntity();

        team = Team.ENEMY;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (currentState)
        {
            case EnemyMoveState.STANDBY:
                // do nothing lol
                break;
            case EnemyMoveState.DECIDE_DESTINATION:
                DecideNewDestination();
                break;
            case EnemyMoveState.MOVING:
                bool turnComplete = Turn();
                if (turnComplete)
                {
                    GoToDestination();
                }
                CheckDestinationReached();
                break;
            case EnemyMoveState.WAIT:
                float timeToWait = Random.Range(randomWaitMinTime, randomWaitMaxTime);
                StartCoroutine(WaitInPlace(timeToWait));
                break;
            case EnemyMoveState.DRINK_POTION:
                TryUseConsumable();
                currentState = EnemyMoveState.WAIT;
                break;
            default:
                Debug.LogError("case not accounted for");
                break;
        }

        // aiming and shooting
        LookToPlayer();
        AddTimeSinceLastShot();
        FireProjectile();
    }

    void DecideNewDestination()
    {
        int attempts = 15;
        while (attempts > 0)
        {
            float distance = Random.Range(randomDestinationMinDistance, randomDestinationMaxDistance);
            Vector3 newDestination = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized * distance;
            newDestination += transform.position;

            RaycastHit hit;
            bool objectHit = Physics.Raycast(transform.position, newDestination - transform.position, out hit, (newDestination - transform.position).magnitude);

            if (!objectHit)
            {
                destination = newDestination;
                currentState = EnemyMoveState.MOVING;
                //GameObject obj = Instantiate(prefabToSpawn);
                //obj.transform.position = destination;
                //Debug.Log(destination);
                return;
            }
            attempts--;
        }
    }

    bool Turn()
    {
        Vector3 relative = transform.InverseTransformPoint(destination);
        float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
        if (angle < 1 && angle > -1)
        {
            // turn complete
            //Debug.Log("Turn Finished");
            return true;
        }

        if (angle > 0)
        {
            Steer(1);
        }
        else
        {
            Steer(-1);
        }
        
        return false;
    }

    void GoToDestination()
    {
        Drive(1);
    }

    void CheckDestinationReached()
    {
        // check if we have reached our destination :)
        Vector3 here = transform.position;
        here.y = 0;
        Vector3 there = destination;
        there.y = 0;

        if ((there - here).magnitude <= 0.5)
        {
            Debug.Log("reached destination");
            rb.linearVelocity = Vector3.zero;
            currentState = EnemyMoveState.DRINK_POTION;
        }
    }

    IEnumerator WaitInPlace(float timeToWait)
    {
        currentState = EnemyMoveState.STANDBY;
        yield return new WaitForSeconds(timeToWait);
        currentState = EnemyMoveState.DECIDE_DESTINATION;
    }

    void LookToPlayer()
    {
        SetSwivelRotation(PlayerController.sInstance.transform.position);
    }

    void FireProjectile()
    {
        RaycastHit hit;
        bool objectHit = Physics.Raycast(transform.position, PlayerController.sInstance.transform.position - transform.position, out hit, 1000);
        if (objectHit && hit.collider.gameObject.GetComponent<PlayerController>() || hit.collider.gameObject.GetComponent<Shield>())
        {
            TryShootProjectile();
        }
    }


}
