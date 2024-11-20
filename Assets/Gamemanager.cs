using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameObject[] enemies; // Array of all enemies in the scene
    public GameObject fireballPrefab; // Reference to the fireball prefab
    private float[] enemyTimes; // Array to store the time each enemy takes to die

    // Start is called before the first frame update
    void Start()
    {
        // Ensure enemies and fireballPrefab are properly assigned
        if (enemies == null || enemies.Length == 0 || fireballPrefab == null)
        {
            Debug.LogError("Enemies or Fireball Prefab not assigned in GameManager!");
            return;
        }

        // Initialize the array to store times
        enemyTimes = new float[enemies.Length];
        
        // Show enemy times (just for testing)
        for (int i = 0; i < enemies.Length; i++)
        {
            enemyTimes[i] = 0f; // Initialize all times to 0 for now
            Debug.Log("Enemy " + i + " initialized with time: " + enemyTimes[i]);
        }
        
        // Start simulating the firing
        FireAtEnemies();
    }

    void FireAtEnemies()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            // Let's simulate the fireball hitting an enemy
            // Normally, this would be a trigger from a collision or something similar
            StartCoroutine(SimulateFireballHit(i));
        }
    }

    private IEnumerator SimulateFireballHit(int enemyIndex)
    {
        float startTime = Time.time;
        
        // Simulate the fireball hit and damage over time (just for testing)
        Debug.Log("Firing at Enemy " + enemyIndex);
        yield return new WaitForSeconds(1f);  // Simulate a delay before damage is applied

        // Assuming the enemy is destroyed after a certain amount of time
        float timeTaken = Time.time - startTime;
        enemyTimes[enemyIndex] = timeTaken;  // Store the time taken to kill the enemy
        
        // Print the time taken for debugging
        Debug.Log("Enemy " + enemyIndex + " killed in: " + timeTaken + " seconds");

        // Display the total time taken for all enemies (optional)
        ShowTimes();
    }

    void ShowTimes()
    {
        Debug.Log("Showing Times: ");
        for (int i = 0; i < enemyTimes.Length; i++)
        {
            Debug.Log("Enemy " + i + " took " + enemyTimes[i] + " seconds.");
        }
    }
}
