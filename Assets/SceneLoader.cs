using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject instructionsPanel;
    
    void Start()
    {
        // Hide instructions panel on start
        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(false);
        }
    }
    public void LoadTimeTrial()
    {
        SceneManager.LoadScene("TimeTrial"); 
        
    }
    
    public void LoadVersus()
    {
        SceneManager.LoadScene("Versus"); 
        
    }
    public void ToggleInstructions()
    {
        if (instructionsPanel != null)
        {
            bool isActive = instructionsPanel.activeSelf;
            instructionsPanel.SetActive(!isActive);
        }
    }
   
}

