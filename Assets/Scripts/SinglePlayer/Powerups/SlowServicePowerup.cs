using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowServicePowerup : Powerup
{
    // Planned powerup not in the game currently; might not be in the future either 
    // Attached to player object with playerMovementAndCamera script 
    // Identical to SuperSpeed, but multiplier is < 1 
    private PlayerMovementAndCamera playerMovement;
    private bool powerupIsActive;
    [SerializeField] private float _powerupDuration;
    private float timeSinceLastActivated; 
    [SerializeField] private float speedMultiplier;

    public override bool IsBeneficial() { 
        return false; 
    }
    void Awake()
    {
       powerupIsActive = false; 
       timeSinceLastActivated = 0f;
       playerMovement = GetComponent<PlayerMovementAndCamera>(); 
       if (playerMovement == null) Debug.Log("Player movement null");
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
       playerMovement.MultiplyPlayerSpeed(speedMultiplier);
       
    }

    public override void DeactivatePowerup() { 
        powerupIsActive = false; 
        playerMovement.MultiplyPlayerSpeed(1/speedMultiplier);
    }

    public override string GetName()
    {
        return "Slow Service"; 
    }

    public override bool IsActive()
    {
        return powerupIsActive; 
    }
    
}
