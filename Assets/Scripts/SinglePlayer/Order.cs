using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI; 

public class Order : MonoBehaviour
{
    // Represents an order, has logic relating to comparing a submitted order to this order 
    [SerializeField] private List<int> ingredientIDs; 
    private Dictionary<int, int> ingredientCounts; 
    [SerializeField] private string orderName; 
    // Links to a prefab of the order canvas for instantiation 
    private float orderStartTime; 
    [SerializeField] private float orderLifetime; // Maximum amount of seconds user can spend before order fails and disappears
    [SerializeField] private int baseAward;

    [Header("Bonuses for completing the order swiftly")]
    [SerializeField] private int fastestBonus; 
    [SerializeField] private int moderateBonus;
    [SerializeField] private int minimalBonus;  

    [SerializeField] private int rawOrBurntPenalty;

    [SerializeField] private GameController gameController; 
    [SerializeField] private OrderManager orderManager; 

    [SerializeField] private Image timerImage; 
    private TimeFreezePowerup timeFreezePowerup; 
    private DoublePointsPowerup doublePointsPowerup; 
    // Enum for representing how well a list of ingredients matches 
    // this order's ingredientIDs (taking into account 
    // cookStates like raw, burnt, or cooked)
    public enum MatchState {
        Perfect, 
        Imperfect, // Ingredients match but some ingredients are raw or burnt
        Mismatch // Ingredients don't match 
    }
    void Awake()
    {
        orderStartTime = Time.time; 
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>(); 
        if (gameController==null) Debug.Log("Game controller found to be null"); 
        orderManager = GameObject.FindFirstObjectByType<OrderManager>(); 
        if (orderManager==null) Debug.Log("Order manager is null"); 
        timerImage = transform.Find("TimerImage").GetComponent<Image>();
        if (timerImage==null) Debug.Log("Timer image is null");  
        timerImage.color = Color.green; 
        timerImage.fillAmount = 1f;
        InitializeIngredientDict();
        timeFreezePowerup = FindFirstObjectByType<TimeFreezePowerup>(); 
        if (timeFreezePowerup == null) Debug.LogError("Time Freeze Powerup is null"); 
        doublePointsPowerup = FindFirstObjectByType<DoublePointsPowerup>(); 
        if (doublePointsPowerup == null) Debug.LogError("Double points powerup is null"); 
    }

    void InitializeIngredientDict() { 
        Debug.Log("InitializeIngredientIDs called");
        ingredientCounts = new Dictionary<int, int>(); 
        foreach (int i in ingredientIDs) { 
            int localval; 
            if (ingredientCounts.TryGetValue(i, out localval)) {
            // this ingredient id is already in the dictionary, increment by 1 
                ingredientCounts[i] = ingredientCounts[i] + 1; 
            }
            else {
                ingredientCounts[i] = 1; 
            }
        }
    }

    void PrintDictionary(Dictionary<int, int> dict) { 
        Debug.Log("Dictionary: ");
        foreach (var kvp in dict) {
            Debug.Log("ingredient id: " + kvp.Key + " , count: "  + kvp.Value);
        }
    }

    void Update()
    {
        CheckOrderExpiration(); 
        UpdateTimer(); 
    }

    public string GetName()
    {
        return orderName; 
    }

    // Function verifies correctness of order and awards points. 
    // Success = passed in parameter as outcome of verify (called by OrderSubmission)
    public void ProcessOrder(MatchState match) { 
        Debug.Log("Process order called for order " + GetName());
        Debug.Log("MatchState: " + match); 
        float completedTime = Time.time; 
        int bonus = 0; 
        int multiplier = 1; 
        if (doublePointsPowerup.IsActive()) multiplier = 2; 
        // If there's at least some match, 
        
        if (match != MatchState.Mismatch) { 
            // Compares time taken (completedTime - orderStartTime) with how many seconds the player has to complete the order
            float proportion = (completedTime-orderStartTime) / orderLifetime;
            if (proportion > 0.9) {
            // No bonus; took too long 
                bonus=0; 
            }  
            else if (proportion > 0.66) {
            // "Red" area; took a long time  
                bonus = minimalBonus;
            }
            else if (proportion > 0.33) { 
                bonus = moderateBonus;
            // "Yellow" area; took a medium amount of time 
            }
            else { 
            // "Green" area, finished swiftly
                bonus = fastestBonus;
            }
            if (match == MatchState.Perfect) {
                gameController.AwardPlayerPoints((baseAward+bonus) * multiplier, match);
                gameController.IncrementPerfectOrders();
            }
            else { 
                // Double Rewards doubles rewards not penalties 
                if (baseAward + rawOrBurntPenalty < 0) multiplier = 1; 
                // Raw or burnt food do not receive a time bonus 
                gameController.AwardPlayerPoints((baseAward+rawOrBurntPenalty) * multiplier, match); 
                gameController.IncrementRawOrBurntOrders(); 
            }
            orderManager.DeleteOrder(this); 
        }
        // If there's no match at all, this is a wrong order
        else { 
            // Penalize player and retain this order in the list. 
            gameController.AwardPlayerPoints(baseAward*-1, match);
            gameController.IncrementWrongOrders();  
        }
    }

    // Checks if order is expired, and if so, deletes self
    public void CheckOrderExpiration() { 
        
        if (timerImage.fillAmount <= 0) { 
            gameController.MissedOrder(baseAward*-1); 
            gameController.IncrementMissedOrders(); 
            orderManager.DeleteOrder(this);  
        }
    }
    
    public float GetFillAmount() { 
        return timerImage.fillAmount; 
    }

    public void SetFillAmount(float new_fill_amount) { 
        timerImage.fillAmount = new_fill_amount; 
    }   

    public MatchState VerifyOrder(Dictionary<int, int> otherIngredientCounts) {
        Debug.Log("Verify order entered"); 
        Debug.Log(GetName() + "order compared, expected: ");
        PrintDictionary(ingredientCounts); 
        Debug.Log("Received: ");
        PrintDictionary(otherIngredientCounts); 

        var expectedCounts = ingredientCounts; 
        // Iterate through foreign keys and see if ALL match expected 
            // Must have NO unexpected keys AND keys must match COUNT 
        bool isMatch = true; 
        // number of ingredients must match 
        if (expectedCounts.Count != otherIngredientCounts.Count) return MatchState.Mismatch; 
        foreach (var other_kvp in otherIngredientCounts) { 
            int localval;
            if (expectedCounts.TryGetValue(other_kvp.Key, out localval)) { 
                if (expectedCounts[other_kvp.Key] != other_kvp.Value) {
                    isMatch = false; 
                    break; 
                }
            }
            else {
                // Has an unexpected value, no longer a match 
                isMatch = false; 
                break; 
            }
        }

        if (!isMatch) return MatchState.Mismatch; 
        // Then, check if there are any burnt or bad ingredients 
        
        bool hasBurntOrRaw = FindFirstObjectByType<Tray>().HasBurntOrRawIngredients(); 
        if (hasBurntOrRaw) return MatchState.Imperfect; 
        return MatchState.Perfect; 

        
    }

    void UpdateTimer() { 
        if (timeFreezePowerup.IsActive()==false) timerImage.fillAmount -= Time.deltaTime/orderLifetime;    
        if (timerImage.fillAmount < .66) { 
            // Change to yellow 
            timerImage.color = Color.yellow;
        }
        else if (timerImage.fillAmount < .33) {
            // Change to red 
            timerImage.color = Color.red; 
        }
    }
    
}
