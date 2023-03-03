using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    // [SerializeField] private float damage = 10f;
    [SerializeField] private float attackDuration = 0.5f;

    private bool isAttacking = false;
    private Vector2 direction;
    private bool isClone;

    private void Start()
    {
        if (isClone)
        {
            Destroy(gameObject, GetAttackDuration());
        }
    }

    public void SetDirection(Vector2 direction)
    {
        this.direction = direction.normalized;
    }

    public float GetAttackDuration()
    {
        return attackDuration;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isAttacking && collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player has been hit.");
            // PlayerController.Instance.TakeDamage(damage);
        }
    }

    public void Attack()
    {
        isAttacking = true;
        StartCoroutine(StopAttackCoroutine());
    }

    private IEnumerator StopAttackCoroutine()
    {
        yield return new WaitForSeconds(GetAttackDuration());
        isAttacking = false;
    }

    public void SetIsClone(bool isClone)
    {
        this.isClone = isClone;
    }
}
