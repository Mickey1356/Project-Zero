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

    private bool growling = false;
    private float growlTime = .5f, growlTimePassed = 0f;
    private float distance = .5f;

    private bool barking = false;
    private float barkTime = 0.3f, barkTimePassed = 0f;
    private float barkDist = 2.5f, hopDist = .7f;

    private Vector3 startPos;

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
        if (growling)
        {
            GrowlAnim();
        }
        else if (barking)
        {
            BarkAnim();
        }
        else
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
                transform.position = new Vector3(transform.position.x, transform.position.y, Constants.PLAYER_LAYER);

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
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Exit")
        {
            win = true;
            gameOver = true;
        }
    }

    private void GrowlAnim()
    {
        if(growlTimePassed > growlTime)
        {
            growling = false;
            growlTimePassed = 0;
        }
        else
        {
            growlTimePassed += Time.deltaTime;
            float theta = Mathf.PI * growlTimePassed / growlTime;
            float dist = distance * Mathf.Sin(theta);
            transform.position = startPos + Vector3.back * dist;
        }
    }

    private void BarkAnim()
    {
        if(barkTimePassed > barkTime)
        {
            barking = false;
            barkTimePassed = 0;
        }
        else
        {
            barkTimePassed += Time.deltaTime * 3f;
            transform.position = Vector3.Lerp(startPos, startPos + Vector3.right * barkDist * transform.localScale.x / Mathf.Abs(transform.localScale.x), barkTimePassed / barkTime);
            float theta = Mathf.PI * barkTimePassed / barkTime;
            float dist = hopDist * Mathf.Sin(theta);
            transform.position = transform.position + Vector3.back * dist;

        }
    }

    public void GameOver()
    {
        gameOver = true;
        rb2d.velocity = Vector2.zero;
        GetComponent<Animator>().SetBool("moving", false);
        GetComponent<PlayerAbilities>().GameOver();
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

    public void Freeze()
    {
        // only called when cat is being stunned
        canMove = false;
        GetComponent<Animator>().SetBool("moving", false);
    }

    public void Unfreeze()
    {
        canMove = true;
        GetComponent<Animator>().SetBool("moving", true);
    }

    public void Growling()
    {
        // do a small jump
        startPos = this.transform.position;
        growling = true;
    }

    public void Barking()
    {
        // lunge forward
        startPos = this.transform.position;
        barking = true;
    }
}
