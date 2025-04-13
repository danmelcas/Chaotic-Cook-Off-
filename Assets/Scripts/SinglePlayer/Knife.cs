using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    // Start is called before the first frame update
    
    private bool isHeld=false; 
    private AudioSource audioSource;
    private PlayerMovementAndCamera playerCamera; 
    void Awake()
    {
        audioSource = GetComponent<AudioSource>(); 
        playerCamera = FindObjectOfType<PlayerMovementAndCamera>(); 
    }
    public void PickUpKnife() {
        isHeld=true; 
        transform.rotation = Quaternion.Euler(new Vector3(0,90,0));
    }

    public void DropKnife() { 
        isHeld=false; 
    }

    void LateUpdate() // Overwrite basic interaction rotation 
    {
    }

    public void PlayChoppingSound() { 
        audioSource.Play(); 
    }

    
}

