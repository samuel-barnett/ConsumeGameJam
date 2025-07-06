using UnityEngine;

public class PotionBarsCollection : MonoBehaviour
{
    public static PotionBarsCollection sInstance { get; private set; }

    [SerializeField] GameObject potionBarPrefab;

    private void Awake()
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

    
    public void AddBar(Consumable consumable)
    {
        GameObject newObj = Instantiate(potionBarPrefab, transform);
        if (newObj)
        {
            PotionDurationVisualizer newBar = newObj.GetComponent<PotionDurationVisualizer>();
            if (newBar)
            {
                newBar.SetupVisualizer(consumable);
            }
        }
    }



}
