using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TelekinesisPowerup : Powerup
{
    // This powerup extends the player's reach by a certain multiplier, making it so that 
    // they can grab or interact with any object from any distance. 
    private bool powerupIsActive;
    [SerializeField] private float _powerupDuration;
    private float timeSinceLastActivated; 
    private Player player;
    [SerializeField] private float reachMultiplier;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>(); 
        if (player==null) Debug.Log("Player is null");
    }

    public override string GetName()
    {
        return "Telekinesis";
    }

    public override bool IsBeneficial()
    {
        return true; 
    }

    public override bool IsActive() { 
        return powerupIsActive; 
    }
    // Start is called before the first frame update
    
    // Update is called once per frame
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
        player.MultiplyPlayerReach(reachMultiplier); 
    }

    public override void DeactivatePowerup() { 
        powerupIsActive = false;
        player.MultiplyPlayerReach(1/reachMultiplier); 
    }
}
