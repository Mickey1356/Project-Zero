using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public static GameObject player;

    private float speed = Constants.PLAYER_SPEED;             //Floating point variable to store the player's movement speed.
    private bool facingLeft = false, canMove = true, gameOver = false, win = false;
    private Vector3 rightScale;

    public static PlayerMove playermove;

    private Rigidbody2D rb2d;       //Store a reference to the Rigidbody2D component required to use 2D Physics.

    void Awake()
    {
        //Get and store a reference to the Rigidbody2D component so that we can access it.
        rb2d = GetComponent<Rigidbody2D>();
        player = gameObject;
        playermove = this;


        rightScale = transform.localScale;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!gameOver && canMove)
        {
            //Store the current horizontal input in the float moveHorizontal.
            float moveHorizontal = Input.GetAxis("Horizontal");

            //Store the current vertical input in the float moveVertical.
            float moveVertical = Input.GetAxis("Vertical");

            //Use the two store floats to create a new Vector2 variable movement.
            Vector2 movement = new Vector2(moveHorizontal, moveVertical);
            //transform.position += new Vector3(moveHorizontal * speed * Time.deltaTime, 0, 0);
            //transform.position += new Vector3(0, moveVertical * speed * Time.deltaTime, 0);

            //float movex = 0;
            //float movey = 0;



            //Call the AddForce function of our Rigidbody2D rb2d supplying movement multiplied by speed to move our player.
            //rb2d.AddForce(movement * speed);
            rb2d.velocity = movement * speed;
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);

            if (moveHorizontal == 0 && moveVertical == 0)
            {
                GetComponent<Animator>().SetBool("moving", false);
            }
            else
            {
                GetComponent<Animator>().SetBool("moving", true);
            }

            if (moveHorizontal < 0)
            {
                facingLeft = true;
            }
            else if (moveHorizontal > 0)
            {
                facingLeft = false;
            }

            if (facingLeft)
            {
                transform.localScale = new Vector3(-rightScale.x, rightScale.y, rightScale.z);
            }
            else
            {
                transform.localScale = rightScale;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Exit")
        {
            win = true;
            gameOver = true;
        }
    }

    public void GameOver()
    {
        gameOver = true;
        rb2d.velocity = Vector2.zero;
        GetComponent<Animator>().SetBool("moving", false);
        canMove = false;
    }

    public bool GetGameOver()
    {
        return gameOver;
    }

    public void SetGameOver(bool value)
    {
        gameOver = value;
    }

    public bool GetWin()
    {
        return win;
    }
}
