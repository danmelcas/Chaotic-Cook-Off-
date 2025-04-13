using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Stove : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource audioSource; 
    private int numIngredientsCooking;
    private Player player; 
    void Start()
    {
        audioSource = GetComponent<AudioSource>();    
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void StartCookingSFX() {
        // Debug.Log("StartCookingSFX triggered");
        audioSource.Play();
    }

    void StopCookingSFX() {
        audioSource.Stop(); 
    }
    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        //Play cooking SFX if an ingredient is dropped on the stove (not held) 
        if (other.GetComponent<Cookable>() != null) { 
            // Is ingredient 
            // Debug.Log("Cooking");
            numIngredientsCooking++;
            if (numIngredientsCooking == 1 && other.gameObject != player.GetHeldItem()) StartCookingSFX(); 
            // Debug.Log("+1 ingredient, numIngredients: " + numIngredientsCooking); 
        }    
    }


    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Cookable>() != null) { 
            // Is ingredient 
            numIngredientsCooking--;
            // Debug.Log("-1 ingredient, numIngredients: " + numIngredientsCooking);
            if (numIngredientsCooking == 0) StopCookingSFX();
        }    
    }


}
