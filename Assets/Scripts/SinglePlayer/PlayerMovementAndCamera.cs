using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovementAndCamera : MonoBehaviour
{
    // Class for player movement and camera 
    [SerializeField] private float playerSpeed=5f;
    public float sensX; 
    public float sensY; 
    
    [SerializeField] private Rigidbody rb; 
    [SerializeField] private float groundDrag = 5f;
    
    private float horizontalInput; 
    private float verticalInput; 
    private float xRotation;
    private float yRotation; 
    private bool freezeLooking=false; 
    private GameController gameController; 

    public void SetFreezeLooking(bool set) { 
        freezeLooking = set; 
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; 
        gameController = FindAnyObjectByType<GameController>(); 
    }

    // Update is called once per frame
    private void Update()
    {
        // LookAround();  
        
        MyInput();
        rb.drag = groundDrag; 
    }

    private void FixedUpdate()
    {
        Move(); 
        SmoothLook();
    }

    void MyInput() { 
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical"); // -1 for back (S), 1 for forward (W) 

    }
    void Move() { 
        if (gameController.GetGameState() != GameController.GameState.Playing) return; 
        Vector3 moveDirection = transform.right * horizontalInput + transform.forward * verticalInput;
        
        rb.AddForce(moveDirection.normalized * playerSpeed, ForceMode.Force);
        //rb.drag = groundDrag; 
        //  Vector3 moveDirection = (transform.TransformDirection(Vector3.right) * horizontalInput +
        //                          transform.TransformDirection(Vector3.forward) * verticalInput).normalized;

        // transform.Translate(moveDirection * playerSpeed * Time.deltaTime, Space.World);
    
    }

    [SerializeField] private float smoothTime = 0.05f; 
    void SmoothLook(){
       if (freezeLooking || gameController.GetGameState() != GameController.GameState.Playing) return; 
       float mouseX = Input.GetAxis("Mouse X") * sensX;
       float mouseY = Input.GetAxis("Mouse Y") * sensY;

       yRotation += mouseX;
       xRotation -= mouseY;

       xRotation = Mathf.Clamp(xRotation, -90f, 90f);

       var targetRotation = Quaternion.Euler(0, yRotation, 0);
       var targetCameraRotation = Quaternion.Euler(xRotation, yRotation, 0); 

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothTime);
        Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, targetCameraRotation, smoothTime);
    }

    public void MultiplyPlayerSpeed(float multiplier) { 
        playerSpeed = playerSpeed * multiplier; 
    }
}
