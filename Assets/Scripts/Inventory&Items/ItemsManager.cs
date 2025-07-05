using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    public static ItemsManager sInstance { get; private set; }

    [SerializeField] AnimationCurve itemBobCurve;


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

    public AnimationCurve GetItemBobCurve()
    {
        return itemBobCurve;
    }


}
