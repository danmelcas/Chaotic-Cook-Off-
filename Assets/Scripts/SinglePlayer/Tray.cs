using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tray : MonoBehaviour
{
    private bool hasBurntOrRawIngredients = false; 
    void OnTriggerEnter(Collider other)
    {
        // If the other is an ingredient or plate, parent it under tray
        if (other.GetComponent<Ingredient>() != null || 
        other.GetComponent<Plate>() != null) { 
            other.transform.parent = transform;
        }
    }

    // Important for penalties 
    public bool HasBurntOrRawIngredients() { 
        return hasBurntOrRawIngredients; 
    }

    // Called by OrderSubmission; tallies up ingredients and their counts 
    public Dictionary<int, int> GetIngredientCounts() { 
        // Get all the ingredients on the tray and return in a dictionary
        // where k = ingredient id and value = num of counts 
        Dictionary<int, int> ret = new Dictionary<int, int>();
        foreach (Ingredient i in transform.GetComponentsInChildren<Ingredient>()) { 

            // Also set flag for if there are burnt or raw ingredients in this dish 
            var cookableI = i.gameObject.GetComponent<Cookable>();
            bool foundBurntOrRaw = false; 
            if (cookableI != null) {
                if (cookableI.IsCooked() == false) foundBurntOrRaw = true;    
            }
            hasBurntOrRawIngredients = foundBurntOrRaw; 

            int ingredientID = i.GetIngredientID(); 
            int ingredientCount; 
            if (ret.TryGetValue(ingredientID, out ingredientCount)) {
            // this ingredient id is already in the dictionary, increment by 1 
                ret[ingredientID] = ret[ingredientID] + 1; 
            }
            else {
                ret[ingredientID] = 1; 
            }
        }
        return ret; 
    }
    
}
