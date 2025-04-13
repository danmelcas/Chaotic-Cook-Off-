using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Powerup : MonoBehaviour
{
    // Start is called before the first frame update
    public abstract void ActivatePowerup();
    public abstract void DeactivatePowerup();
    public abstract bool IsActive(); 
    public abstract string GetName();
    public abstract bool IsBeneficial();
    
}
