using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    public GameObject growler;
    public GameObject barker;

    private PlayerMove pm;

    private bool canGrowl = true;
    private float growlCooldown = 5f, growlCooldownPassed = 0;

    private float growlRadius = 2f;

    private float barkLength = 3f;
    private float barkWidth = 1.6f;
    private float startBarkWidth = .2f;

    private bool canBark = true;
    private float barkCooldown = 10f, barkCooldownPassed = 0;

    private bool gameOver = false;

    private GameObject growlerActual, barkerActual;

    // Use this for initialization
    void Awake()
    {
        pm = GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            if (!canBark)
            {
                if (barkCooldownPassed < barkCooldown) barkCooldownPassed += Time.deltaTime;
                else
                {
                    barkCooldownPassed = 0;
                    canBark = true;
                }
            }

            if (!canGrowl)
            {
                if (growlCooldownPassed < growlCooldown) growlCooldownPassed += Time.deltaTime;
                else
                {
                    growlCooldownPassed = 0;
                    canGrowl = true;
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                canGrowl = false;
                Growl();
            }
            else if (Input.GetMouseButtonDown(1) && canBark)
            {
                canBark = false;
                Bark();
            }
        }
    }

    private void Growl()
    {
        if (growlerActual == null)
        {
            pm.Growling();
            growlerActual = Instantiate(growler, this.transform);
            //growlerActual.transform.position = this.transform.position;
            growlerActual.GetComponent<CircleCollider2D>().radius = growlRadius;
        }
    }

    private void Bark()
    {
        if(barkerActual == null)
        {
            pm.Barking();
            barkerActual = Instantiate(barker, this.transform);
            //barkerActual.transform.position = this.transform.position;
            Vector2[] pts = new Vector2[] { new Vector2(barkLength, barkWidth / 2),
                                            new Vector2(barkLength, -barkWidth / 2),
                                            new Vector2(0, -startBarkWidth / 2),
                                            new Vector2(0, startBarkWidth / 2)};
            barkerActual.GetComponent<PolygonCollider2D>().SetPath(0, pts);
        }
    }

    public void GameOver()
    {
        gameOver = true;
    }
}
