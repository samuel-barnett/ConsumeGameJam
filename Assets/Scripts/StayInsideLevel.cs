using UnityEngine;
using static UnityEngine.InputSystem.Controls.AxisControl;

[RequireComponent(typeof(BoxCollider))]
public class StayInsideLevel : MonoBehaviour
{
    BoxCollider bc;

    Tank[] allTanks;

    float width, depth, height;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bc = GetComponent<BoxCollider>();
        width = bc.size.x / 2;
        height = bc.size.y / 2;
        depth = bc.size.z / 2;


        allTanks = GameObject.FindObjectsByType<Tank>(FindObjectsSortMode.None);
    }

    private void Update()
    {
        StayInside();
    }
    void FixedUpdate()
    {
        StayInside();
    }

    void StayInside()
    {
        Vector3 current;
        foreach (Tank tank in allTanks)
        {
            if (tank == null)
            {
                continue;
            }

            current = tank.transform.position;
            Vector3 clamp = current;

            // x
            if (current.x > transform.position.x + width)
            {
                clamp.x = transform.position.x + width;
            }
            else if (current.x < transform.position.x - width)
            {
                clamp.x = transform.position.x - width;
            }

            // z
            if (current.z > transform.position.z + depth)
            {
                clamp.z = transform.position.z + depth;
            }
            else if (current.z < transform.position.z - depth)
            {
                clamp.z = transform.position.z - depth;
            }

            // y
            if (current.y > transform.position.y + height)
            {
                clamp.y = transform.position.y + height;
            }
            if (current.y < transform.position.y - height)
            {
                clamp.y = transform.position.y - height;
            }

            tank.transform.position = clamp;
        }
    }


}
