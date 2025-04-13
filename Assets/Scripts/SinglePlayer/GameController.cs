using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;
using System.Linq;
using UnityEngine.AI;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;
using UnityEngine.SceneManagement;

// Script controls the game state and has many UI and Game State related functions. 
public class GameController : MonoBehaviour
{
    
    private int playerPoints; 

    [SerializeField] private Powerup[] powerups; 
    private List<Powerup> usedPowerups; 
    public enum GameState { 
        Waiting,
        Playing,
        Paused,
        GameOver
    }
    public enum GameMode { 
        SinglePlayer,
        MultiplayerCoop,
        MultiplayerVersus
    }

    public static GameState currentGameState=GameState.Playing; 
    public static GameMode currentGameMode; 
    
    [SerializeField] private float powerupSpawnInterval; // A random powerup spawns every x seconds 
    private float timeElapsedSinceLastPowerup; 
    [SerializeField] private int gameLengthInSeconds; 
    private float remainingGameTime; 
    [Header("Goal points for stars")]
    [SerializeField] private int oneStarGoal; 
    [SerializeField] private int twoStarGoal; 
    [SerializeField] private int threeStarGoal;

    [Header("UI variables")]
    private string perfectMessage, imperfectMessage, mismatchMessage, missedOrderMessage;
    [SerializeField] private TextMeshProUGUI scorePopupText; 
    [SerializeField] private Animator scorePopupAnimator; 
    [SerializeField] private TextMeshProUGUI gameTimerText; 
    [SerializeField] private Animator powerupPopupAnimator; 
    [SerializeField] private TextMeshProUGUI powerupPopupText; 
    [SerializeField] private TextMeshProUGUI playerPointsText; 
    [SerializeField] private GameObject pauseScreen; 
    [SerializeField] private GameObject mainMenu; 
    [SerializeField] private GameObject tutorialScreen; 
    [SerializeField] private GameObject gameOverScreen; 
    [SerializeField] private GameObject timesUpScreen;
    [SerializeField] private GameObject crosshair; 
    [SerializeField] private GameObject[] starPrefabs; 
    [SerializeField] private TextMeshProUGUI pointsEarnedNumberText; 
    [SerializeField] private TextMeshProUGUI perfectDishesNumberText; 
    [SerializeField] private TextMeshProUGUI rawOrBurntDishesNumberText; 
    [SerializeField] private TextMeshProUGUI wrongOrdersNumberText; 
    [SerializeField] private TextMeshProUGUI missedOrdersNumberText; 
    [SerializeField] private TextMeshProUGUI oneStarNumber; 
    [SerializeField] private TextMeshProUGUI twoStarNumber; 
    [SerializeField] private TextMeshProUGUI threeStarNumber; 
    [SerializeField] private TextMeshProUGUI gameOverFeedbackText; 
    [SerializeField] private GameMusicAndSFX gameMusic; 
    [SerializeField] private AudioSource powerupAudioSource; 
    private string zeroStarMessage, oneStarMessage, twoStarMessage, threeStarMessage; 

    private int numPerfectDishes, numRawOrBurntDishes, numWrongOrders, numMissedOrders = 0; 
    [SerializeField] private AudioSource kachingSoundEffect; 

    void Awake()
    {
        // Initializes UI, sets game state vars
        Time.timeScale = 0f; 
        currentGameState=GameState.Waiting;
        playerPoints=0; 
        timeElapsedSinceLastPowerup = 0f;
        InitializeMessages();
        remainingGameTime = gameLengthInSeconds; 
        // Set all other screens inactive 
        pauseScreen.SetActive(false); 
        tutorialScreen.SetActive(false); 
        timesUpScreen.SetActive(false);
        crosshair.SetActive(false); 
        gameOverScreen.SetActive(false); 
        mainMenu.SetActive(true); 
        MakeCursorVisible(); 
        UnlockCursor(); 
        numPerfectDishes = 0; numRawOrBurntDishes = 0; numWrongOrders = 0; 
        usedPowerups = new List<Powerup>(); 
        gameMusic = GameObject.FindGameObjectWithTag("Music").GetComponent<GameMusicAndSFX>(); 
        
    }

    void Start()
    {
     gameMusic.PlayMainMenuMusic();   
    }

    // Activates a random powerup 
    void ActivateRandomPowerup() { 
        if (currentGameState != GameState.Playing) return; 
        Debug.Log("Activate random powerup called");
        int randIndex = Random.Range(0, powerups.Length);
        Powerup powerup = powerups[randIndex]; 
        if (usedPowerups.Contains(powerup)) { 
            // reroll again, just so user is less likely to get repeats  
            powerup = powerups[Random.Range(0, powerups.Length)]; 
        }
        powerup.ActivatePowerup(); 
        usedPowerups.Add(powerup); 

        // Animate the powerup popup 
        powerupPopupText.text = powerup.GetName() + "!";
        powerupPopupAnimator.SetTrigger("Powerup");
        powerupAudioSource.Play(); 
    }

