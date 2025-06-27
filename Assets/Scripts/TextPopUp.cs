using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshPro))]
public class TextPopUp : MonoBehaviour
{
    float timeAlive;

    Color textColor;
    TextMeshPro textRef;

    Vector3 velocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textRef = GetComponent<TextMeshPro>();
        textColor = textRef.color;
        velocity = new Vector3(Random.Range(-0.5f, 0.5f), 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        timeAlive += Time.deltaTime;

        // look at camera
        transform.LookAt(transform.position - Camera.main.transform.position, Vector3.up);

        // move
        transform.Translate(velocity * Time.deltaTime);

        // lower opacity
        textColor.a = TextPopUpSpawner.sInstance.EvaluateOpacity(timeAlive);
        if (textColor.a <= 0)
        {
            Destroy(gameObject);
        }
        textRef.color = textColor;
    }
}
