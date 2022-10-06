    //---------------------------------------------------------------------------------------------
    //  -   -   -   -   -   -   -   -  ENEMY AI SCRIPT BY KYLE -   -   -   -   -   -   -   -    -
    //---------------------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyAI : MonoBehaviour
{
    //---------------------------------------------------------------------------------------------
    //--All variables in this box were borrowed from PlayerMovemnet, some of their usages as well--
    //---------------------------------------------------------------------------------------------
    public float movSpeed = 2f;
    public float jumpForce = 14f;
    private float fastFallForce = -7f;
    private float dirX;
    private bool isGrounded;
    private bool canAttack = true;
    private bool isAttacking = false;
    public float attackTime = 0.5f;
    public float dashForce = 5f;
    public float attackCoolDownTime = 1f;
    private float playerX;
    private float playerY;
    private float enemyX;
    private float enemyY;
    private GameObject humanPlayer;
    public float attackRange = 1f;
    public float AggroY = 7f;
    public float AggroX = 25f;
    bool playerIsLeft = true;
    bool playerIsUp = true;
    //detection variables
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private Rigidbody2D EnemyRB;
    [SerializeField] private BoxCollider2D EnemyBC;
    private Animator anim;
    private SpriteRenderer sprite;
    // toggles between possible animation states
    private enum MovementState
    {
        idle,
        running,
        jumping,
        falling,
        attacking,
    }

    //---------------------------------------------------------------------------------------------
    // Start is called before the first frame update    -   -   -   -   -   -   -   -   -   -   -
    //---------------------------------------------------------------------------------------------
    
    void Start()
    {

        EnemyRB = GetComponent<Rigidbody2D>();
        EnemyBC = GetComponent<BoxCollider2D>();
        jumpableGround = LayerMask.GetMask("Ground");
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        //--Grabbing the player object
        humanPlayer = GameObject.FindWithTag("Player");
        
    }
    //---------------------------------------------------------------------------------------------
    // Update is called once per frame  -   -   -   -   -   -   -   -   -   -   -   -   -   -   -
    //---------------------------------------------------------------------------------------------
    void Update()
    {
        //-- Get player position and Enemy position -- 
        playerX = humanPlayer.transform.position.x;
        playerY = humanPlayer.transform.position.x;
        enemyX = transform.position.x;
        enemyY = transform.position.y;

        if (playerX > enemyX)
        {
            playerIsLeft = false;
        }
        else
        {
            playerIsLeft = true;
        }
        if (playerY > enemyY)
        {
            playerIsUp = true;
        }
        else
        {
            playerIsUp = false;
        }


        isGrounded = IsGrounded();
        // Allows attack to work properly.
        if (isAttacking)
        {
            return;
        }
        //--Modified directional calculations--
        if (playerIsLeft)
        {
            dirX = -1;
        }
        else dirX = 1;


        EnemyRB.velocity = new Vector2(dirX * movSpeed, EnemyRB.velocity.y);
        //--Modified jump behavior--
        if ( playerIsUp && isGrounded)
        {
            EnemyRB.velocity = new Vector2(EnemyRB.velocity.x, jumpForce);
        }
        //Changing the dash code for an attack
        if ((playerX - attackRange < enemyX || playerX - attackRange > enemyY) && canAttack)
        {
            StartCoroutine(Dash());
        }

    }
    //---------------------------------------------------------------------------------------------
    //  -   -   -   -   -   -   -   -Helper Functions-  -   -   -   -   -   -   -   -   -   -   -   
    //---------------------------------------------------------------------------------------------
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(EnemyBC.bounds.center, EnemyBC.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);
    }
    private IEnumerator Dash()
    {
        canAttack = false;
        isAttacking = true;
        float originalGravity = EnemyRB.gravityScale;
        EnemyRB.gravityScale = 0f;
        if (EnemyRB.velocity.x > 0f)
        {
            EnemyRB.velocity = new Vector2(transform.localScale.x * dashForce, 0f);
        }
        else if (EnemyRB.velocity.x < 0f)
        {
            EnemyRB.velocity = new Vector2(-transform.localScale.x * dashForce, 0f);
        }


        yield return new WaitForSeconds(attackTime);
        EnemyRB.gravityScale = originalGravity;
        isAttacking = false;
        yield return new WaitForSeconds(attackCoolDownTime);
        canAttack = true;
    }
    private void UpdateAnimation()
    {
        MovementState state;
        // Flips character and checks if running
        if (dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        // checks if attacking
        if (isAttacking)
        {
            state = MovementState.attacking;
        }

        // checks if jumping or falling
        if (EnemyRB.velocity.y > 0.1f)
        {
            state = MovementState.jumping;
        }
        else if (EnemyRB.velocity.y < -0.1f)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }

    public void killMob()
    {
        Destroy(this);
    }
    //---------------------------------------------------------------------------------------------
    //-     Collision Detection -   -   -   -   -   -   -   -   -   -   -   -   -   -   -   -   -   
    //---------------------------------------------------------------------------------------------
//    private void OnTriggerEnter(Collider other)
 //   {
 //       if (other.gameObject && other.tag == "Player")
 //       {
//            //Destroy the mob
  //          killMob();
  //      }
       
 //   }

}


