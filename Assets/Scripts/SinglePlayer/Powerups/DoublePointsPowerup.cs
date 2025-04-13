using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoublePointsPowerup : Powerup
{
    // When active, this powerup doubles the points for orders that receive positive points 
    private bool powerupIsActive;
    [SerializeField] private float _powerupDuration = 10f; 
    private float timeSinceLastActivated; 
    private float multiplier = 2f; 
    public override void ActivatePowerup()
    {
        powerupIsActive = true; 
        timeSinceLastActivated = 0f;
    }

    public override void DeactivatePowerup() {
        powerupIsActive = false; 
    }

    public override bool IsActive() { 
        return powerupIsActive; 
    }
    public override string GetName()
    {
        return "Doubled Rewards"; 
    }

    public override bool IsBeneficial()
    {
        return true;
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

    
}
