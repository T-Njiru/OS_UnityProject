using UnityEngine;
using System.Collections;  // Add this line for IEnumerator and coroutines

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    private float direction;
    private bool hit;
    private float lifetime;

    private Animator anim;
    private BoxCollider2D boxCollider;

    [SerializeField] private int damage = 10;  // Define the damage variable
    [SerializeField] private float travelDistance = 20f;  // Define the distance the fireball must travel before exploding

    private Vector3 startPosition;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogError("Animator is null in Awake for " + gameObject.name);
        }

        boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider == null)
        {
            Debug.LogError("BoxCollider2D is missing on " + gameObject.name);
        }
    }

    private void Start()
    {
        startPosition = transform.position; // Record the initial position
    }

    private void Update()
    {
        if (hit) return;

        // Move the fireball forward
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);

        // Handle fireball lifetime (optional: set a maximum lifetime to avoid infinite existence)
        lifetime += Time.deltaTime;
        if (lifetime > 5f) gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug to track what is colliding with the fireball
        Debug.Log("Fireball collided with: " + collision.gameObject.name + " (Tag: " + collision.tag + ")");

        if (hit) return;

        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Fireball hit an enemy: " + collision.gameObject.name);  // Debugging when the fireball hits an enemy

            hit = true;
            boxCollider.enabled = false;  // Disable the collider once the fireball hits

            anim.SetTrigger("explode");  // Trigger the explosion animation
            Debug.Log("Explosion animation triggered!");  // Debugging explosion trigger

            // Apply damage to the enemy
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                Debug.Log("Applying damage to enemy: " + enemy.name);  // Debugging damage application
                enemy.TakeDamage(damage);  // Apply damage to the enemy
            }

            // Deactivate the fireball after the explosion animation is triggered
            StartCoroutine(DeactivateAfterExplosion());
        }
        else
        {
            Debug.Log("Fireball hit something that's not an enemy: " + collision.gameObject.name);  // Debugging non-enemy collisions
        }
    }

    // Coroutine to wait for the explosion animation to finish before deactivating the fireball
    private IEnumerator DeactivateAfterExplosion()
    {
        // Wait for the explosion animation to finish (adjust time according to your animation length)
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        Debug.Log("Deactivating fireball after explosion");  // Debugging fireball deactivation
        gameObject.SetActive(false);
    }

    public void SetDirection(float _direction)
    {
        lifetime = 0;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;

        if (anim == null)
        {
            Debug.LogError("Animator is null in SetDirection for " + gameObject.name);
            return; // Stop execution if Animator is null
        }

        boxCollider.enabled = true;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
            localScaleX = -localScaleX;

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
