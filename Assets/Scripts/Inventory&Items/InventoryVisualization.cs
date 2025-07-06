using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class InventoryVisualization : MonoBehaviour
{
    public static InventoryVisualization sInstance { get; private set; }

    List<GameObject> inventorySlots = new List<GameObject>();

    [SerializeField] GameObject InventoryHoverCursor;

    [SerializeField] Sprite blueSprite;
    [SerializeField] Sprite greenSprite;
    [SerializeField] Sprite orangeSprite;
    [SerializeField] Sprite purpleSprite;

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
            inventorySlots.Add(transform.GetChild(i).gameObject);
        }

    }

    void FixedUpdate()
    {
        foreach (GameObject item in inventorySlots)
        {
            Image image = item.transform.GetChild(0).GetComponent<Image>();
            int index = inventorySlots.IndexOf(item);
            if (item != null && index < PlayerController.sInstance.GetInventorySize())
            {
                GameObject obj = PlayerController.sInstance.GetItemAtIndex(index);

                if (obj)
                {
                    image.enabled = true;
                    Consumable consumable = obj.GetComponent<Consumable>();
                    Color c;
                    image.sprite = GetSprite(consumable, out c);
                }
                else
                {
                    image.enabled = false;
                }

                if (index == PlayerController.sInstance.GetCurrentItem())
                {
                    InventoryHoverCursor.transform.position = item.transform.position;
                }

            }
        }


    }

    public Sprite GetSprite(Consumable consumable, out Color color)
    {
        if (consumable)
        {
            if (consumable is BluePotion)
            {
                color = Color.blue;
                return blueSprite;
            }
            else if (consumable is GreenPotion)
            {
                color = Color.green;
                return greenSprite;
            }
            else if (consumable is OrangePotion)
            {
                color = Color.orange;
                return orangeSprite;
            }
            else if (consumable is PurplePotion)
            {
                color = Color.purple;
                return purpleSprite;
            }
            else
            {
                Debug.LogError("noooooooo");
            }
        }
        color = Color.white;
        return null;
    }


}
