using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadTimeTrial()
    {
        SceneManager.LoadScene("TimeTrial"); 
    }
}

// Attach this script to an empty GameObject in your start scene.