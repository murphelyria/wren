using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    
    private PlayerMovement playerMovement;
    private PlayerAttack playerAttack;
    private PlayerSpell playerSpell;

    public float horizontalInput = 1;

    public bool isInputEnabled;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();
        playerSpell = GetComponent<PlayerSpell>();
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
        if (Input.GetButtonDown("Attack"))
        {
            playerAttack.HandleAttack();
        }
    }

    private void spellControl()
    {
        if (Input.GetButtonDown("Spell"))
        {
            playerSpell.HandleSpell();
        }
    }
}
