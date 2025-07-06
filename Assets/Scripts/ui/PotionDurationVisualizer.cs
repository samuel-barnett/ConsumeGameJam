using UnityEngine;
using UnityEngine.UI;

public class PotionDurationVisualizer : MonoBehaviour
{
    float timeElapsed;
    float duration;

    [Header("Refs to children")]
    [SerializeField] Image icon;
    [SerializeField] Image bar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        //( value - min ) / ( max - min )
        float value = (timeElapsed - duration) / (0 - duration);
        bar.transform.localScale = new Vector3(value, 1, 1);

        if (timeElapsed >= duration)
        {
            Destroy(gameObject);
        }
    }

    public void SetupVisualizer(Consumable consumable)
    {
        duration = consumable.GetDuration();
        Color potionColor;
        icon.sprite = InventoryVisualization.sInstance.GetSprite(consumable, out potionColor);
        bar.color = potionColor;

    }

}
