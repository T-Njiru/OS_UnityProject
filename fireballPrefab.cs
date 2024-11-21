using UnityEngine;
using System.Collections;  // For IEnumerator and coroutines

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
        Debug.Log("Fireball initialized at position: " + startPosition);
    }

    private void Update()
    {
        if (hit) return;

        // Move the fireball forward
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);
        Debug.Log("Fireball position: " + transform.position + ", Speed: " + movementSpeed);

        // Handle fireball lifetime
        lifetime += Time.deltaTime;
        if (lifetime > 5f)
        {
            Debug.Log("Fireball lifetime exceeded. Deactivating.");
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Fireball collided with: " + collision.gameObject.name + " (Tag: " + collision.tag + ")");

        if (hit) return;

        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Fireball hit an enemy: " + collision.gameObject.name);

            hit = true;
            boxCollider.enabled = false;

            anim.SetTrigger("explode");
            Debug.Log("Explosion animation triggered!");

            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                Debug.Log("Applying damage (" + damage + ") to enemy: " + enemy.name);
                enemy.TakeDamage(damage);
            }

            StartCoroutine(DeactivateAfterExplosion());
        }
        else
        {
            Debug.Log("Fireball hit a non-enemy object: " + collision.gameObject.name);
        }
    }

    private IEnumerator DeactivateAfterExplosion()
    {
        Debug.Log("Waiting for explosion animation to finish.");
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        Debug.Log("Deactivating fireball after explosion animation.");
        gameObject.SetActive(false);
    }

    public void SetDirection(float _direction)
    {
        Debug.Log("Setting direction for: " + gameObject.name);
        
        lifetime = 0;
        direction = _direction;
        Debug.Log("Before activation: Fireball activeSelf: " + gameObject.activeSelf);
        gameObject.SetActive(true); // Ensure the fireball is activated
        Debug.Log("After activation: Fireball activeSelf: " + gameObject.activeSelf);
        
        hit = false;

        Debug.Log("Fireball activated with direction: " + _direction);

        if (anim == null)
        {
            Debug.LogError("Animator is null in SetDirection for " + gameObject.name);
            return;
        }

        boxCollider.enabled = true;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
            localScaleX = -localScaleX;

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
        Debug.Log("Fireball scale updated for direction: " + transform.localScale);
    }

    private void Deactivate()
    {
        Debug.Log("Deactivating fireball manually.");
        gameObject.SetActive(false);
    }
}
