 using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OrderSubmission : MonoBehaviour
{
    // OrderSubmission script is attached to a tray that submits the order 
    // Once the bell is rung. 
    [SerializeField] private OrderManager orderManager;
    private bool isProcessing=false; 
    void Awake()
    {
        orderManager = GameObject.FindFirstObjectByType<OrderManager>(); 
    }


    // Bell will trigger StartProcessingOrder, which will only start once
    public void StartProcessingOrder() { 
        // Debug.Log("StartProcessingOrder called");
        if (isProcessing==false) {
            isProcessing=true; 
            StartCoroutine(TriggerDelayedProcessOrder()); 
        } 
    }
    
    public IEnumerator TriggerDelayedProcessOrder() { 
        yield return new WaitForSeconds(0.5f); 
        ProcessOrder(); 
    }

    void ProcessOrder() {
        Debug.Log("Process order called"); 
        Debug.Log("Calling get ingredients from tray");
        // Runs once button is pressed. Gets a list of ingredients from Plate 
        // List<Ingredient> ingredients = GetComponent<Tray>().GetIngredients();
        Dictionary<int, int> ingredients = GetComponent<Tray>().GetIngredientCounts(); 
        Debug.Log("Getting orders"); 
        List<Order> currentOrders = orderManager.GetOrders(); 
        if (ingredients.Count > 0 && currentOrders.Count > 0) {
        // Get a list of all currentOrders from the OrderManager. 
            // Iterate through list of currentOrders in order to find best match
            Order.MatchState bestMatchState = Order.MatchState.Mismatch; 
            Order bestMatchedOrder = currentOrders[0];
            foreach (Order order in currentOrders) {
                Debug.Log("Looking at order: " + order.GetName()); 
                Order.MatchState localMatch = order.VerifyOrder(ingredients);
                Debug.Log("Local match with order: " + order.GetName() + " is " + localMatch);
                if (localMatch == Order.MatchState.Perfect) {
                    bestMatchState = Order.MatchState.Perfect; 
                    bestMatchedOrder = order; 
                    break; 
                }
                else if (localMatch == Order.MatchState.Imperfect) {
                    bestMatchState = Order.MatchState.Imperfect; 
                    bestMatchedOrder = order; 
                }
            }
            Debug.Log("Finished comparing orders");
            Debug.Log("Best matched order: " + bestMatchedOrder.GetName()); 
            Debug.Log("Best match state: " + bestMatchState);
            bestMatchedOrder.ProcessOrder(bestMatchState); 
            
            // Destroy everything on the tray (the tray's children)
            foreach (Transform child in transform) {
                Destroy(child.gameObject); 
            }
        }
        Debug.Log("Processed order"); 
        isProcessing=false; 
    }

}


