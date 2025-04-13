using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeFreezePowerup : Powerup
{
    // When set active, all orders temporarily don't decrease their time. 
    // Logic handled in the Order script. 
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
       timeSinceLastActivated = 0f; 
       powerupIsActive = true; 
    }

    public override void DeactivatePowerup() { 
        powerupIsActive = false; 
    }

    public override string GetName()
    {
        return "Frozen Timers"; 
    }
    
}
