using UnityEngine;
using TMPro;  // Ensure you have this line to use TextMeshPro

public class GemCollector : MonoBehaviour
{
    public TextMeshProUGUI gemsText;  // Reference to the UI text displaying the gem count
    private int gemsCollected = 0;    // Count of gems collected by the player

    private void Start()
    {
        UpdateGemText();  // Initialize the UI text when the game starts
    }

    public void CollectGem()
    {
        gemsCollected++;  // Increment the gem count
        UpdateGemText();  // Update the UI text to show the new count
    }

    private void UpdateGemText()
    {
        gemsText.text = gemsCollected.ToString();  // Update the UI text
    }

    public void ResetGems()  // Add this method to reset gems
    {
        gemsCollected = 0;    // Reset the gem count to zero
        UpdateGemText();      // Update the UI text to show the reset count
    }
}
