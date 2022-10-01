using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //movement variables
    private float movSpeed = 7f;
    private float jumpForce = 14f;
    private float fastFallForce = -7f;
    private float dirX;
    private bool facingRight = true;
    private bool canDash = true;
    private bool isDashing = false;
    private float dashForce = 5f;
    private float dashTime = 0.2f;

    //cooldown variables
    private float dashCooldownTime = 2f;

    //detection variables
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private Rigidbody2D player1;
    [SerializeField] private BoxCollider2D p1Hitbox;
    

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("New Game Start - view from console");
        player1 = GetComponent<Rigidbody2D>();
        p1Hitbox = GetComponent<BoxCollider2D>();
        jumpableGround = LayerMask.GetMask("Ground");

    }

    // Update is called once per frame
    void Update()
    {
        // This allows the dash to work properly.
        if (isDashing) {
            return;
        }
        
        //horizontal movement
        dirX  = Input.GetAxisRaw("Horizontal");
        player1.velocity = new Vector2(dirX * movSpeed, player1.velocity.y);

        // Jump
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            player1.velocity = new Vector2(player1.velocity.x, jumpForce);
        }
        //Fast Fall
        if (Input.GetKeyDown(KeyCode.S) && !IsGrounded()) {
            player1.velocity = new Vector2(player1.velocity.x, fastFallForce);
        }
        
        // Flipping player Model
        if (dirX > 0 && !facingRight) {
            Flip();
        }
        
        if (dirX < 0 && facingRight) {
            Flip();
        }

        // Dashing
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash) {
            StartCoroutine(Dash());
        }
    }

    //this method checks if the player is grounded.
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(p1Hitbox.bounds.center, p1Hitbox.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);
    }

    //function that flips the player model
    private void Flip() {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        facingRight = !facingRight;
    }
    
    //coroutine that is responsible for dash cooldown and dash mechanic.
    private IEnumerator Dash() {
        canDash = false;
        isDashing = true;
        float originalGravity = player1.gravityScale;
        player1.gravityScale = 0f;
        player1.velocity = new Vector2(transform.localScale.x * dashForce, 0f);
        yield return new WaitForSeconds(dashTime);
        player1.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldownTime);
        canDash = true;
    }
}
