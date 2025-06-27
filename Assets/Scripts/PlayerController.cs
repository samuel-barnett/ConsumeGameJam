using System.Collections;
using UnityEngine;


public class PlayerController : Entity
{
    // states 
    bool canJump;
    bool canRotateCamera;

    // Serializes
    [Header("Refs")]
    [SerializeField] GameObject cameraRoot;

    [Header("Adjustable Values")]
    [SerializeField] float movementSpeed = 5;
    [SerializeField] float jumpForce = 1;
    [SerializeField] AnimationCurve cameraRotation;

    protected override void SetupEntity()
    {
        cameraRoot.transform.SetParent(null);
        canRotateCamera = true;
    }

    // Update is called once per frame
    void Update()
    {
        MovementCode();
        Jump();
        CameraCode();

    }

    void MovementCode()
    {
        float moveSide = Input.GetAxisRaw("Horizontal");
        float moveForward = Input.GetAxisRaw("Vertical");

        Vector3 moveVector = ((cameraRoot.transform.right * moveSide) + (cameraRoot.transform.forward * moveForward)).normalized;

        //Vector3 move = cameraRoot.transform.worldToLocalMatrix * new Vector3(moveSide, 0, moveForward).normalized;
        rb.AddForce(moveVector * movementSpeed);

        // look in the direction of movement
        transform.LookAt(transform.position + moveVector);
    }

    // trigger hits or leaves floor
    private void OnTriggerEnter(Collider other)
    {
        canJump = true;
    }
    private void OnTriggerExit(Collider other)
    {
        //canJump = false;
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            //Debug.Log("jump");
            rb.AddForce(transform.up * jumpForce);
            canJump = false;
        }
    }

    void CameraCode()
    {
        cameraRoot.transform.position = transform.position;

        // rotate left
        if (Input.GetKeyDown(KeyCode.Q) && canRotateCamera)
        {
            StartCoroutine(RotateCamera(true));
            //cameraRoot.transform.Rotate(Vector3.up * 90);
        }

        // rotate right
        if (Input.GetKeyDown(KeyCode.E) && canRotateCamera)
        {
            StartCoroutine(RotateCamera(false));
            //cameraRoot.transform.Rotate(Vector3.down * 90);
        }
    }

    IEnumerator RotateCamera(bool left)
    {
        canRotateCamera = false;
        Vector3 rotationVector = (left) ? Vector3.up : Vector3.down;
        Quaternion originalRotation = cameraRoot.transform.rotation;

        float timer = 0;
        float currentEvaluation;
        while (timer < 0.25)
        {
            timer += Time.fixedDeltaTime;
            currentEvaluation = cameraRotation.Evaluate(timer);

            cameraRoot.transform.rotation = originalRotation;
            cameraRoot.transform.Rotate(rotationVector * currentEvaluation);

            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }

        canRotateCamera = true;
    }


}
