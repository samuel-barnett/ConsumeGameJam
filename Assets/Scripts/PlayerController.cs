using UnityEngine;


public class PlayerController : Entity
{
    // 
    bool canJump;


    // Serializes
    [SerializeField] GameObject cameraRoot;


    [SerializeField] float movementSpeed = 5;
    [SerializeField] float jumpForce = 1;


    protected override void SetupEntity()
    {
        cameraRoot.transform.SetParent(null);
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

        Vector3 move = cameraRoot.transform.worldToLocalMatrix * new Vector3(moveSide, 0, moveForward).normalized;
        rb.AddForce(move * movementSpeed);

        // look in the direction of movement
        transform.LookAt(transform.position + move);
    }

    // trigger hits or leaves floor
    private void OnTriggerEnter(Collider other)
    {
        canJump = true;
    }
    private void OnTriggerExit(Collider other)
    {
        canJump = false;
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            //Debug.Log("jump");
            rb.AddForce(transform.up * jumpForce);

        }
    }

    void CameraCode()
    {
        cameraRoot.transform.position = transform.position;

        // rotate left
        if (Input.GetKeyDown(KeyCode.Q))
        {
            cameraRoot.transform.Rotate(Vector3.up * 90);
        }

        // rotate right
        if (Input.GetKeyDown(KeyCode.E))
        {
            cameraRoot.transform.Rotate(Vector3.down * 90);
        }
    }


}
