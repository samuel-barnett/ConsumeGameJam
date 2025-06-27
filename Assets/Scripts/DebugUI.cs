using TMPro;
using UnityEngine;

public class DebugUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text1; 


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text1.text = "why: " + Input.mousePosition;
    }

}
