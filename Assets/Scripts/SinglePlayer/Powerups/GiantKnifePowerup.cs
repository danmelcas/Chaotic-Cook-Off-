using UnityEngine; 


public class GiantKnifePowerup : Powerup {
    [SerializeField] private float scaleMultiplier;
    private bool powerupIsActive;  
    [SerializeField] private float _powerupDuration;
    private float timeSinceLastActivated; 

    // The giant knife powerup instantly chops choppables into the remaining slices left
        // This chopping logic is handled by the choppable script   

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
        transform.localScale = transform.localScale / scaleMultiplier; 
    }

    public override void ActivatePowerup()
    {
        timeSinceLastActivated = 0;
        powerupIsActive = true;
        Debug.Log("Giant knife powerup activate powerup called");
        transform.localScale = transform.localScale * scaleMultiplier; 
    }

    public override bool IsActive() {
        return powerupIsActive; 
    }

    public override string GetName()
    {
        return "Giant Knife";
    }
}