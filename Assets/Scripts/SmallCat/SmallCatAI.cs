using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallCatAI : MonoBehaviour
{

    // what do we want to do?
    // if cat is hidden, wait until player is nearby
    // once player is nearby, move towards him
    // if lose sight of player, wander around for a while and go back into hiding

    private GameObject player;
    public Sprite[] sprites;

    private bool inHiding = true, gameOver = false;
    private float detectRadius = Constants.SIZE_SCALE;
    private float speed;

    private float timeToHide = 10f, hideTimePassed = 0f;

    private bool jumping = false;
    private Vector3 jumpStart;
    private float distance = 1.5f, jumpSpeed = 1.5f;
    private float jumpTime = 1.5f, jumpTimePassed = 0f;

    private bool facingRight = true;
    private Vector3 rightScale, prevPos;

    public int debug_speed_mod;

    private void Awake()
    {
        speed = Random.Range(Constants.SMALLCAT_SPEED_MIN, Constants.SMALLCAT_SPEED_MAX);

        rightScale = this.transform.localScale;

        if(Random.Range(0,2) == 1)
        {
            transform.localScale = new Vector3(-rightScale.x, rightScale.y, rightScale.z);
        }

        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        prevPos = this.transform.position;
        if(inHiding)
        {
            // just detect if the player comes close
            if (Vector2.Distance(player.transform.position, this.transform.position) < detectRadius && InLineOfSight() && !jumping)
            {
                hideTimePassed = 0;
                jumping = true;
                jumpStart = transform.position;
                Appear();
            }
            if (jumping)
            {
                Appear();
            }
        }
        else
        {
            if (InLineOfSight())
            {
                hideTimePassed = 0;
                MoveCat();
            }
            else
            {
                hideTimePassed += Time.deltaTime;
                LostPlayer();
            }
        }
    }

    private void MoveCat()
    {
        this.transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, Constants.SCAT_LAYER);

        if (this.transform.position.x > prevPos.x) facingRight = true;
        else facingRight = false;

        if (facingRight) this.transform.localScale = rightScale;
        else this.transform.localScale = new Vector3(-rightScale.x, rightScale.y, rightScale.z);

    }

    private void Appear()
    {
        // "jump" up
        GetComponent<Animator>().SetBool("moving", true);
        if (jumpTimePassed >= jumpTime)
        {
            jumping = false;
            inHiding = false;
            jumpTimePassed = 0;
        }
        else
        {
            jumpTimePassed += Time.deltaTime * jumpSpeed;
            Debug.Log(jumpTimePassed);
        }
        if(jumping)
        {
            float theta = Mathf.PI * jumpTimePassed / jumpTime;
            float dist = distance * Mathf.Sin(theta);
            transform.position = jumpStart + Vector3.back * dist;
            //transform.position = Vector3.Lerp(jumpStart, jumpTarget, prog);
        }
        
    }

    private void LostPlayer()
    {
        // wander around for some time then hide again (for now just turn into a dustbin)
        if(hideTimePassed > timeToHide)
        {
            hideTimePassed = 0;
            // turn into dustbin
        }
        else
        {

        }
    }

    private bool InLineOfSight()
    {
        Vector3 _min = GetComponent<BoxCollider2D>().bounds.min;
        Vector3 _max = GetComponent<BoxCollider2D>().bounds.max;

        Vector3 pos1 = new Vector2(_min.x, _min.y);
        Vector3 pos2 = new Vector2(_min.x, _max.y);
        Vector3 pos3 = new Vector2(_max.x, _min.y);
        Vector3 pos4 = new Vector2(_max.x, _max.y);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position);
        RaycastHit2D hit1 = Physics2D.Raycast(pos1, player.transform.position - pos1);
        RaycastHit2D hit2 = Physics2D.Raycast(pos2, player.transform.position - pos2);
        RaycastHit2D hit3 = Physics2D.Raycast(pos3, player.transform.position - pos3);
        RaycastHit2D hit4 = Physics2D.Raycast(pos4, player.transform.position - pos4);

        string tag1 = hit1.collider.tag;
        string tag2 = hit2.collider.tag;
        string tag3 = hit3.collider.tag;
        string tag4 = hit4.collider.tag;

        return (hit.collider.tag == "Player" && tag1 == "Player" && tag2 == "Player" && tag3 == "Player" && tag4 == "Player");
    }

    public void GameOver()
    {

    }

    public void SetGameOver(bool value)
    {
        gameOver = value;
    }

    public bool GetGameOver()
    {
        return gameOver;
    }
}
