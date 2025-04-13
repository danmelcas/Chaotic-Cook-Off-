using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperSpeedPowerup : Powerup
{
    // When this powerup is active, players' speed is multiplied by the speedMultiplier
    private PlayerMovementAndCamera playerMovement;
    private bool powerupIsActive;
    [SerializeField] private float _powerupDuration;
    private float timeSinceLastActivated; 
    [SerializeField] private float speedMultiplier;

    public override bool IsBeneficial()
    {
        return true; 
    }

    public override bool IsActive()
    {
        return powerupIsActive; 
    }
    void Awake()
    {
       powerupIsActive = false; 
       timeSinceLastActivated = 0f;
       playerMovement = FindFirstObjectByType<PlayerMovementAndCamera>(); 
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

    // Player speed is reset to its original speed
    public override void DeactivatePowerup() { 
        powerupIsActive = false; 
        playerMovement.MultiplyPlayerSpeed(1/speedMultiplier);
    }

    public override string GetName()
    {
        return "Super Speed"; 
    }
    
}
