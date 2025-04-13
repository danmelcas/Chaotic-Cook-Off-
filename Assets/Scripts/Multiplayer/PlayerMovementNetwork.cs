using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Unity.Netcode; 
using Unity.Netcode.Components;
using System.Security.Cryptography;

public class PlayerMovementNetwork : NetworkBehaviour {
    [SerializeField] private float playerSpeed=5f;
    [SerializeField] private float sensX; 
    [SerializeField] private float sensY; 
    
    private Rigidbody rb; 
    [SerializeField] private float groundDrag = 5f;
    private float horizontalInput; 
    private float verticalInput; 
    private float xRotation;
    private float yRotation; 
    private GameController gameController; 
    [SerializeField] private float smoothTime = 0.9f; 
    private Camera playerCamera; 
    private NetworkVariable<int> randomNumber = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public override void OnNetworkSpawn()
    {
         playerCamera = GetComponentInChildren<Camera>(); 
        if (playerCamera == null) Debug.Log("Player camera is missing");
        rb = GetComponent<Rigidbody>(); if (rb == null) Debug.Log("Rigid body missing"); 
        rb.freezeRotation = true; 
        gameController = FindAnyObjectByType<GameController>(); 
        Debug.Log("This player isServer: " + IsServer + ", isHost: " + IsHost + ", isClient: " + IsClient);
        
        randomNumber.OnValueChanged += (int previousValue, int newValue) => {
            Debug.Log(OwnerClientId + "; randomNumber: " + randomNumber.Value);
        };
    }

    void Start()
    {
       
    }
    
    // Update is called once per frame
    private void Update()
    {
        if (!IsOwner) return;   
        if (Input.GetKeyDown(KeyCode.T)) { 
           TestServerRpc(); 
           // randomNumber.Value = Random.Range(0, 100);  
        }
        MyInput();
        SmoothLook();
        rb.drag = groundDrag; 
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return; 
        Move(); 
        
    }

    void MyInput() { 
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical"); // -1 for back (S), 1 for forward (W) 
    }

    void Move() { 
        // if (gameController.GetGameState() != GameController.GameState.Playing) return; 
        Vector3 moveDirection = transform.right * horizontalInput + transform.forward * verticalInput;
        // transform.Translate(moveDirection * playerSpeed * Time.fixedDeltaTime, Space.World); 
        rb.AddForce(moveDirection.normalized * playerSpeed, ForceMode.Force);
    }

    
    void SmoothLook(){
    //    if (gameController.GetGameState() != GameController.GameState.Playing) return; 
       float mouseX = Input.GetAxis("Mouse X") * sensX;
       float mouseY = Input.GetAxis("Mouse Y") * sensY;
       yRotation += mouseX;
       xRotation -= mouseY;

       xRotation = Mathf.Clamp(xRotation, -90f, 90f);

       var targetRotation = Quaternion.Euler(0, yRotation, 0);
       var targetCameraRotation = Quaternion.Euler(xRotation, yRotation, 0); 

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothTime);
        playerCamera.transform.rotation = Quaternion.Slerp(playerCamera.transform.rotation, targetCameraRotation, smoothTime);
    }

    [ServerRpc]
    private void TestServerRpc() { 
        Debug.Log("TestServerRpc " + OwnerClientId); 
    }
}




