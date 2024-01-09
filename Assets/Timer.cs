using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText; // Assign this in the inspector to your UI Text element
    private float startTime;
    private bool isRunning = true;

    void Start()
    {
        // Record the start time when the game begins
        startTime = Time.time;
    }

    void Update()
    {
        if (isRunning)
        {
            float t = Time.time - startTime;
            string minutes = ((int)t / 60).ToString();
            string seconds = (t % 60).ToString("f2");
            timerText.text = minutes + ":" + seconds;
        }
    }

    public void Finish()
    {
        // Call this method when the boat crosses the finish line
        isRunning = false;
        timerText.color = Color.yellow; // Change the timer color to indicate completion
    }
}