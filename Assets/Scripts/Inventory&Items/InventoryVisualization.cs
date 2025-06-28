using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class InventoryVisualization : MonoBehaviour
{
    public static InventoryVisualization sInstance { get; private set; }

    List<Image> inventorySlots = new List<Image>();

    [SerializeField] GameObject InventoryHoverCursor;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // singleton
        if (sInstance != null && sInstance != this)
        {
            Destroy(this);
        }
        else
        {
            sInstance = this;
        }

        // setup slots
        for (int i = 0; i < transform.childCount; i++)
        {
            inventorySlots.Add(transform.GetChild(i).gameObject.GetComponent<Image>());
        }

    }

    // Update is called once per frame
    void Update()
    {
        foreach (Image item in inventorySlots)
        {
            int index = inventorySlots.IndexOf(item);
            if (item != null && index < PlayerController.sInstance.GetInventorySize())
            {
                if (PlayerController.sInstance.GetItemAtIndex(index))
                {
                    item.color = Color.green;
                }
                else
                {
                    item.color = Color.white;
                }
                
                if (index == PlayerController.sInstance.GetCurrentItem())
                {
                    InventoryHoverCursor.transform.position = item.transform.position;
                }

            }
        }


    }
}
