using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    private float direction;
    private bool hit;
    private float lifetime;

    private Animator anim;
    private BoxCollider2D boxCollider;

    private void Awake()
{
    anim = GetComponent<Animator>();
    if (anim == null)
    {
        Debug.LogError("Animator component is missing on " + gameObject.name);
    }
    boxCollider = GetComponent<BoxCollider2D>();
    if (boxCollider == null)
    {
        Debug.LogError("BoxCollider2D component missing on " + gameObject.name);
    }
}


    private void Update()
    {
        if (hit) return;

        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > 5) gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        boxCollider.enabled = false;
        anim.SetTrigger("explode");
    }

    public void SetDirection(float _direction)
    {
        // Debug to check if components are missing
        if (anim == null)
        {
            Debug.LogError("Animator is null in SetDirection for " + gameObject.name);
            return;
        }

        if (boxCollider == null)
        {
            Debug.LogError("BoxCollider2D is null in SetDirection for " + gameObject.name);
            return;
        }

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

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
