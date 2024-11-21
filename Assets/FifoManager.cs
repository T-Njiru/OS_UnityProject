using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FIFOManager : MonoBehaviour
{
    public GameObject[] memoryFrames; // Assign square sprites for frames in the Inspector
    public TextMeshProUGUI pageFaultsText; // UI Text for displaying page faults
    public Button startButton, stepButton, resetButton;

    private Queue<int> frameQueue = new Queue<int>(); // FIFO structure for page replacement
    private int pageFaults = 0;
    private List<int> referenceString = new List<int> { 7, 0, 1, 2, 0, 3, 0, 4 }; // Example reference string
    private int currentIndex = 0;

    void Start()
    {
        // Button listeners
        startButton.onClick.AddListener(StartSimulation);
        stepButton.onClick.AddListener(StepSimulation);
        resetButton.onClick.AddListener(ResetSimulation);

        // Initialize UI
        ResetSimulation();
    }

    public void StartSimulation()
    {
        ResetSimulation(); // Ensure simulation starts fresh
        StartCoroutine(RunSimulation());
    }

    private IEnumerator RunSimulation()
    {
        // Loop through the reference string
        while (currentIndex < referenceString.Count)
        {
            StepSimulation();
            yield return new WaitForSeconds(1f); // Pause for visualization
        }
    }

    public void StepSimulation()
    {
        if (currentIndex >= referenceString.Count) return;

        int currentPage = referenceString[currentIndex];
        Debug.Log("Processing page: " + currentPage);

        if (!frameQueue.Contains(currentPage))
        {
            // Page fault occurred
            pageFaults++;
            if (frameQueue.Count >= memoryFrames.Length)
            {
                // Remove the oldest page
                int removedPage = frameQueue.Dequeue();
                AnimateRemoval(removedPage);
            }
            frameQueue.Enqueue(currentPage);
            AnimateAddition(currentPage);
        }
        else
        {
            HighlightPage(currentPage);
        }

        // Update UI
        pageFaultsText.text = "Page Faults: " + pageFaults;
        currentIndex++;
    }

    private void AnimateAddition(int page)
    {
        Debug.Log("Adding page: " + page);

        // Find the first available slot (empty frame) and add the page sprite
        for (int i = 0; i < memoryFrames.Length; i++)
        {
            if (memoryFrames[i].GetComponent<SpriteRenderer>().sprite == null)
            {
                // Assuming you have an array or dictionary of sprites for the pages
                Sprite pageSprite = GetPageSprite(page); // You can map pages to specific sprites
                memoryFrames[i].GetComponent<SpriteRenderer>().sprite = pageSprite;

                // Animation: Scale up, then back down to normal size and move it slightly
                StartCoroutine(AnimateFrameAdd(memoryFrames[i]));
                return;
            }
        }
    }

    private IEnumerator AnimateFrameAdd(GameObject frame)
    {
        Vector3 originalScale = frame.transform.localScale;
        Vector3 targetScale = originalScale * 1.2f; // Scale the frame up by 20%
        Vector3 originalPosition = frame.transform.position;

        // Move frame slightly upward to simulate an animation
        Vector3 targetPosition = originalPosition + new Vector3(0, 0.2f, 0);

        // Animate the frame's scale and position over 0.5 seconds
        float duration = 0.5f;
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            float lerpFactor = time / duration;

            frame.transform.localScale = Vector3.Lerp(originalScale, targetScale, lerpFactor);
            frame.transform.position = Vector3.Lerp(originalPosition, targetPosition, lerpFactor);

            yield return null;
        }

        // Return to normal size and position
        frame.transform.localScale = originalScale;
        frame.transform.position = originalPosition;
    }

    private void AnimateRemoval(int removedPage)
    {
        Debug.Log("Removing page: " + removedPage);

        // Find and clear the slot with the removed page
        for (int i = 0; i < memoryFrames.Length; i++)
        {
            if (memoryFrames[i].GetComponent<SpriteRenderer>().sprite != null &&
                memoryFrames[i].GetComponent<SpriteRenderer>().sprite.name == removedPage.ToString())
            {
                // Start shake animation and clear the sprite after shaking
                StartCoroutine(AnimateFrameRemove(memoryFrames[i]));
                return;
            }
        }
    }

    private IEnumerator AnimateFrameRemove(GameObject frame)
    {
        Vector3 originalPosition = frame.transform.position;
        float shakeDuration = 0.5f; // Duration of the shake
        float shakeAmount = 0.1f;  // How much the frame will shake
        float time = 0;

        // Shake the frame
        while (time < shakeDuration)
        {
            time += Time.deltaTime;
            float xOffset = Mathf.Sin(time * 20) * shakeAmount; // Fast sine wave for shaking effect
            frame.transform.position = originalPosition + new Vector3(xOffset, 0, 0);
            yield return null;
        }

        // Reset position and clear the sprite
        frame.transform.position = originalPosition;
        frame.GetComponent<SpriteRenderer>().sprite = null; // Clear the sprite
    }

    private void HighlightPage(int page)
    {
        Debug.Log("Page hit: " + page);

        // Highlight the frame containing the page
        for (int i = 0; i < memoryFrames.Length; i++)
        {
            if (memoryFrames[i].GetComponent<SpriteRenderer>().sprite != null &&
                memoryFrames[i].GetComponent<SpriteRenderer>().sprite.name == page.ToString())
            {
                StartCoroutine(AnimateFrameHighlight(memoryFrames[i]));
                return;
            }
        }
    }

    private IEnumerator AnimateFrameHighlight(GameObject frame)
    {
        // Save the original scale and color
        Vector3 originalScale = frame.transform.localScale;
        Color originalColor = frame.GetComponent<SpriteRenderer>().color;

        // Set new scale and color for the highlight
        Vector3 targetScale = originalScale * 1.1f; // Slightly enlarge
        Color targetColor = Color.yellow; // Highlight with yellow

        // Animate scaling and color change
        float duration = 0.3f;
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            float lerpFactor = time / duration;

            frame.transform.localScale = Vector3.Lerp(originalScale, targetScale, lerpFactor);
            frame.GetComponent<SpriteRenderer>().color = Color.Lerp(originalColor, targetColor, lerpFactor);

            yield return null;
        }

        // Reset scale and color back to normal
        frame.transform.localScale = originalScale;
        frame.GetComponent<SpriteRenderer>().color = originalColor;
    }

    public void ResetSimulation()
    {
        StopAllCoroutines(); // Stop ongoing simulations
        frameQueue.Clear();
        currentIndex = 0;
        pageFaults = 0;

        // Reset all frames (clear sprites)
        foreach (var frame in memoryFrames)
        {
            if (frame != null)
            {
                frame.GetComponent<SpriteRenderer>().sprite = null;
            }
        }

        pageFaultsText.text = "Page Faults: 0";
    }

    private Sprite GetPageSprite(int page)
    {
        // Example of returning a sprite based on the page number
        // You'll need to create a sprite mapping (an array or dictionary)
        // For now, just return a placeholder sprite based on the page number
        return Resources.Load<Sprite>("PageSprites/Page_" + page); // Assuming sprites are in Resources/Path/Page_7.png
    }
}
