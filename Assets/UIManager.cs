using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text speedText; // Assign in Inspector
    public Slider staminaSlider; // Assign in Inspector
    public Text timerText; // Assign in Inspector

    private float startTime;
    private bool timerRunning = false;

    void OnEnable()
    {
        Debug.Log("UIManager is subscribing to OnBoatStateUpdated");
        Boat.OnBoatStateUpdated += UpdateBoatUI;
    }

    void OnDisable()
    {
        Debug.Log("UIManager is unsubscribing from OnBoatStateUpdated");
        Boat.OnBoatStateUpdated -= UpdateBoatUI;
    }

    void Start()
    {
 
        if (staminaSlider != null)
        {
            staminaSlider.maxValue = 100; 
            staminaSlider.value = 100;
        }
        
        //StartTimer();  
    }

    void Update()
    {
        if (timerRunning)
        {
            UpdateTimer();
        }
    }

    void UpdateBoatUI(float speed, float stamina)
    {
        Debug.Log("Updating UI - Speed: " + speed + ", Stamina: " + stamina);
        if (speedText != null)
        {
            speedText.text = "Speed: " + (speed * 10).ToString("0");
        }

        if (staminaSlider != null)
        {
            staminaSlider.value = stamina;
        }
    }

    public void StartTimer()
    {
        startTime = Time.time;
        timerRunning = true;
    }

    public void UpdateTimer()
    {
        float t = Time.time - startTime;
        string minutes = ((int)t / 60).ToString();
        string seconds = (t % 60).ToString("f2");

        if (timerText != null)
        {
            timerText.text = minutes + ":" + seconds;
        }
    }

    public void FinishTimer()
    {
        timerRunning = false;
        if (timerText != null)
        {
            timerText.color = Color.yellow; // Example of changing the text color
        }
    }
    
}