    void InitializeMessages() { 
        perfectMessage = "Amazing!"; 
        imperfectMessage = "Inedible!";
        mismatchMessage = "Wrong dish!"; 
        missedOrderMessage = "Missing order!"; 
        zeroStarMessage = "Total Dish-aster!"; 
        oneStarMessage = "Not bad.";
        twoStarMessage = "Solid Service!";
        threeStarMessage = "Chef's Kiss!"; 
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsedSinceLastPowerup += Time.deltaTime; 
        if (timeElapsedSinceLastPowerup > powerupSpawnInterval) {
            timeElapsedSinceLastPowerup = 0f;
            ActivateRandomPowerup();
        }
        UpdateGameTimer(); 
        CheckPause(); 
    }

    public void AwardPlayerPoints(int points, Order.MatchState match) { 
        //Debug.Log("AwardPlayerPoints called, +" + points); 
        playerPoints += points; 
        //Debug.Log("Current score: " + playerPoints);
        if (match==Order.MatchState.Perfect) AnimatePlayerPoints(points, perfectMessage);
        if (match==Order.MatchState.Imperfect) AnimatePlayerPoints(points, imperfectMessage);
        if (match==Order.MatchState.Mismatch) AnimatePlayerPoints(points, mismatchMessage); 
        UpdatePointsText(); 
    }

    public void MissedOrder(int penalty) { 
        //Debug.Log("Missed an order!"); 
        playerPoints += penalty;
        //Debug.Log("Current score: " + playerPoints); 
        AnimatePlayerPoints(penalty, missedOrderMessage);
        UpdatePointsText(); 
    }

    // Game States 
    // Players can pause with P
    void CheckPause() { 
        if (Input.GetKeyDown(KeyCode.P) && currentGameState==GameState.Playing) { 
            Pause();
        }
    }

  

    public void Pause() { 
        Debug.Log("Game paused");
        currentGameState = GameState.Paused;
        Time.timeScale = 0f; 
        pauseScreen.SetActive(true); 
        crosshair.SetActive(false); 
        MakeCursorVisible();
        UnlockCursor(); 
    }
    
    public void Unpause() { 
        currentGameState = GameState.Playing;
        Time.timeScale = 1f; 
        pauseScreen.SetActive(false); 
        MakeCursorInvisible(); 
        crosshair.SetActive(true); 
        LockCursor(); 
    }

    public void ShowTutorial() { 
        mainMenu.SetActive(false); 
        tutorialScreen.SetActive(true); 
    }

    // Called when player hits "Start!" in tutorial 
    public void CloseTutorial() { 
        Debug.Log("Close Tutorial called"); 
        tutorialScreen.SetActive(false); 
        Time.timeScale = 1f; 
        MakeCursorInvisible(); 
        LockCursor(); 
        currentGameState = GameState.Playing;
        Debug.Log(currentGameState);
        crosshair.SetActive(true); 
        gameMusic.PlayInGameMusic();
    }

    // Called when player hits "single player" button 
    public void PlaySinglePlayerMode() { 
        Debug.Log("Called play single player mode");
        currentGameMode = GameMode.SinglePlayer;
        ShowTutorial(); 
    }

    // Called when player hits "multiplayer co-op" button 
    public void PlayMultiplayerCoop() { 
        Debug.Log("Called play multiplayer co op mode");

        currentGameMode = GameMode.MultiplayerCoop;
        ShowTutorial(); 
    }

    // Called when player hits "multiplayer versus" button 
    public void PlayMultiplayerVersus() { 
        Debug.Log("Called play multiplayer versus mode");

        currentGameMode = GameMode.MultiplayerVersus;
        ShowTutorial();
    }

    void GameOver() { 
        currentGameState = GameState.GameOver;
        StartCoroutine(TimesUp());     
            
    }

    public void ReturnToMainMenu() { 
        // Runs once player returns to main menu after finishing game. Handles 
        // setting of UI and game variables 
        crosshair.SetActive(false);
        pauseScreen.SetActive(false);
        timesUpScreen.SetActive(false);
        gameOverScreen.SetActive(false); 
        mainMenu.SetActive(true);
        ResetValues(); 
        currentGameState = GameState.Waiting;
        MakeCursorVisible();
        UnlockCursor(); 
    }

    void ResetValues() { 
        Time.timeScale = 1f; 
        playerPoints = 0; 
        numPerfectDishes = 0;
        numRawOrBurntDishes = 0;
        numWrongOrders = 0;
        numMissedOrders = 0; 
        usedPowerups = new List<Powerup>(); 
        remainingGameTime = gameLengthInSeconds; 
        //Reload scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
    }
    // UI methods 


