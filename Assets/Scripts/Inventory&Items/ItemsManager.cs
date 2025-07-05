using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class ItemsManager : MonoBehaviour
{
    public static ItemsManager sInstance { get; private set; }

    [SerializeField] AnimationCurve itemBobCurve;

    [SerializeField] List<GameObject> itemPrefabPool = new List<GameObject>();

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

    public GameObject SpawnRandomItem()
    {
        GameObject prefab = itemPrefabPool[Random.Range(0, itemPrefabPool.Count)];
        return Instantiate(prefab);
    }

    public AnimationCurve GetItemBobCurve()
    {
        return itemBobCurve;
    }


}
