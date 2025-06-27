using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

public enum EnemyMoveState
{
    STANDBY,
    DECIDE_DESTINATION,
    MOVING,
    WAIT
}


public class EnemyTankBehavior : Tank
{
    Vector3 destination;

    EnemyMoveState currentState = EnemyMoveState.DECIDE_DESTINATION;

    float timeToWait;

    public GameObject prefabToSpawn;


    // Update is called once per frame
    void Update()
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
                break;
            case EnemyMoveState.WAIT:
                StartCoroutine(WaitInPlace(1f));
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
            Vector3 newDestination = new Vector3(transform.position.x + Random.Range(-3f, 3f), 0, transform.position.z + Random.Range(-3f, 3f)).normalized;
            newDestination.y = transform.position.y;

            RaycastHit hit;
            bool objectHit = Physics.Raycast(transform.position, newDestination - transform.position, out hit, (newDestination - transform.position).magnitude);

            if (!objectHit)
            {
                destination = newDestination;
                currentState = EnemyMoveState.MOVING;
                GameObject obj = Instantiate(prefabToSpawn);
                obj.transform.position = destination;
                Debug.Log(destination);
                return;
            }

            attempts--;
        }
    }

    bool Turn()
    {
        Vector3 relative = transform.InverseTransformPoint(destination);
        float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
        DebugUI.sInstance.SetDebugText("angle:" + angle);
        if (angle < 1 && angle > -1)
        {
            // turn complete
            //Debug.Log("Turn Finished");
            return true;
        }
        else
        {
            Debug.Log(angle);
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

        // check if we have reached our destination :)
        if ((transform.position - destination).magnitude <= 0.05)
        {
            rb.linearVelocity = Vector3.zero;
            currentState = EnemyMoveState.WAIT;
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
        if (objectHit && hit.collider.gameObject.GetComponent<PlayerController>())
        {
            TryShootProjectile();
        }
    }


}
