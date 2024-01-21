using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RaceStarterVersus : MonoBehaviour
{
    public Text startText; // Assign in Inspector
    public Text countdownText; // Assign in Inspector
    private UIManagerVersus uiManager; // Reference to your UIManager
    public BoatVersus playerOneBoat; 
    public BoatVersus playerTwoBoat;
    private bool raceStarted = false;

    void Start()
    {
        uiManager = FindObjectOfType<UIManagerVersus>(); // Find and assign the UIManager
        if (startText != null) 
        {
            startText.gameObject.SetActive(true); // Ensure the start text is visible at the beginning
        }
        if (countdownText != null) 
        {
            countdownText.gameObject.SetActive(false); // Ensure countdown text is hidden initially
        }
        if (playerOneBoat != null)
        {
            playerOneBoat.DisableMovement();
        }
        if (playerTwoBoat != null)
        {
            playerTwoBoat.DisableMovement();
        }
    }
    

    void Update()
    {
        if (!raceStarted && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(StartCountdown());
            if (startText != null) 
            {
                startText.gameObject.SetActive(false); // Hide the start text
            }
        }
    }

    IEnumerator StartCountdown()
    {
        countdownText.gameObject.SetActive(true);

        countdownText.text = "3";
        yield return new WaitForSeconds(1);

        countdownText.text = "2";
        yield return new WaitForSeconds(1);

        countdownText.text = "1";
        yield return new WaitForSeconds(1);

        countdownText.text = "GO!";
        yield return new WaitForSeconds(1);

        countdownText.gameObject.SetActive(false);
        StartRace();
        
    }

    void StartRace()
    {
        if (playerOneBoat != null)
        {
            playerOneBoat.EnableMovement();
        }
        if (playerTwoBoat != null)
        {
            playerTwoBoat.EnableMovement();
        }

        if (uiManager != null)
        {
            uiManager.StartTimer(); // Start the timer via UIManager
        }

        raceStarted = true;
    }
}