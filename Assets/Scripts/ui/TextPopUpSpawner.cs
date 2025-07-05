using TMPro;
using UnityEngine;

public class TextPopUpSpawner : MonoBehaviour
{
    public static TextPopUpSpawner sInstance { get; private set; }

    [SerializeField] GameObject TextPopUpPrefab;

    [Header("Adjustable Variables")]
    [Tooltip("This Curve Controls The Opacity Of The Text Color. As The Curve Approaches 0, The Text Dissapears")]
    [SerializeField] AnimationCurve opacityCurve;


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
    }

    public float EvaluateOpacity(float time)
    {
        return opacityCurve.Evaluate(time);
    }


    void SpawnPopUp(string message, Vector3 position, Color color)
    {
        GameObject obj = Instantiate(TextPopUpPrefab, transform);
        TextMeshProUGUI text = obj.GetComponent<TextMeshProUGUI>();
        obj.transform.position = position;
        text.text = message;
        text.color = color;
    }

    public void DamagePopUp(int damage, Vector3 position)
    {
        SpawnPopUp(damage.ToString(), position, Color.red);
    }

    public void ShieldDamagePopUp(int damage, Vector3 position)
    {
        SpawnPopUp(damage.ToString(), position, Color.blue);
    }

    public void HealPopUp(int heal, Vector3 position)
    {
        SpawnPopUp(heal.ToString(), position, Color.green);
    }

    

}
