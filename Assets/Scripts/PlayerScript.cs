using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private const float timeUntilReset = 3.5f;

    private EntityType type = EntityType.DOG;
    private string killer = "???";

    public bool canMove = true;
    public bool canDie = true;
    public EntityType Type
    {
        get
        {
            return type;
        }
    }

    private bool dying = false;

    public EntityState state;

    public static GameObject player;
    public static PlayerScript playerScript;

    // Use this for initialization
    void Awake()
    {
        //Get and store a reference to the Rigidbody2D component so that we can access it.
        player = gameObject;
        playerScript = this;
    }

    void Start()
    {
        state = EntityState.ALIVE;
    }

    void Update()
    {
        if (state == EntityState.DEAD && dying == false)
        {
            dying = true;
            Die();
        }
    }

    void Die()
    {
        Debug.Log("You were killed by a " + killer); //different messages?
                                                     //death animation, statistics etc.
                                                     //wait for some time
        PlayerMove.playermove.SetMove(false);
        CatController.cc.SetDeath();
        Manager.man.SetText("You died. Restart the game.");
        Time.timeScale = 0;
        Invoke("ResetStuff", Constants.RESTART_TIME);
    }

    private void ResetStuff()
    {
        Debug.Log("hello");
        Debug.Log("Restarting level...");
        //reset game
        GameManager.gameManager.RestartLevel();

    }

    public void PlayerTouched(GameObject obj = null)
    {
        Debug.Log("received");
        if (obj != null && obj.tag == "Cat" && obj.GetComponent<CatController>() != null)
        {
            EntityType ent = obj.GetComponent<CatController>().EntType;
            if ((ent == EntityType.BIGCAT || ent == EntityType.SMALLCAT) && canDie == true)
            {
                switch (ent)
                {
                    case (EntityType.BIGCAT):
                        killer = "Big Cat";
                        break;
                    case (EntityType.SMALLCAT):
                        killer = "Small cat";
                        break;
                }
                state = EntityState.DEAD;

            }
        }
    }
}
