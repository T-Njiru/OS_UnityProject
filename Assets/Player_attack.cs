using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 1f; // Cooldown time between attacks
    [SerializeField] private Transform firePoint;       // The fire point where fireballs will appear
    [SerializeField] private GameObject[] fireballs;    // Array to hold fireball game objects



    private Animator anim;
    private float cooldownTimer = Mathf.Infinity;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        // Check if left mouse button is held down, cooldown has passed
        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown)
            Attack();

        // Increase cooldown timer each frame
        cooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        // Trigger the attack animation
        anim.SetTrigger("attack");

        // Reset the cooldown timer
        cooldownTimer = 0;

        // Get a fireball from the pool (array of fireballs)
        int fireballIndex = FindFireball();

        Debug.Log("FirePoint position: " + firePoint.position);

<<<<<<< HEAD
        Debug.Log("firePoint position: " + firePoint.position);
        Debug.Log("Fireball index: " + fireballIndex + ", activeSelf: " + fireballs[fireballIndex].activeSelf);
=======
Debug.Log("firePoint position: " + firePoint.position);
Debug.Log("Fireball index: " + fireballIndex + ", activeSelf: " + fireballs[fireballIndex].activeSelf);
>>>>>>> aa4a99484d885c4c680e7bbbe9ec020987ae843c

        if (fireballIndex != -1)
        {
            // Set fireball's position to the fire point
            fireballs[fireballIndex].transform.position = firePoint.position;
            Debug.Log("Fireball spawned at: " + firePoint.position); // Log the position

            // Activate the fireball and set its direction
            fireballs[fireballIndex].SetActive(true);
            fireballs[fireballIndex].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));

            Debug.Log("Fireball activated: " + fireballs[fireballIndex].name); // Log fireball activation
        }
        else
        {
            Debug.LogWarning("No inactive fireballs available!"); // Warn if no fireballs are available in the pool
        }
    }

    private int FindFireball()
    {
        // Look for an inactive fireball in the pool (array)
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
            {
                Debug.Log("Found inactive fireball at index: " + i); // Log the found inactive fireball
                return i;  // Return the index of the first inactive fireball
            }
        }

        // If no inactive fireballs are found, return -1
        return -1;
    }
}