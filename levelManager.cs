using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management
using TMPro; // Required for TextMeshPro

public class levelManager : MonoBehaviour
{
    [Header("Next Level UI")]
    [SerializeField] private GameObject nextLevelUI; // Reference to the UI GameObject
    [SerializeField] private TextMeshProUGUI countDownText; // Reference to the countdown text
    [SerializeField] private TextMeshProUGUI completionMessageText; // Reference to the TextMeshProUGUI for the completion message
    [SerializeField] private GemCollector gemCollector; // Reference to the GemCollector

    private void Awake()
    {
        nextLevelUI.SetActive(false); // Initially hide the UI
        if (completionMessageText != null)
        {
            completionMessageText.gameObject.SetActive(false); // Hide the completion message at the start
        }
    }

    // This method can be called to load the next level
    public void ShowNextLevelUI()
    {
        nextLevelUI.SetActive(true); // Show the UI when called
    }

    // Show a completion message for the level
    public void ShowLevelCompletionMessage()
    {
        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        string message = "Level " + buildIndex + " complete!";

        // Display the message in the UI
        if (completionMessageText != null)
        {
            completionMessageText.text = message; // Update the completion message text
            completionMessageText.gameObject.SetActive(true); // Show the message
        }
        Debug.Log(message); // Optionally log to the console
        StartNextLevelCountdown();
    }

    // This method starts the countdown and loads the next level
    public void StartNextLevelCountdown()
    {
        if (gemCollector == null)
        {
            nextLevelUI.SetActive(false);
            Debug.LogError("GemCollector reference is not assigned in levelManager.");
            return; // Exit if gemCollector is not assigned
        }

        // Reset gems collected to zero
        StartCoroutine(NextLevelCountdown(3f)); // Start the countdown coroutine
    }

    private IEnumerator NextLevelCountdown(float countdownTime)
    {
        for (int i = (int)countdownTime; i > 0; i--)
        {
            countDownText.text = i.ToString(); // Update the countdown text
            yield return new WaitForSeconds(1f); // Wait for 1 second
        }
        countDownText.text = "GO!"; // Change text to "GO!"
        yield return new WaitForSeconds(1f); // Wait for 1 second
        gemCollector.ResetGems();
        if ((SceneManager.GetActiveScene().buildIndex + 1) == 4)
        {
            SceneManager.LoadScene(0); // Return to main menu or first scene
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Change to the next scene
        }
    }
}
