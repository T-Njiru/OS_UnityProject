using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int damage = 10;

    private float direction;
    private bool hit;
    private float lifetime;
    private Animator anim;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (hit) return;

        // Move the fireball forward
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);

        // Handle fireball lifetime
        lifetime += Time.deltaTime;
        if (lifetime > 5f)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hit) return;

        if (collision.CompareTag("Enemy"))
        {
            hit = true;
            boxCollider.enabled = false;
            anim.SetTrigger("explode");

            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);

                // Notify the GameManager when an enemy is killed
                if (enemy.GetHealth() <= 0)
                {
                    FindObjectOfType<GameManager>().OnEnemyKilled(enemy);
                }
            }

            StartCoroutine(DeactivateAfterExplosion());
        }
    }

    private IEnumerator DeactivateAfterExplosion()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        gameObject.SetActive(false);
    }

    public void SetDirection(float _direction)
    {
        lifetime = 0;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
            localScaleX = -localScaleX;

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }
}
