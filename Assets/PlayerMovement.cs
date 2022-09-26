using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D player1;
    


    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("New Game Start - view from console");
        player1 = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    private void Update()
    {
        
        float dirX  = Input.GetAxisRaw("Horizontal");
        player1.velocity = new Vector2(dirX * 7f, player1.velocity.y);


        if (Input.GetButtonDown("Jump"))
        {
            player1.velocity = new Vector2(player1.velocity.x, 14f);
        }

       

        

    }

    
}
