using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighSteaks : MonoBehaviour
{
    // Attached to meat objects; when HighSteaksPowerup is active, 
    // these steaks float upwards (assuming they aren't plated,
    // proxy measure of this is if they're kinematic; this is why 
    // we are using rb.AddForce instead of transform.translate

    private HighSteaksPowerup highSteaksPowerup;
    private float movementTimer;
    private Rigidbody rb;  
    [SerializeField] private float floatingForce; 
    [SerializeField] private float floatCycleTime; // Floats up 1/3 of the time, pauses for another 1/3, then descends gently down 
    void Awake()
    {
        highSteaksPowerup = FindFirstObjectByType<HighSteaksPowerup>(); 
        movementTimer = 0;
        rb = GetComponent<Rigidbody>(); if (rb == null) Debug.LogError("Rb is null!"); 
    }


    void Update()
    {
        if (!highSteaksPowerup.IsActive()) rb.useGravity = true;  
        else { 
            rb.useGravity = false;
            movementTimer += Time.deltaTime;
            if (movementTimer > floatCycleTime) { 
                movementTimer = 0f; 
            }
            else if (movementTimer > 2*floatCycleTime/3) { 
                // Descends down 
                rb.AddForce(Vector3.down * floatingForce, ForceMode.Force); 
            }
            else if (movementTimer > floatCycleTime/3) {
                // Then pauses  
            }
            else { 
                // Rises for a few seconds in the first couple seconds 
                rb.AddForce(Vector3.up * floatingForce, ForceMode.Force); 
            }
        }
    }
}
