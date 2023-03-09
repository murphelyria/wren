using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class New_PlayerController : MonoBehaviour
{
    private PlayerMovement playerMovement;

    private float horizontalInput;

    public bool isInputEnabled;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isInputEnabled)
        {
            moveInput();
            jumpInput();
            dashInput();
            attackInput();
            spellInput();
        }
    }

    private void moveInput() //
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        playerMovement.HandleMovement(horizontalInput);
    }

    private void jumpInput()
    {
        if (Input.GetButton("Jump"))
        {
            playerMovement.HandleJump();
        }
    }

    private void dashInput()
    {

    }

    private void attackInput()
    {

    }

    private void spellInput()
    {

    }
}
