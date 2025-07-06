using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public static HPBar sInstance { get; private set; }

    [SerializeField] GameObject heartUIElement;


    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite emptyHeart;

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


    private void Start()
    {
        MenuButtons.sInstance.gameObject.SetActive(false);
    }

    public void UpdateHealthDisplay(int currentHP, int maxHP)
    {
        if (transform.childCount == maxHP)
        {
            //Debug.Log("correct number of children");
            // do nothing ur good
        }
        else if (transform.childCount < maxHP)
        {
            //Debug.Log("not enough children");
            while (transform.childCount < maxHP)
            {
                Instantiate(heartUIElement, transform);
            }
        }
        else
        {
            //Debug.Log("too many children");
            while (transform.childCount > maxHP)
            {
                Destroy(transform.GetChild(0));
            }
        }

        Image spriteRendererRef;
        for (int i = 0; i < maxHP; i++)
        {
            spriteRendererRef = transform.GetChild(i).GetComponent<Image>();
            if (i < currentHP)
            {
                spriteRendererRef.sprite = fullHeart;
            }
            else
            {
                spriteRendererRef.sprite = emptyHeart;
            }
        }
    }
}
