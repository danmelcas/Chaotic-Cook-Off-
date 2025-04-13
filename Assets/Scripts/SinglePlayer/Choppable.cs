using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class Choppable : MonoBehaviour
{
    // Script attached to choppable objects and defines the cutting behavior 
    [SerializeField] private GameObject nextCut; 
    [SerializeField] private GameObject slice;
    private GameObject knife;
    private GiantKnifePowerup giantKnifePowerup; 
    private float timeElapsed=0f;
    private bool giantKnifeActive = false; // If giantKnifePowerup is active
    [SerializeField] private int numSlicesLeft; // Max amount of slices that can be cut from this  
    [SerializeField] private float sliceSpacing; 
    [SerializeField] private bool isHalvable; // Differentiates between cuts like "salami cuts" (slices) and halving cuts (as with bread) 
    void Awake()
    {
        timeElapsed=0f;
        // Make knife be able to pass through this choppable ingredient 
        knife = GameObject.FindGameObjectWithTag("Knife");
        giantKnifePowerup = knife.GetComponent<GiantKnifePowerup>();
        Physics.IgnoreCollision(knife.GetComponent<Collider>(), transform.parent.GetComponent<Collider>());
    }

    void Update()
    {
        if (timeElapsed < 0.2f) timeElapsed += Time.deltaTime;
    }
    void OnTriggerEnter(Collider other)
    {
        if (timeElapsed < 0.2f) return; // Avoids bug where all slice triggers fire immediately
        Debug.Log("Choppable OnTriggerEnter entered");
        if (other.CompareTag("Knife")) {
            Debug.Log("Detected knife"); 
            // If a knife is detected, this chops it into the next chopped prefab 
            ChopIntoSlices(knife.GetComponentInChildren<Transform>()); 
        }
    }
    
    // Chops choppables into slices or halves 
    // Replaces this current prefab with the next cut and a new slice 
    public void ChopIntoSlices(Transform slicedAtPosition) { 
        Debug.Log("ChopIntoSlices called");
        if (giantKnifePowerup.IsActive() && !isHalvable) {
            // If giant knife active, salami is immediately cut into all its slices
            Vector3 chopDirection = Vector3.left;  
            float offset = 0; 
            for (int i = 0; i < numSlicesLeft; i++) {
                Vector3 posToInstantiate = slicedAtPosition.position - (chopDirection * offset) + 
                GameObject.FindGameObjectWithTag("Player").transform.forward*1f;
                Instantiate(slice, posToInstantiate, Quaternion.identity);
                offset += sliceSpacing; 
            }
        }
        else {
            Instantiate(nextCut, transform.position, transform.rotation); 
            Instantiate(slice, slicedAtPosition.position, Quaternion.identity); 
        }
        
        knife.GetComponent<Knife>().PlayChoppingSound();
        Destroy(transform.parent.gameObject);
        Destroy(gameObject); 
    }
}
