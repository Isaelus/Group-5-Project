using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerMovement : MonoBehaviour
{
    //movement variables
    private float movSpeed = 7f;
    private float jumpForce = 14f;
    private float fastFallForce = -7f;
    private float dirX;
    private bool canDash = true;
    private bool isDashing = false;
    private float dashForce = 5f;
    private float dashTime = 0.2f;
    private bool canAttack = true;
    private bool isAttacking = false;
    private float attackTime = 0.5f;
    public float attackRange = .75f;
    private bool facingRight;
    
    //combat variables
    public bool immunity = false;
    public int health = 3;
    public Transform attackPoint;
    public LayerMask enemyLayers;

    //cooldown variables
    private float dashCooldownTime = 2f;
    private float attackCoolDownTime = 0.5f;

    //detection variables
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private Rigidbody2D player1;
    [SerializeField] private BoxCollider2D p1Hitbox;
    private Animator anim;
    private SpriteRenderer sprite;

    // toggles between possible animation states
    private enum MovementState {
        idle,
        running,
        jumping,
        falling,
        attacking,
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("New Game Start - view from console");
        player1 = GetComponent<Rigidbody2D>();
        p1Hitbox = GetComponent<BoxCollider2D>();
        jumpableGround = LayerMask.GetMask("Ground");
        enemyLayers = LayerMask.GetMask("Enemy");
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        facingRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        // This allows the dash to work properly.
        if (isDashing) {
            return;
        }
        // Allows attack to work properly.
        if (isAttacking) {
            return;
        }
        
        //horizontal movement
        dirX  = Input.GetAxisRaw("Horizontal");
        player1.velocity = new Vector2(dirX * movSpeed, player1.velocity.y);

        if (dirX > 0 && !facingRight) {
            Flip();
        }
        if (dirX < 0 && facingRight) {
            Flip();
        }
        // Jump
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            player1.velocity = new Vector2(player1.velocity.x, jumpForce);
        }

        //Fast Fall
        if (Input.GetKeyDown(KeyCode.S) && !IsGrounded()) {
            player1.velocity = new Vector2(player1.velocity.x, fastFallForce);
        }

        // Attack
        if (Input.GetKeyDown(KeyCode.K) &&  canAttack)
        {//IsGrounded() &&
            StartCoroutine(Attack());
            Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
            foreach(Collider2D enemy in enemiesHit)
            {
                if (facingRight && enemy.transform.position.x > transform.position.x)
                {
                    Debug.Log("Hit!");
                    Destroy(enemy.gameObject);

                }
                else if (!facingRight && enemy.transform.position.x < transform.position.x)
                {
                    Debug.Log("Hit!");
                    Destroy(enemy.gameObject);
                }
                else
                {
                    Debug.Log("Enemy found, but in wrong direction");
                }


            }
        }
        
        // Dashing
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash) {
            StartCoroutine(Dash());
        }
        /*
        // fast way to test health HUD
        if (Input.GetKeyDown(KeyCode.G)) {
            health--;
        }
        */

        UpdateAnimation();
    }

    private void UpdateAnimation() {
        MovementState state;
        // Flips character and checks if running
        if (dirX > 0f) {
            state = MovementState.running;
        } else if (dirX < 0f) {
            state = MovementState.running;
        } else {
            state = MovementState.idle;
        }

        // checks if attacking
        if (isAttacking) {
            state = MovementState.attacking;
        }

        // checks if jumping or falling
        if (player1.velocity.y > 0.1f) {
            state = MovementState.jumping;
        } else if (player1.velocity.y < -0.1f) {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int) state);
    }

    private void Flip() {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;
        facingRight = !facingRight;
    }

    //this method checks if the player is grounded.
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(p1Hitbox.bounds.center, p1Hitbox.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);
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
    //coroutine for attacking
    private IEnumerator Attack() {
        canAttack = false;
        isAttacking = true; 
        UpdateAnimation();
        yield return new WaitForSeconds(attackTime);
        isAttacking = false;
        yield return new WaitForSeconds(attackCoolDownTime);
        canAttack = true;
    }

    //function that only exists to visualize the hitbox when selecting player1 gameObject
    void OnDrawGizmosSelected() {
        if (attackPoint == null) {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    //---------------------------------------------------------------------------------------------
    //-     Collision Detection -   -   -   -   -   -   -   -   -   -   -   -   -   -   -   -   -   
    //---------------------------------------------------------------------------------------------
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Hurt the player
        if (other.tag == "Enemy")
        {
            health--;
            if (health <= 0)
            {
                //Death
                Destroy(this);
            }
        }
    }
}

