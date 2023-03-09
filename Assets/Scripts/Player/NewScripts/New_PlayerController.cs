using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class New_PlayerController : MonoBehaviour
{
    public static New_PlayerController instance;
    
    private PlayerMovement playerMovement;

    public float horizontalInput;

    public bool isInputEnabled;
    private bool isJumpEnded;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        instance = this;
        
        if(isInputEnabled)
        {
            moveControl();
            jumpControl();
            fallControl();
            dashControl();
            attackControl();
            spellControl();
        }
    }

    private void moveControl()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        playerMovement.HandleMovement(horizontalInput);
    }

    private void jumpControl()
    {
        if (Input.GetButtonDown("Jump"))
        {
            playerMovement.HandleJump();
            PlayerStatistics.instance.hasJumped = true;
        }
    }

    private void fallControl()
    {
        if (Input.GetButtonUp("Jump"))
        {
            playerMovement.HandleFall();
        }
    }

    private void dashControl()
    {
        if (Input.GetButtonDown("Dash"))
        {
            playerMovement.HandleDash();
        }
    }

    private void attackControl()
    {

    }

    private void spellControl()
    {

    }
}
