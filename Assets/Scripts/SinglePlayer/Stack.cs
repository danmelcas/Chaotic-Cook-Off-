using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events; 

// Script attached to a GameObject representing a stack of objects(ex. stack of buns)
public class Stack : MonoBehaviour, IInteractable
{

    [SerializeField] private GameObject stackObject; 
    [SerializeField] private UnityEvent onInteraction; 
    [SerializeField] private UnityEvent onHover; 
    [SerializeField] private UnityEvent onHoverExit; 
    private Outline outline; 
    void Awake() { 
        if (stackObject == null) Debug.Log("Stack object not set"); 
        outline = GetComponent<Outline>(); 
        if (outline == null) Debug.Log("Outline is null on stack"); 
        DisableOutline();
        // if (onInteraction == null) onInteraction.AddListener(GetFromStack); 
        // if (onHover == null) onHover.AddListener(EnableOutline); 
        // if (onHoverExit == null) onHoverExit.AddListener(DisableOutline);
    }

    public void GetFromStack() { 
        // Debug.Log("Get from stack called");
        GameObject obj = Instantiate(stackObject); 
        obj.AddComponent<Rigidbody>();
        if (obj.GetComponent<Rigidbody>() == null) Debug.Log("Missing Rigidbody");
        if (obj.GetComponent<Collider>() == null) obj.AddComponent<BoxCollider>();
        obj.GetOrAddComponent<Outline>();
        obj.GetOrAddComponent<BasicInteraction>();
        if (obj.GetComponent<BasicInteraction>() == null) Debug.Log("Basic interaction is null");
        obj.GetComponent<BasicInteraction>().SetPickupEvents(); 
        obj.GetComponent<BasicInteraction>().PickUpItem(); 
    }

    public void OnHover()
    {
        onHover.Invoke(); 
    }

    public void OnHoverExit()
    {
        onHoverExit.Invoke(); 
    }

    public void OnInteraction()
    {
        onInteraction.Invoke(); 
    }

    public void EnableOutline() { 
        outline.enabled=true; 
    }

    public void DisableOutline() { 
        outline.enabled = false;
    }
}
