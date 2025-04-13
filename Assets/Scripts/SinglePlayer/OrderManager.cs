using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    // Manages and tracks orders and their addition, deletion (when order is completed or expires)
    private List<Order> currentOrders;
    [SerializeField] private List<GameObject> orderPrefabs; 
    [SerializeField] private GameObject orderRow; 
    [SerializeField] private float orderCooldownTime; 
    [SerializeField] private int maxOrders; // The maximum number of orders at a time 
    [SerializeField] private GameController gameController; 
    void Awake()
    {
        currentOrders = new List<Order>();     
        orderRow = GameObject.Find("OrderRow");
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>(); 
        StartCoroutine(AddOrder());
    }

    IEnumerator AddOrder() { 
        Debug.Log("Add order coroutine called");
        while (true) {
            if (gameController.GetGameState() != GameController.GameState.Playing) yield return null; 
            else { 
                int randIndex = Random.Range(0, orderPrefabs.Count);
                // Add if we can 
                if (currentOrders.Count < maxOrders) {
                // Instantiate a new order of random type as a child of the orderRow
                    GameObject obj = Instantiate(orderPrefabs[randIndex], orderRow.transform); 
                    // Add this order 
                    currentOrders.Add(obj.GetComponent<Order>()); 
                    if (obj.GetComponent<Order>() == null) Debug.Log("Failed to access order script in orderPrefab"); 
                    yield return new WaitForSeconds(orderCooldownTime); 
                }
                // Otherwise don't 
                else yield return new WaitForSeconds(1f); 
            }    
        }
    }

    public void DeleteOrder(Order order) { 
        currentOrders.Remove(order); 
        Destroy(order.gameObject); 
    }
    public List<Order> GetOrders() { 
        return currentOrders; 
    }

    public void RefreshOrders() { 
        int num_orders = currentOrders.Count; 
        // clear all orders 
        List<float> previous_timers = new List<float>();
        for (int i = num_orders - 1; i >= 0; i--) {
            previous_timers.Add(currentOrders[i].GetFillAmount());
            Destroy(currentOrders[i].gameObject);
            currentOrders.RemoveAt(i); 
        }
        for (int i = 0; i < num_orders; i++) { 
            int randIndex = Random.Range(0, orderPrefabs.Count);
            GameObject obj = Instantiate(orderPrefabs[randIndex], orderRow.transform); 
            currentOrders.Add(obj.GetComponent<Order>()); 
            obj.GetComponent<Order>().SetFillAmount(previous_timers[num_orders-1-i]); 
            if (obj.GetComponent<Order>() == null) Debug.Log("Failed to access order script in orderPrefab"); 
        }
        
    }
}
