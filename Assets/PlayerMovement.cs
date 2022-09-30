using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //movement variables
    [SerializeField] private float movSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;
    /*
    [SerializeField] private float dashForce;
    [SerializeField] private bool canDash;
    [SerializeField] private float dashDirection;
    */

    //detection variables
    [SerializeField] private LayerMask jumpableGround = 6;
    [SerializeField] private Rigidbody2D player1;
    [SerializeField] private BoxCollider2D p1Hitbox;

    /*
    //cooldown variables
    public float dashCooldownTime = 2f; //working out how to dash right now - Brylle
    [SerializeField] private float currentDashCD = 0f;
    */

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("New Game Start - view from console");
        player1 = GetComponent<Rigidbody2D>();
        p1Hitbox = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

        //horizontal movement
        float dirX  = Input.GetAxisRaw("Horizontal");
        player1.velocity = new Vector2(dirX * movSpeed, player1.velocity.y);

        // Jump
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            player1.velocity = new Vector2(player1.velocity.x, jumpForce);
        }

        /* Dash
        if (Input.GetButtonDown("Dash") && dirX != 0) {
            canDash = false;
            currentDashCD = dashCooldownTime;
            rb.velocity = Vector2.zero;
            dashDirection = (int)dirX;
        }
        
        // 
        if (!canDash) {
            dashCD();
        }
        */
        
    }

    //this method checks if the player is grounded.
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(p1Hitbox.bounds.center, p1Hitbox.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);
    }

    //timer for cooldown
    /*
    private void dash() {
        currentDashCD -= Time.deltaTime;
        if (currentDashCD <= 0) {
            canDash = true;
        }
    }
    */
}
