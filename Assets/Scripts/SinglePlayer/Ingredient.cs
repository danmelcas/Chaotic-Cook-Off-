using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Animations;

public class Ingredient : MonoBehaviour
{
    [SerializeField] private int ingredientID;
    private Rigidbody rb;  
    private GameObject defaultParent;
    private Player player; 
    private bool isPlated; 
  
    public bool IsPlated() { 
        return isPlated; 
    }

    public void SetIsPlated(bool set) { 
        isPlated = set;
    }
    void Awake()
    {
        isPlated = false;
        defaultParent = GameObject.Find("FoodContainer");
        transform.parent = defaultParent.transform; 
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (player==null) Debug.LogError("Player null from Ingredient");
        rb = GetComponent<Rigidbody>();
    }
    

    public int GetIngredientID() { 
        return ingredientID;
    }

    // void OnCollisionEnter(Collision collision)
    // {
    //     ParentToPlateCheck(collision);
    // }

    // // Checks if this ingredient should be parented to a plate 
    // void ParentToPlateCheck(Collision collision) { 
    //     var obj = collision.gameObject; 
    //     // If this ingredient collides with a plate, parent it to the plate and become kinematic ("stuck" to plate)
    //     if (obj.CompareTag("Plate")) { 
    //         obj.GetComponent<Rigidbody>().isKinematic = true; 
    //         transform.parent = obj.transform;
    //         rb.isKinematic = true;
    //         obj.GetComponent<Rigidbody>().isKinematic = false; 
    //     }
    //     // If this ingredient collides with an ingredient whose parent is a plate 
    //     else if (obj.GetComponent<Ingredient>() != null && obj.transform.parent != null && 
    //     obj.transform.parent.gameObject.CompareTag("Plate")
    //     && player.GetHeldItem() != gameObject) {
    //         // and the player is also not currently holding this ingredient  
    //         // parent it to the plate (the other ingredient's parent) and become kinematic 
    //         obj.transform.parent.gameObject.GetComponent<Rigidbody>().isKinematic = true; 
    //         transform.parent = obj.transform.parent; 
    //         rb.isKinematic = true;
    //         obj.transform.parent.gameObject.GetComponent<Rigidbody>().isKinematic = false; 

    //     }
    // }

    // void OnCollisionStay(Collision collision)
    // {
    //     // If a plate is not our parent, check if we should be parented to a plate 
    //     // Needs to be done because player could have been holding this ingredient when OnTriggerEnter was called 
    //     if (transform.parent != null && transform.parent.gameObject.CompareTag("Plate")==false) {
    //         ParentToPlateCheck(collision);
    //     }
    // }

}
