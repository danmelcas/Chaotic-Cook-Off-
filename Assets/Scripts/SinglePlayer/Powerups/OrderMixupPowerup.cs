using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderMixupPowerup : Powerup
{
    // This powerup refreshes current orders by communicating with the order manager. 
    private bool powerupIsActive;
    [SerializeField] private float _powerupDuration;
    private float timeSinceLastActivated; 
    private OrderManager orderManager;
    void Awake()
    {
       powerupIsActive = false; 
       timeSinceLastActivated = 0f;
       orderManager = FindFirstObjectByType<OrderManager>();
    }

    public override bool IsActive() { 
        return powerupIsActive;
    }
    
    public override bool IsBeneficial() { 
        return false; 
    }

    public override void ActivatePowerup()
    {
       timeSinceLastActivated = 0f; 
       powerupIsActive = true; 
       orderManager.RefreshOrders(); 
    }

    public override void DeactivatePowerup() { 
        powerupIsActive = false; 
    }

    public override string GetName()
    {
        return "Order Mix-Up"; 
    }
}
