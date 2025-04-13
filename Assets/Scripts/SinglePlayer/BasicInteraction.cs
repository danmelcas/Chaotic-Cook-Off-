using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR;

public class BasicInteraction : MonoBehaviour, IInteractable
{
    // Script that implements the IInteractable interface and has UnityEvents for 
    // when a player presses E while looking at the object (onInteraction),
    // when a player is looking at the object (onHover),
    // and when a player stops looking at the object (onHoverExit)
    [SerializeField] private UnityEvent onInteraction;
    [SerializeField] private UnityEvent onHover; 
    [SerializeField] private UnityEvent onHoverExit; 
    [SerializeField] private UnityEvent onPickup; 
    [SerializeField] private UnityEvent onDrop; 
    [SerializeField] private Outline outline; // Outline of this object 
    [SerializeField] private Player player; 
    [SerializeField] private GameObject playerRightHand; 
    private bool isHeld=false; 
    [SerializeField] private GameObject heldItemContainer; 
    [SerializeField] private Rigidbody rb;  
    [SerializeField] private Vector3 customRotation; // custom Rotation for when objects are picked up, defined by each object or left blank 
    [SerializeField] private bool hasCustomRotation;
    [SerializeField] private Vector3 customOffset; // Init as 0 but custom objects like fire extinguishers want different offsets
    
    
    // Helper method to set the unity events of objects that can be picked up 
    public void SetPickupEvents() { 
        onInteraction.AddListener(PickUpItem); 
        onHover.AddListener(EnableOutline);
        onHoverExit.AddListener(DisableOutline); 
    }

    // Initializes important references
    void Awake() { 
        outline = GetComponent<Outline>(); // gets outline component attached to this interactable gameObject
        if (outline == null) Debug.Log("Outline missing"); 
        DisableOutline(); 
        
        player = FindAnyObjectByType<Player>(); 
        if (player==null) Debug.Log("Player not found"); 

        //Logs if this gameobject has no rigid body (desirable for UI interactions, 
        // but a warning for interactable items with intended physics properties)
        rb = transform.GetComponent<Rigidbody>();
        if (rb == null) Debug.Log("No rigid body found on this interactable");

        heldItemContainer = GameObject.FindGameObjectWithTag("HeldItemContainer");
        if (heldItemContainer==null) Debug.Log("Held item container not found"); 

        //Initialize events if not assigned to inspector (listeners may be added later, ex in Stack getFromStack)
        if (onInteraction == null) {
            onInteraction = new UnityEvent();
            Debug.Log("On interaction is null"); 
        }
        if (onHover == null) { 
            onHover = new UnityEvent();
            Debug.Log("On hover is null");
        }
        if (onHoverExit == null) {
            onHoverExit = new UnityEvent();
            Debug.Log("On hover exit is null");
        }
    }

    // WhileHeld handles moving objects around while the player looks around 
    void LateUpdate()
    {
        if (isHeld) { 
            WhileHeld(); 
        }
    }

    // Calls oninteraction event
    public void OnInteraction()
    {
        onInteraction.Invoke(); 
    }

    // Calls set onHover event
    public void OnHover() {
        onHover.Invoke(); 
     //  Debug.Log("On hover event invoked"); 
    }

    // Calls set onHoverExit event
    public void OnHoverExit() { 
        onHoverExit.Invoke();
    //   Debug.Log("On hover exit event invoked"); 
    }

    // Below are common methods and interactions for interactable objects  
    public void EnableOutline() { 
    //    Debug.Log("Enable outline called"); 
        outline.enabled=true; 
    }

    public void DisableOutline() { 
    //    Debug.Log("Disable outline called");
        outline.enabled = false;
    }


    // Method called when object is picked up; sets it to a container and makes it kinematic (so it doesn't fall or responds to 
    // external forces)
    public void PickUpItem() { 
        onPickup.Invoke(); 
        if (gameObject.CompareTag("Knife")) {
            GetComponent<Knife>().PickUpKnife();
        }
        else {
            transform.rotation = Quaternion.identity; 
            // if (hasCustomRotation) transform.rotation = customRotation; 
        }
        isHeld = true; 
        rb.isKinematic = true;
        rb.freezeRotation = true; 
        player.PickUpItemAnim(this.gameObject); 
        transform.SetParent(heldItemContainer.transform);
        // Line below makes sure object can't push on the player 
        Physics.IgnoreCollision(this.GetComponent<Collider>(), player.GetComponent<Collider>(), true); 
        
        if (gameObject.CompareTag("Plate")) gameObject.GetComponent<Plate>().IgnoreCollisionsWithIngredients(); 
        Ingredient i = GetComponent<Ingredient>(); 
        if (i != null) i.SetIsPlated(false); 
        // transform.parent = heldItemContainer.transform; 
    }

    // Called when player drops item 
    public void DropItem() { 
        isHeld=false;
        rb.isKinematic = false; 
        rb.freezeRotation = false; 
        Physics.IgnoreCollision(this.GetComponent<Collider>(), player.GetComponent<Collider>(), false); 
        transform.parent = null; 
        if (gameObject.CompareTag("Knife")) GetComponent<Knife>().DropKnife(); 
        if (gameObject.CompareTag("Plate")) gameObject.GetComponent<Plate>().ResetCollisionsWithIngredients(); 
        onDrop.Invoke(); 
    }

    public void WhileHeld() { 
        // continually set position equal to item container 
        // transform.position = Camera.main.transform.position + Camera.main.transform.forward * 1.5f;
        // transform.position = heldItemContainer.transform.position + customOffset; 
        transform.position = heldItemContainer.transform.TransformPoint(customOffset);
        if (hasCustomRotation) transform.rotation = heldItemContainer.transform.rotation * Quaternion.Euler(customRotation);
        // Sets it to a custom rotation if defined (Ex. for fire extinguisher)
        // rb.MovePosition(heldItemContainer.transform.position); 
    }

    // Method that can be used for UnityEvents
    public void PlaySFX() { 
        if (GetComponent<AudioSource>() == null) {
            Debug.LogWarning("Tried to play SFX but could not find audio source on object " + transform.name);
        }
        else GetComponent<AudioSource>().Play(); 
    }

    public bool IsHeld() { return isHeld; }
}
