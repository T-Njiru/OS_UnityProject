using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : MonoBehaviour
{
    [SerializeField] private int health = 100;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            // Notify the GameManager about this enemy's death
            if (gameManager != null)
            {
                gameManager.OnEnemyKilled(this);
            }

            // Handle death (e.g., destroy the enemy)
            Destroy(gameObject);
        }
    }
    public int GetHealth()
    {
        return health;
    }

}

