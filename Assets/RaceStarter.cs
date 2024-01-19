using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RaceStarter : MonoBehaviour
{
    public Text startText; // Assign in Inspector
    public Text countdownText; // Assign in Inspector
    private UIManager uiManager; // Reference to your UIManager
    private Boat playerBoat;
    private bool raceStarted = false;

    void Start()
    {
        uiManager = FindObjectOfType<UIManager>(); // Find and assign the UIManager
        if (startText != null) 
        {
            startText.gameObject.SetActive(true); // Ensure the start text is visible at the beginning
        }
        if (countdownText != null) 
        {
            countdownText.gameObject.SetActive(false); // Ensure countdown text is hidden initially
        }
        
        playerBoat = FindObjectOfType<Boat>();
        if (playerBoat != null)
        {
            playerBoat.DisableMovement();
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
        
        if (playerBoat != null)
        {
            playerBoat.EnableMovement();
        }
    }

    void StartRace()
    {
        if (uiManager != null)
        {
            uiManager.StartTimer(); // Start the timer via UIManager
        }
        raceStarted = true;
    }
}