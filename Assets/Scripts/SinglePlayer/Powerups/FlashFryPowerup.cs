using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashFryPowerup : Powerup
{
    // When set active, cookables are instantly cooked  
    // Cooking logic is handled by Cookable script 
    private bool powerupIsActive;
    [SerializeField] private float _powerupDuration;
    private float timeSinceLastActivated; 

    public override bool IsBeneficial() { 
        return true; 
    }
    void Awake()
    {
       powerupIsActive = false; 
       timeSinceLastActivated = 0f;
    }

    public override bool IsActive() { 
        return powerupIsActive;
    }
    void Update()
    {
        if (powerupIsActive) {
            timeSinceLastActivated += Time.deltaTime;
            if (timeSinceLastActivated >= _powerupDuration) { 
                DeactivatePowerup(); 
            }
        }
    }
    public override void ActivatePowerup()
    {
        Debug.Log("Flash fry activated, powerup active"); 
       timeSinceLastActivated = 0f; 
       powerupIsActive = true; 
    }

    public override void DeactivatePowerup() { 
        powerupIsActive = false; 
    }

    public override string GetName()
    {
        return "Flash Frying"; 
    }
    
}
