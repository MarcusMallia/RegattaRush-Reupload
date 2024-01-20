using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class UIManager : MonoBehaviour
{
    public Text speedText;         
    public Slider staminaSlider;   
    public Text timerText;          
    public GameObject gameOverPanel; 
    public Text finalTimeText;
    private float startTime;
    private bool timerRunning = false;
    private const string HighScoreKey = "HighScore";
    public Text highScoreText;
    private float finalRaceTime;
    
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

        // Initialize Game Over Panel to be hidden initially
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
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
        finalRaceTime = Time.time - startTime; // Calculate final race time

        if (timerText != null)
        {
            timerText.color = Color.yellow;
        }

        ShowGameOverScreen(finalRaceTime);
    }

    public void ShowGameOverScreen(float finalTime)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            UpdateFinalTimeText();
            
            HighScoreData highScoreData = LoadHighScore();
            if (finalTime < highScoreData.bestTime)
            {
                SaveHighScore(finalTime);
                highScoreData.bestTime = finalTime;
            }

            highScoreText.text = "High Score: " + highScoreData.bestTime.ToString("F2");
        }
    }
    
    public void SaveHighScore(float time)
    {
        HighScoreData data = new HighScoreData(time);
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(HighScoreKey, json);
        PlayerPrefs.Save();
    }

    public HighScoreData LoadHighScore()
    {
        string json = PlayerPrefs.GetString(HighScoreKey, JsonUtility.ToJson(new HighScoreData(float.MaxValue)));
        return JsonUtility.FromJson<HighScoreData>(json);
    }
    private void UpdateFinalTimeText()
    {
        if (finalTimeText != null && timerText != null)
        {
            finalTimeText.text = "Your Time: " + timerText.text; // Display the final time
        }
    }
   
    public void RestartGame()
    {
        SceneManager.LoadScene("Timetrial");
    }

    public void ReturnToMenu()
    {
        
        SceneManager.LoadScene("StartScene");
    }
}
