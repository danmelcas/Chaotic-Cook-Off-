using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.UI;
using UnityEngine;

public class Fire : MonoBehaviour
{
    // Script attached to fire prefab, spawns and controls spreading behavior of fire and the death of the fire 
    [SerializeField] private float health; 
    [SerializeField] private float timeToSpread; 
    [SerializeField] private float spreadFireTimer;
    [SerializeField] private float spreadRadius;
    [SerializeField] private float maxSpreadRadius; 
    private Player player;
    private Flammable flammable; 
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();    
        flammable = GetComponentInParent<Flammable>(); if (flammable==null) Debug.LogError("Flammable missing");
        transform.localScale = new Vector3(3,4,3);  
        // var particleSystemTransform = GetComponentInChildren<ParticleSystem>().transform; 
        // particleSystemTransform.localPosition = new Vector3(0,0,0); 
    }
    void Update()
    {
        // Every timeToSpread seconds, try spreading the fire, and have it grow in size 
        if (Time.timeScale == 0f) return; 
        spreadFireTimer += Time.deltaTime; 
        if (spreadFireTimer >= timeToSpread) { 
            TrySpreading(); 
            spreadFireTimer = 0f; 
            if (spreadRadius < maxSpreadRadius) spreadRadius += 0.5f; 
        }
    }
    public void Extinguish(float extinguishingPower) {
        health -= extinguishingPower; 
        Debug.Log("Extinguishing fire, health: " + health);
        flammable.AttemptedToExtinguishFire(); 
        if (health < 0) { 
            Die(); 
        } 
    }

    void Die() { 
        Debug.Log("Fire dying"); 
        gameObject.GetComponentInParent<Flammable>().StopFire(); 
        Destroy(transform.gameObject); 
    }

    void TrySpreading() { 
        Debug.Log("Trying to spread fire"); 
        Collider[] hits = Physics.OverlapSphere(transform.position, spreadRadius);
        foreach (var hit in hits) { 
            Flammable f = hit.transform.GetComponent<Flammable>(); 
            
            // If the hit object is flammable and it's not already on fire, 
            // and it's not being held by the player, call start fire 
    
    /* However, if it's currently being put out or just recently been put out 
     we should avoid restarting the fire on the
     object (can get very difficult or almost impossible to put out with many 
     things on fire next to one another)
    */
            if (f != null && !f.IsOnFire() && player.GetHeldItem() != hit.gameObject && 
            f.TimeSinceLastExtinguishingAttempt() >= 10f) { 
                // Catch on fire. 
                Debug.Log("Found flammable object not on fire; starting fire"); 
                f.StartFire(); 
            }
        }
    }

    // Helper method to visualize the spreadable radius of the fire in the scene view
    void OnDrawGizmos() { 
        Gizmos.DrawWireSphere(transform.position, spreadRadius); 
    }
}
