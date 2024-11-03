using UnityEngine;
using System.Collections;

public class GEm : MonoBehaviour
{
    public float fadeDuration = 1.5f;  // Duration of the fade-out effect

    private void OnTriggerEnter(Collider other)  // Use OnTriggerEnter for 3D
    {
        if (other.CompareTag("Player"))  // Ensure the player has the "Player" tag
        {
            // Find the Collector component on the player and update the gem count
            GemCollector collector = other.GetComponent<GemCollector>();
            if (collector != null)
            {
                collector.CollectGem();  // Increment gem count in the UI
            }

            // Start fading out the gem
            StartCoroutine(FadeOutAndDestroy());
        }
    }

    private IEnumerator FadeOutAndDestroy()
    {
        Renderer renderer = GetComponent<Renderer>();
        Color originalColor = renderer.material.color;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);  // Fade alpha from 1 to 0
            renderer.material.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        Destroy(gameObject);  // Destroy the gem after it fades out
    }
}
