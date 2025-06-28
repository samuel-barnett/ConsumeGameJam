using UnityEngine;

public class Reticle : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // get positions
        Vector3 reticlePosition = Input.mousePosition;

        GameObject barrelRef = PlayerController.sInstance.GetBarrel();
        Vector3 startPosition = Camera.main.WorldToScreenPoint(barrelRef.transform.position);
        Vector3 endPosition = Camera.main.WorldToScreenPoint(barrelRef.transform.position + barrelRef.transform.up * 20);

        // project it onto a line
        reticlePosition = VectorUtil.ClampPoint(reticlePosition, startPosition, endPosition);

        transform.position = reticlePosition;
    }
}
