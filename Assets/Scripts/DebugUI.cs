using TMPro;
using UnityEngine;

public class DebugUI : MonoBehaviour
{
    public static DebugUI sInstance { get; private set; }

    [SerializeField] TextMeshProUGUI text1;
    [SerializeField] TextMeshProUGUI text2;

    // message counter
    int messagesSentHere = 0;

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

    // Update is called once per frame
    void Update()
    {
        text1.text = "time: " + PlayerController.sInstance.GetTimeSince();
        if (messagesSentHere > 1)
        {
            Debug.LogError("too many debugUI messages sent");
        }
        messagesSentHere = 0;
    }

    public void SetDebugText(string message)
    {
        text2.text = message;
        messagesSentHere++;
    }


}
