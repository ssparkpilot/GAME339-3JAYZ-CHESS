using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private int bulletDamage = 1;

    [Header("Rotation")]
    [SerializeField] private float rotationOffset = 0f;
    [SerializeField] private float rotationSpeed = 2000f;

    private Transform target;

    public float DelayInSeconds = 10f;

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    public void SetRotationOffset(float offset)
    {
        rotationOffset = offset;
    }

    private void FixedUpdate()
    {
        if (!target)
        {
            Destroy(gameObject, DelayInSeconds);
            return;
        }

        Vector2 direction = (target.position - transform.position).normalized;

        rb.linearVelocity = direction * bulletSpeed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + rotationOffset;

        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Health health = other.gameObject.GetComponent<Health>();

        if (health != null)
        {
            health.TakeDamage(bulletDamage);
        }

        Destroy(gameObject);
    }
}