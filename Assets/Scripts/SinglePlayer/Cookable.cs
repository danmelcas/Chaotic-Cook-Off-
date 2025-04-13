using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cookable : MonoBehaviour
{
    // Script attached onto cookable objects
    public enum CookState { 
        Raw,
        Cooked,
        Burnt
    }
    public CookState currentCookState;
    private bool isCooking=false;
    // Seconds elapsed since this ingredient began cooking
    private float cookTime; 
    // Seconds it takes to cook the ingredient 
    [SerializeField] private float timeToCook; 
    // Seconds it takes to burn the inredient (measured since begun cooking) 
    [SerializeField] private float timeToBurn; 
    [SerializeField] private float timeToCatchOnFire; 
    [SerializeField] private Mesh cookedMesh; 
    [SerializeField] private Mesh burntMesh; 
    [SerializeField] private Material cookedMaterial;
    [SerializeField] private Material burntMaterial; 
    private FlashFryPowerup flashFryPowerup; 
    private Flammable flammable; 
    void Awake()
    {
        currentCookState=CookState.Raw; 
        flashFryPowerup = GameObject.FindFirstObjectByType<FlashFryPowerup>();
        if (flashFryPowerup==null) Debug.LogError("Could not find flash fry powerup");
        flammable = GetComponent<Flammable>(); if (flammable==null) Debug.LogWarning("Flammable is null!"); 
    }
    public bool IsCooked() { 
        return currentCookState==CookState.Cooked; 
    }
   
    void Update()
    {
        UpdateCookState();
    }

    // Updates cook state based on the cook timers 
    void UpdateCookState() { 
        if (isCooking) { 
            cookTime += Time.deltaTime; 
            if (flashFryPowerup.IsActive() && currentCookState == CookState.Raw) {
                Debug.Log("Cookable found flashFryPowerup to be active"); 
                currentCookState = CookState.Cooked;
                cookTime = timeToCook; // Jumps ahead to cooked time 
                Debug.Log("Updates"); 
                UpdateLook(); 
            }
            // If this object has been cooking too long and is not already on fire, make it catch fire 
            if (cookTime >= timeToCatchOnFire && !flammable.IsOnFire() && flammable.TimeSinceLastExtinguishingAttempt() > 10f) {
                currentCookState = CookState.Burnt; 
                Debug.Log("Calling start fire from cookable"); 
                flammable.StartFire(); 
            }
            if (currentCookState != CookState.Burnt && cookTime >= timeToBurn) {
                currentCookState = CookState.Burnt;
                UpdateLook(); 
            }
            else if (currentCookState == CookState.Raw && cookTime >= timeToCook) {
                currentCookState = CookState.Cooked;
                UpdateLook(); 
            }
        }
    }

    
    void UpdateLook() {
        if (currentCookState == CookState.Cooked) {
            if (cookedMaterial != null) GetComponent<MeshRenderer>().material = cookedMaterial; 
            if (cookedMesh != null) GetComponent<MeshFilter>().mesh = cookedMesh; 
        }
        else if (currentCookState == CookState.Burnt) { 
            if (burntMaterial != null) GetComponent<MeshRenderer>().material = burntMaterial; 
            if (burntMesh != null) GetComponent<MeshFilter>().mesh = burntMesh; 

        }
    }

    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Stove")) { 
            isCooking=true; 
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Stove")) { 
            isCooking=false; 
        }
    }

    // If this ingredient is cooking but intense fires cause it to burn, it will become burnt 
    public void BecomeBurnt() { 
        if (currentCookState != CookState.Burnt) { 
            currentCookState = CookState.Burnt; 
            timeToCook = timeToBurn; 
        }
    }

}
