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
    public Sprite catSprite;
    public Sprite dustbinSprite;

    private bool inHiding = true, gameOver = false, hitPlayer = false;
    private float detectRadius = Constants.SIZE_SCALE * 2f;
    private float speed;

    private float sizeScaleMin = 0.75f, sizeScaleMax = 1.2f;
    private float timeToHide, timeToHideMin = 9f, timeToHideMax = 15f, hideTimePassed = 0f;

    private bool wandering = false;
    private float wanderTime = 1f, wanderTimePassed = 0f;
    private Vector3 startPos, targetPos;

    private bool jumping = false;
    private Vector3 jumpStart;
    private float distance = 1.7f, jumpSpeed = 1.7f;
    private float jumpTime = 1f, jumpTimePassed = 0f;

    private bool facingRight = true, canMove = true;
    private Vector3 rightScale, prevPos;

    private float sizeScale;
    private float collision_dist = Constants.SIZE_SCALE / 4;

    private bool frozen = false;
    private float freezeScale = 0.85f;
    private Vector3 originalScale;

    private bool dying = false;
    private float deathTime = 2f, deathTimePassed = 0;
    private Vector3 deathStartPos;

    public int debug_speed_mod;

    private void Awake()
    {
        speed = Random.Range(Constants.SMALLCAT_SPEED_MIN, Constants.SMALLCAT_SPEED_MAX);
        sizeScale = Random.Range(sizeScaleMin, sizeScaleMax);
        timeToHide = Random.Range(timeToHideMin, timeToHideMax);
        this.transform.localScale *= sizeScale;

        rightScale = this.transform.localScale;

        if(Random.Range(0,2) == 1)
        {
            transform.localScale = new Vector3(-rightScale.x, rightScale.y, rightScale.z);
        }

        player = GameObject.FindGameObjectWithTag("Player");

    }

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = dustbinSprite;
    }

    // Update is called once per frame
    void Update()
    {
        if(dying)
        {
            if(deathTimePassed < deathTime)
            {
                deathTimePassed += Time.deltaTime;
                transform.position = Vector3.Lerp(deathStartPos, deathStartPos + Vector3.back * 50, deathTimePassed / deathTime);
            }
            else
            {
                this.enabled = false;
                Destroy(this.transform.GetChild(0).gameObject);
            }
        }

        if (!gameOver && !hitPlayer)
        {
            prevPos = this.transform.position;
            if (inHiding)
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
                if(Vector2.Distance(this.transform.position, player.transform.position) < collision_dist && !dying)
                {
                    gameOver = true;
                    Debug.Log("A small cat hit the player.");
                }

                if (canMove)
                {
                    if (InLineOfSight())
                    {
                        hideTimePassed = 0;
                        MoveCat();
                        wandering = false;
                        wanderTimePassed = 0;
                    }
                    else
                    {
                        hideTimePassed += Time.deltaTime;
                        LostPlayer();
                    }
                }
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
            inHiding = true;
            GetComponent<Animator>().SetBool("moving", false);
        }
        else
        {
            hideTimePassed += Time.deltaTime;

            if(!wandering)
            {
                // choose a random direction to wander in
                float distX = Random.Range(-Constants.SIZE_SCALE, Constants.SIZE_SCALE);
                float distY = Random.Range(-Constants.SIZE_SCALE, Constants.SIZE_SCALE);

                float dist = Mathf.Sqrt(distX * distX + distY * distY) * 1.5f;

                Vector3 targetPos = new Vector3(distX + this.transform.position.x, distY + this.transform.position.y, this.transform.position.z);

                Vector3 _min = GetComponent<BoxCollider2D>().bounds.min;
                Vector3 _max = GetComponent<BoxCollider2D>().bounds.max;

                Vector3 pos1 = new Vector2(_min.x, _min.y);
                Vector3 pos2 = new Vector2(_min.x, _max.y);
                Vector3 pos3 = new Vector2(_max.x, _min.y);
                Vector3 pos4 = new Vector2(_max.x, _max.y);

                // Raycast to see if you can reach that point

                RaycastHit2D hit = Physics2D.Raycast(transform.position, targetPos - transform.position, dist);
                RaycastHit2D hit1 = Physics2D.Raycast(pos1, targetPos - pos1, dist);
                RaycastHit2D hit2 = Physics2D.Raycast(pos2, targetPos - pos2, dist);
                RaycastHit2D hit3 = Physics2D.Raycast(pos3, targetPos - pos3, dist);
                RaycastHit2D hit4 = Physics2D.Raycast(pos4, targetPos - pos4, dist);

                Debug.DrawLine(pos1, targetPos);
                Debug.DrawLine(pos2, targetPos);
                Debug.DrawLine(pos3, targetPos);
                Debug.DrawLine(pos4, targetPos);


                if (!hit && !hit1 && !hit2 && !hit3 && !hit4)
                {
                    this.startPos = this.transform.position;
                    this.targetPos = targetPos;
                    wandering = true;
                }
            }
            else
            {
                if(wanderTimePassed < wanderTime)
                {
                    wanderTimePassed += Time.deltaTime;
                    this.transform.position = Vector3.Lerp(this.startPos, this.targetPos, wanderTimePassed / wanderTime);

                    if (this.targetPos.x > this.startPos.x) facingRight = true;
                    else facingRight = false;

                    if (facingRight) this.transform.localScale = rightScale;
                    else this.transform.localScale = new Vector3(-rightScale.x, rightScale.y, rightScale.z);
                }
                else
                {
                    wandering = false;
                    wanderTimePassed = 0;
                }
            }
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
        hitPlayer = true;
        GetComponent<Animator>().SetBool("moving", false);
    }

    public void SetGameOver(bool value)
    {
        gameOver = value;
    }

    public bool GetGameOver()
    {
        return gameOver;
    }

    public void Freeze()
    {
        if (!frozen)
        {
            originalScale = this.transform.localScale;
            this.transform.localScale = new Vector3(originalScale.x, originalScale.y * freezeScale, originalScale.z);
            frozen = true;
        }
        // only called when cat is being stunned
        canMove = false;
        GetComponent<Animator>().enabled = false;
        GetComponent<Animator>().SetBool("moving", false);
        GetComponent<SpriteRenderer>().sprite = catSprite;
    }

    public void Unfreeze()
    {
        frozen = false;
        canMove = true;
        GetComponent<Animator>().SetBool("moving", true);
        GetComponent<Animator>().enabled = true;
        this.transform.localScale = originalScale;
    }

    public bool GetHiding()
    {
        return inHiding;
    }

    public void KillCat()
    {
        dying = true;
        canMove = false;
        GetComponent<Animator>().enabled = false;
        GetComponent<Animator>().SetBool("moving", false);
        GetComponent<SpriteRenderer>().sprite = catSprite;
        this.deathStartPos = transform.position;
    }
}