    IEnumerator TimesUp() { 
        timesUpScreen.SetActive(true); 
        gameMusic.PlayTimesUpMusic(); 
        yield return new WaitForSeconds(3f);
        timesUpScreen.SetActive(false); 
        gameOverScreen.SetActive(true); 
        Time.timeScale = 0f; 
        MakeCursorVisible(); 
        UnlockCursor(); 
        
        CommonGameOverUI();
        if (currentGameMode == GameMode.SinglePlayer) { 
            GameOverUISinglePlayer(); 
        }
        else if (currentGameMode == GameMode.MultiplayerCoop) {
            GameOverUIMultiplayerCoop();
        }
        else if (currentGameMode == GameMode.MultiplayerVersus) { 
            GameOverUIMultiplayerVersus(); 
        }
    }

    void CommonGameOverUI() { 
        pointsEarnedNumberText.text = "$" + playerPoints.ToString();
        perfectDishesNumberText.text = numPerfectDishes.ToString();
        rawOrBurntDishesNumberText.text = numRawOrBurntDishes.ToString(); 
        wrongOrdersNumberText.text = numWrongOrders.ToString(); 
        missedOrdersNumberText.text = numMissedOrders.ToString(); 
    }

    void GameOverUISinglePlayer() { 
        StarSystemGameUI();
    }

    void GameOverUIMultiplayerCoop() { 
        StarSystemGameUI();
    }
    void GameOverUIMultiplayerVersus() { 
        // Unknown for now
    }
    
    void StarSystemGameUI() { 
        GameObject obj; 
        
        if (playerPoints >= threeStarGoal) { 
            obj = Instantiate(starPrefabs[3], gameOverScreen.transform);
            gameOverFeedbackText.text = threeStarMessage; 
        }
        else if (playerPoints >= twoStarGoal) { 
            obj = Instantiate(starPrefabs[2], gameOverScreen.transform);
            gameOverFeedbackText.text = twoStarMessage; 
        }
        else if (playerPoints >= oneStarGoal) { 
            obj = Instantiate(starPrefabs[1], gameOverScreen.transform);
            gameOverFeedbackText.text = oneStarMessage; 
        }
        else { 
            // Zero stars. 
            obj = Instantiate(starPrefabs[0], gameOverScreen.transform);
            gameOverFeedbackText.text = zeroStarMessage; 
        }
        obj.SetActive(true); 
        oneStarNumber.text = oneStarGoal.ToString(); 
        twoStarNumber.text = twoStarGoal.ToString();
        threeStarNumber.text = threeStarGoal.ToString(); 
        if (playerPoints >= oneStarGoal) gameMusic.PlayPositiveGameOverSFX(); 
        else gameMusic.PlayNegativeGameOverSFX(); 
    }
        
    void UpdateGameTimer() { 
        if (currentGameState == GameState.Playing) {
            remainingGameTime -= Time.deltaTime; 
            int mins = (int) remainingGameTime / 60; 
            int secs = (int) remainingGameTime % 60;
            if (secs < 10) gameTimerText.text = mins + ":0" + secs;
            else gameTimerText.text = mins + ":" + secs; 
        } 
        if (remainingGameTime <= 0 && currentGameState != GameState.GameOver) {
           GameOver(); 
        }
    }
    
    void AnimatePlayerPoints(int pointsAdded, string message) {
        string sign = "";
        if (pointsAdded > 0) sign="+";
        scorePopupText.text = message + "\n" + sign + pointsAdded;  
        scorePopupAnimator.SetTrigger("ScorePopup");
        if (pointsAdded > 0) kachingSoundEffect.Play();
    }

    void UpdatePointsText() { 
        playerPointsText.text = "$" + playerPoints; 
    }
    
    // Helpers
    void MakeCursorInvisible() { 
        Cursor.visible = false;
    }

    void MakeCursorVisible() { 
        Cursor.visible = true; 
    }

    void LockCursor() { 
        Cursor.lockState = CursorLockMode.Locked;
    }

    void UnlockCursor() { 
        Cursor.lockState = CursorLockMode.None;
    }

    // Public 
    public GameState GetGameState() {
        return currentGameState; 
    }

    public GameMode GetGameMode() { 
        return currentGameMode;
    }

    public void IncrementPerfectOrders() { 
        numPerfectDishes += 1;
    }

    public void IncrementRawOrBurntOrders() { 
        numRawOrBurntDishes += 1;
    }
    public void IncrementWrongOrders() { 
        numWrongOrders += 1; 
    }

    public void IncrementMissedOrders() { 
        numMissedOrders += 1; 
    }
}
