using UnityEngine;

public class Target : MonoBehaviour
{
    float timer = 0;
    float maxTime;

    GameObject bulletRef;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (bulletRef == null)
        {
            Destroy(gameObject);
        }
    }


    public void SetRef(GameObject bullet)
    {
        bulletRef = bullet;
    }


}
