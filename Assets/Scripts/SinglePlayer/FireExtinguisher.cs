using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FireExtinguisher : MonoBehaviour
{
    [SerializeField] private float extinguishingPower; 
    private Camera playerCamera;
    private float playerReach; 
    private BasicInteraction basicInteraction; 
    private ParticleSystem foam; 
    private AudioSource sprayAudio; 

    void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        playerCamera = player.GetComponentInChildren<Camera>();    
        playerReach = player.GetComponent<Player>().GetPlayerReach(); 
        basicInteraction = GetComponent<BasicInteraction>(); 
        foam = GetComponentInChildren<ParticleSystem>(); if (foam==null) Debug.LogError("Foam null"); 
        sprayAudio = GetComponent<AudioSource>(); if (sprayAudio == null) Debug.LogError("Spray audio null"); 
    }

    void Update()
    {
        TryExtinguish();
    }


    void TryExtinguish() { 
        // If player is hitting left click and holding this object, extinguish the fire 
        if (Input.GetMouseButton(0) && basicInteraction.IsHeld()) { 
            if (!foam.isPlaying) { 
                foam.Play(); 
                sprayAudio.Play();
            }
            // Want to only reduce while foam is being shot out 
            if (foam.isPlaying) {
                Debug.Log("Tried to extinguish fire"); 
                RaycastHit hit; 
                // Don't want player to look at this extinguisher and try to interact with it 
                gameObject.layer = LayerMask.NameToLayer("Ignore Raycast"); 
                if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, playerReach)) {
                    var obj = hit.transform; 
                    var parentOnFire = obj.GetComponent<Fire>(); 
                    var childOnFire = obj.GetComponentInChildren<Fire>(); 
                    
                    if (parentOnFire != null) { 
                        Debug.Log("Hit a parent object on fire"); 
                        parentOnFire.Extinguish(extinguishingPower * Time.deltaTime); 
                    }
                    else if (childOnFire != null) { 
                        Debug.Log("Hit a child object on fire"); 
                        childOnFire.Extinguish(extinguishingPower * Time.deltaTime); 
                    }
            }
            }
        }
        // If player is not hitting left click, stop 
        else if (foam.isPlaying) { 
            foam.Stop();
            sprayAudio.Stop(); 
        }  
    }

    public void ResetLayer() { 
        gameObject.layer = LayerMask.NameToLayer("Default"); 
    }


}
