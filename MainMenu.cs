using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlayGame()
    {
        // Load Level 1 (which has index 1 in the Build Settings)
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        // Quit the application
        Application.Quit();
        Debug.Log("Game has exited"); // This only works in the editor
    }
}
