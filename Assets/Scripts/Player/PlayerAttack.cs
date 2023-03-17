using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject horizontalAttackPrefab;
    public GameObject verticalAttackPrefab;
    public GameObject attackPrefab;

    public void HandleAttack()
    {
        if (PlayerStatistics.instance.canAttack)
        {
            PlayerStatistics.instance.UpdateAttackDirection();
            Vector2 attackDirection = PlayerStatistics.instance.attackDirection;

            if (PlayerStatistics.instance.yAttack)
            {
                attackPrefab = verticalAttackPrefab;
            }
            else
            {
                attackPrefab = horizontalAttackPrefab;
            }

            StartCoroutine(DoAttack(attackDirection));
        }
    }

    private IEnumerator DoAttack(Vector2 attackDirection)
    {
        PlayerStatistics.instance.canAttack = false;

        Vector3 offset = new Vector3(attackDirection.x, attackDirection.y, 0);
        GameObject attackInstance = Instantiate(attackPrefab, transform.position + offset, Quaternion.identity, transform);
        StartCoroutine(DestroyAfterFrames(attackInstance, PlayerStatistics.instance.attackDurationFrames));

        yield return null;
    }

    private IEnumerator DestroyAfterFrames(GameObject obj, int frames)
    {
        yield return new WaitForSeconds(frames * Time.deltaTime);

        Destroy(obj);

        yield return new WaitForSeconds(PlayerStatistics.instance.attackDelay);
        PlayerStatistics.instance.canAttack = true;
    }
}

//      Needs damage dealing code