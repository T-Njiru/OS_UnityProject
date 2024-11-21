using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int score;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // This method is called when an enemy is killed
    public void OnEnemyKilled(Enemy enemy)
    {
        // Example: Increase the score by a fixed value, or you can have a custom score logic per enemy
        int enemyScore = 10; // You can change this based on enemy type or other factors
        score += enemyScore;

        // Log the current score or update the UI with the new score
        Debug.Log("Enemy killed! Current score: " + score);

        // Optionally, update the UI or trigger further game logic
        // UIManager.instance.UpdateScore(score);
    }

    public int GetScore()
    {
        return score;
    }
}
