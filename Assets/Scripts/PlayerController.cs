using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerController : Tank
{
    public static PlayerController sInstance { get; private set; }


    // states 
    bool canJump;
    bool canRotateCamera;

    // Serializes
    [Header("PlayerController - Refs")]
    [SerializeField] GameObject cameraRoot;

    [Header("PlayerController - Adjustable Values")]
    [SerializeField] float movementSpeed = 5;
    [SerializeField] float jumpForce = 1;
    [SerializeField] AnimationCurve cameraRotation;

    protected override void SetupEntity()
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

        cameraRoot.transform.SetParent(null);
        canRotateCamera = true;
    }

    // Update is called once per frame
    void Update()
    {
        MovementCode();
        //Jump();
        SwivelRotation();
        ShootInput();
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

    void SwivelRotation()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float length;

        if (groundPlane.Raycast(cameraRay, out length))
        {
            Vector3 pointToLook = cameraRay.GetPoint(length);
            SetSwivelRotation(pointToLook);
        }


        //RaycastHit hit;
        //bool objectHit = Physics.Raycast(mouse, Camera.main.transform.forward, out hit, 1000);
        
    }
    

    void ShootInput()
    {
        if (Input.GetMouseButtonDown((int)MouseButton.Left))
        {
            ShootProjectile();
        }
    }


    void CameraCode()
    {
        cameraRoot.transform.position = transform.position;

        // rotate left
        if (Input.GetKeyDown(KeyCode.Q) && canRotateCamera)
        {
            StartCoroutine(RotateCamera(true));
        }

        // rotate right
        if (Input.GetKeyDown(KeyCode.E) && canRotateCamera)
        {
            StartCoroutine(RotateCamera(false));
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

        cameraRoot.transform.rotation = originalRotation;
        cameraRoot.transform.Rotate(rotationVector * 90f);

        canRotateCamera = true;
    }


    


}
