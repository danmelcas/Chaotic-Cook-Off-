using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flammable : MonoBehaviour
{
    // Script attached to flammable objects; only flammable objects can catch on fire 
    private bool isOnFire;
    [SerializeField] private GameObject firePrefab; 
    void Awake() => isOnFire = false; 
    [SerializeField] private Material burntMaterial;
    private float timeSinceLastExtinguishingAttempt = 3f;

    void Update()
    {
        timeSinceLastExtinguishingAttempt += Time.deltaTime;   
    }
    public bool IsOnFire() { 
        return isOnFire; 
    }
    public void StartFire() { 
        // Instantiate the fire prefab as a child of this object and make this a burnt object
        Debug.Log("StartFire called"); 
        var obj = Instantiate(firePrefab, transform);
        obj.transform.localPosition = Vector3.zero; 
        isOnFire = true;
        GetComponent<MeshRenderer>().material = burntMaterial;    
        var cookable = GetComponent<Cookable>(); 
        if (cookable != null) cookable.BecomeBurnt();  
    }

    // Called by fire when fire extinguisher reduces its health to 0, resets onFire variable
    public void StopFire() { 
        isOnFire = false; 
    }

    public void AttemptedToExtinguishFire() { 
        timeSinceLastExtinguishingAttempt = 0f; 
    }
    // Float used in fire spread script to avoid re-starting fires immediately after they are extinguished
    public float TimeSinceLastExtinguishingAttempt() { 
        return timeSinceLastExtinguishingAttempt;
    }
}
