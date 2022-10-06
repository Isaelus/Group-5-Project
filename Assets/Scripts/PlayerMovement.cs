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
    private bool isGrounded;
    private bool canDash = true;
    private bool isDashing = false;
    private float dashForce = 5f;
    private float dashTime = 0.2f;
    private bool canAttack = true;
    private bool isAttacking = false;
    private float attackTime = 0.5f;
    private float attackRange = 10f;
    
    public bool immunity = false;
    public int health = 3;
    private Collider2D nearestEnemy;
    private GameObject target;

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
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = IsGrounded();
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

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            player1.velocity = new Vector2(player1.velocity.x, jumpForce);
        }

        //Fast Fall
        if (Input.GetKeyDown(KeyCode.S) && !isGrounded) {
            player1.velocity = new Vector2(player1.velocity.x, fastFallForce);
        }

        // Attack
        if (Input.GetKeyDown(KeyCode.K) && isGrounded && canAttack) {
            StartCoroutine(Attack());
        }
        
        // Dashing
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash) {
            StartCoroutine(Dash());
        }

        UpdateAnimation();
    }

    private void UpdateAnimation() {
        MovementState state;
        // Flips character and checks if running
        if (dirX > 0f) {
            state = MovementState.running;
            sprite.flipX = false;
        } else if (dirX < 0f) {
            state = MovementState.running;
            sprite.flipX = true;
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
        if (player1.velocity.x > 0f) {
            player1.velocity = new Vector2(transform.localScale.x * dashForce, 0f);
        } else if (player1.velocity.x < 0f) {
            player1.velocity = new Vector2(-transform.localScale.x * dashForce, 0f);
        }
        
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
        //Get nearest enemy location in direction of attack NOTE: Gave up on this part, someone wants to crack at it, be my guest - Kyle
            
        //nearestEnemy = GetClosestEnemy(player1.velocity.x > 0f);

        //if (nearestEnemy != null) {
           // target = nearestEnemy.gameObject;
            //if ((transform.position.x - Math.Abs(target.transform.position.x)) > attackRange || (transform.position.x - Math.Abs(target.transform.position.y) > attackRange))
            //{
               // target = nearestEnemy.gameObject;
               // if (target)
               // {
                   // Destroy(target,.05f);
                //};
            //}
        //}
     
        nearestEnemy = null;

        UpdateAnimation();
        yield return new WaitForSeconds(attackTime);
        isAttacking = false;
        yield return new WaitForSeconds(attackCoolDownTime);
        canAttack = true;
    }
    //---------------------------------------------------------------------------------------------
    //-     Collision Detection -   -   -   -   -   -   -   -   -   -   -   -   -   -   -   -   -   
    //---------------------------------------------------------------------------------------------
    private void OnTriggerEnter(Collider other)
    {
        //Hurt the player
        if (other.tag == "Enemy")
        {
            health = health - 1;
            if (health == 0)
            {
                //Death
                Destroy(this);
            }
        }

    }

    //--Code taken from Cameron Oltmann on stackexchange-- Returns the collision of the closest enemy facing the player
    Collider2D GetClosestEnemy(bool facingRight)
    {
        Vector2 pos = transform.position;
        IEnumerable<Collider2D> validTargets;

        // Get all targets in range
        // Note:  You'll probably want to add a layermask to the OverlapCircleAll call
        var targets = Physics2D.OverlapCircleAll(pos, 15);

        if (facingRight)
        {
            // Filter results to only include targets to the right                
            validTargets = targets.Where(coll => coll.transform.position.x >= pos.x);
        }
        else
        {
            // Filter results to only include targets to the left
            validTargets = targets.Where(coll => coll.transform.position.x <= pos.x);
        }

        // I've broken the return statement into multiple lines for easier readability
        return validTargets
            // Sort targets by distance
            .OrderBy(coll => Vector2.Distance(pos, coll.transform.position))
            // Return first result, or null if no valid targets
            .FirstOrDefault();

    }
}

