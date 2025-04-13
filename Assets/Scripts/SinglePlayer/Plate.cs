using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    private Player player; 
    private bool isOnSurface; 
    private Rigidbody rb; 
    private AudioSource audioSource; 
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>(); 
        rb = GetComponent<Rigidbody>();
        // audioSource = GetComponent<AudioSource>(); 
    }


    void Update()
    {
        if (!isOnSurface && player.GetHeldItem() != gameObject) rb.isKinematic = false; 
    }

    private void OnCollisionEnter(Collision other) {
        // Debug.Log("OnCollisionEnter entered"); 
        if (other.gameObject.CompareTag("Surface") && player.GetHeldItem() != gameObject) { 
            isOnSurface = true; 
            StartCoroutine(BecomeKinematic()); 
        }
    }

    private void OnCollisionExit(Collision collision)
    {   
       if (collision.gameObject.CompareTag("Surface")) { 
            isOnSurface = false; 
       } 
    }
    IEnumerator BecomeKinematic() { 
        var rb = GetComponent<Rigidbody>();
        float secondsStill=0f; 
        while (rb.velocity.magnitude > 0.01f) yield return null; // Velocity needs to slow down  
        while (rb.velocity.magnitude < 0.01f && secondsStill < 1f) {
            secondsStill += Time.deltaTime; 
            yield return null; 
        }
        // if still touching a surface, become kinematic 
        if (player.GetHeldItem() != rb.gameObject && isOnSurface && rb.isKinematic==false) { 
            rb.isKinematic = true;
        }
    }

    public void IgnoreCollisionsWithIngredients() { 
        foreach (Ingredient i in transform.GetComponentsInChildren<Ingredient>()) {
            Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), i.GetComponent<Collider>(), true);
        }
    }

    public void ResetCollisionsWithIngredients() { 
        foreach (Ingredient i in transform.GetComponentsInChildren<Ingredient>()) {
            Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), i.GetComponent<Collider>(), false);
        }
    }

    // public void PlaySoundEffect() { 
    //     audioSource.Play(); 
    // }
    
}