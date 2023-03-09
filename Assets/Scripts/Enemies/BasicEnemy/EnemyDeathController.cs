using UnityEngine;

public class EnemyDeathController : MonoBehaviour
{
    public float knockbackForce = 500f;
    public float knockbackDuration = 0.5f;
    public float despawnDelay = 2f;

    private Rigidbody2D _rb;
    private Animator animator;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        // Apply knockback force
        Vector2 knockbackDirection = transform.position - PlayerController.Instance.transform.position;
        knockbackDirection.Normalize();
        _rb.AddForce(knockbackDirection * knockbackForce);
        // Start the death animation
        // animator.SetTrigger("deathKnockback");
        // Start the despawn timer
        Invoke("Despawn", despawnDelay);
    }

    private void Despawn()
    {
        // Start the death despawn animation
        // animator.SetTrigger("deathDespawn");
        // Destroy the game object after the animation finishes
        Invoke("DestroyGameObject", 1f);
    }

    private void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}