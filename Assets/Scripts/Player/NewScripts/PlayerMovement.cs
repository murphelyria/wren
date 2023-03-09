using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rb;

    private bool isJumpEnded;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void HandleMovement(float horizontalInput)
    {
        float horizontalMovement = horizontalInput * PlayerStatistics.instance.moveSpeed;

        Vector2 horzMovement = new(horizontalMovement, _rb.velocity.y);
        _rb.velocity = horzMovement;
    }

    public void HandleJump()
    {
        StartCoroutine(JumpTimer());
        isJumpEnded = false;

        if (!isJumpEnded)
        {
            Vector2 vertMovement = new(_rb.velocity.x, PlayerStatistics.instance.jumpSpeed);
            _rb.velocity = vertMovement;
        }
    }

    private IEnumerator JumpTimer()
    {
        float jumpHoldTime = 2f;
        while (jumpHoldTime > 0f)
        {
            jumpHoldTime -= Time.deltaTime;
            yield return null;
        }

        isJumpEnded = true;
    }
}