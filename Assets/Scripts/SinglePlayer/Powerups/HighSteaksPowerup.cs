using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighSteaksPowerup : Powerup
{
    // When active, meat objects float to the ceiling and then back down. 
    // Logic handled by the high steaks script. 
    private bool powerupIsActive;  
    [SerializeField] private float _powerupDuration;
    private float timeSinceLastActivated; 

    public override bool IsBeneficial() { 
        return true; 
    }
    void Awake() { 
        powerupIsActive = false; 
    }

    void Update() { 
        if (powerupIsActive) { 
            timeSinceLastActivated += Time.deltaTime; 
            if (timeSinceLastActivated >= _powerupDuration) { 
                DeactivatePowerup();
            }
        }    
    }

    public override void DeactivatePowerup() {
        powerupIsActive = false; 
    }

    public override void ActivatePowerup()
    {
        timeSinceLastActivated = 0;
        powerupIsActive = true;
    }

    public override bool IsActive() { 
        return powerupIsActive;
    }

    public override string GetName()
    {
        return "High Steaks";
    }
}