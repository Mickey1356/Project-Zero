using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject player;
    public static GameManager gameManager;

    //STATISTICS

    private float wins;
    public float Wins
    {
        get
        {
            return wins;
        }
    }

    private float deaths;
    public float Deaths
    {
        get
        {
            return deaths;
        }
    }

    private float kills;
    public float Kills
    {
        get
        {
            return kills;
        }
    }

    private float bigCatKills;
    public float BigCatKills
    {
        get
        {
            return bigCatKills;
        }
    }

    private float smallCatKills;
    public float SmallCatKills
    {
        get
        {
            return smallCatKills;
        }
    }

    //barks this round
    private float barksLocal;
    public float BarksLocal
    {
        get
        {
            return barksLocal;
        }
    }

    private float barksGlobal;
    public float BarksGlobal
    {
        get
        {
            return barksGlobal;
        }
    }


    private float growlsLocal;
    public float GrowlsLocal
    {
        get
        {
            return growlsLocal;
        }
    }

    private float growlsGlobal;
    public float GrowlsGlobal
    {
        get
        {
            return growlsGlobal;
        }
    }

    void Awake()
    {
        gameManager = this;
    }

    void Start()
    {
        player = PlayerScript.player;
    }

    public void RestartLevel(string option = "default")
    {
        if (option == "default")
        {
            Debug.Log("Restarting level...");
            StopAllCoroutines();
            SceneManager.LoadScene(Constants.MAIN_SCENE); // restart the level
        }
    }
}
