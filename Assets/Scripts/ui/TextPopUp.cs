using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextPopUp : MonoBehaviour
{
    float timeAlive;

    Color textColor;
    TextMeshProUGUI textRef;

    Vector3 originalPosition;
    Vector3 velocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalPosition = transform.position;
        textRef = GetComponent<TextMeshProUGUI>();
        textColor = textRef.color;
        velocity = new Vector3(Random.Range(-0.5f, 0.5f), 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        timeAlive += Time.deltaTime;

        // look at camera
        //transform.LookAt(transform.position - Camera.main.transform.position, Vector3.up);

        // move
        //originalPosition
        transform.position = Camera.main.WorldToScreenPoint(originalPosition);
        transform.Translate(velocity * Time.deltaTime);
        originalPosition += (velocity * Time.deltaTime);

        // lower opacity
        textColor.a = TextPopUpSpawner.sInstance.EvaluateOpacity(timeAlive);
        if (textColor.a <= 0)
        {
            Destroy(gameObject);
        }
        textRef.color = textColor;
    }
}
