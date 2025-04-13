using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlateTrigger : MonoBehaviour
{
    // Script attached to a child object of plate; handles parenting any ingredients in this collider to the plate
    private GameObject plate;
    private GameObject foodContainer;
    private Player player; 

    void Awake()
    {
        plate = transform.parent.gameObject; 
        foodContainer = GameObject.Find("FoodContainer");
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>(); 
    }
    void OnTriggerStay(Collider other)
    {
        // Debug.Log("OnPlateTrigger entered");
        // If this is an ingredient that isn't being held by the user 
        // and it's not already plated by this plate or another plate, parent it to the plate 
        Ingredient i = other.transform.GetComponent<Ingredient>(); 
        if (i != null && player.GetHeldItem() != other.gameObject
        && !i.IsPlated()) { 
            i.SetIsPlated(true);
            other.transform.parent = plate.transform; 
            StartCoroutine(SetToKinematic(other.GetComponent<Rigidbody>()));
        }
    }

    IEnumerator SetToKinematic(Rigidbody rb) { 
        float stillSeconds=0;
        if (rb==null) Debug.LogError("Rigidbody null from onplatetrigger");
        // Make sure ingredient has time to fall onto plate before being set to stuck on the plate 
        while (rb.velocity.magnitude > 0.01f) yield return null; // Velocity needs to slow down  
        while (rb.velocity.magnitude < 0.01f && player.GetHeldItem() != rb.gameObject && stillSeconds < 1) { 
            stillSeconds += Time.deltaTime;
            yield return null; 
        }
        // Make sure ingredient has had a chance to fall before being set to kinematic 
        // Note: should only set to kinematic if this rb's gameobject is still in this trigger (still parented to plate)

        if (player.GetHeldItem() != rb.gameObject && rb.transform.parent == plate.transform) rb.isKinematic = true;
        
    }

    void OnTriggerExit(Collider other)
    {
        // Debug.Log("OnPlateTrigger exit");
        
        if (other.transform.GetComponent<Ingredient>() != null && other.transform.parent == plate.gameObject) { 
            other.transform.parent = foodContainer.transform;
            other.transform.GetComponent<Ingredient>().SetIsPlated(false); 
        }
    }
}
