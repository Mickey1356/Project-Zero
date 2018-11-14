using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public GameObject bigCat;
    public GameObject smallCat;
    public GameObject player;
    public GameObject exit;
    public GameObject endText;
    public GameObject trash;

    public Transform sCatParent;
    public Transform trashParent;

    private Level level;
    private GameObject cat;
    private GameObject exitGo;

    private float catThreshold = 0.85f;

    private bool gameOver = false;

    private List<GameObject> scats;

    public static Manager man;

    private void Awake()
    {
        level = GetComponent<Level>();
        level.Initialise();

        man = this;
        endText.SetActive(false);

        scats = new List<GameObject>();
    }

    private void Start()
    {
        level.CreateLevel();

        cat = Instantiate(bigCat);
        cat.GetComponent<CatAI>().Initialise(level.GetLevel(), player);
        cat.transform.position = level.GetCatSpawn();

        player.transform.position = level.GetPlayerSpawn();
        //cat.GetComponent<CatAI>().UpdateCat();

        exitGo = Instantiate(exit);
        exitGo.transform.position = level.GetExit();

        foreach(Vector2 pos in level.GetSmallCatSpawns())
        {
            if (Random.value < catThreshold)
            {
                GameObject scat = Instantiate(smallCat, sCatParent);
                scats.Add(scat);
                scat.transform.position = new Vector3(pos.x * Constants.SIZE_SCALE, pos.y * Constants.SIZE_SCALE, Constants.SCAT_LAYER);
            }
            else
            {
                GameObject go = Instantiate(trash, trashParent);
                go.transform.position = new Vector3(pos.x * Constants.SIZE_SCALE, pos.y * Constants.SIZE_SCALE, Constants.SCAT_LAYER);
            }
        }
    }

    private void Update()
    {
        if (!gameOver)
        {
            cat.GetComponent<CatAI>().UpdateCat();
            if (cat.GetComponent<CatAI>().GetGameOver() || player.GetComponent<PlayerMove>().GetGameOver() || CheckSCatGameOver())
            {
                GameOver(player.GetComponent<PlayerMove>().GetWin());
            }
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                RestartLevel();
            }
        }

        // for the perspective tilt
        Camera.main.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 5, Constants.CAMERA_LAYER);
    }

    private bool CheckSCatGameOver()
    {
        foreach(GameObject scat in scats)
        {
            if (scat.GetComponent<SmallCatAI>().GetGameOver()) return true;
        }

        return false;
    }

    private void SmallCatGameOver()
    {
        foreach(GameObject scat in scats)
        {
            scat.GetComponent<SmallCatAI>().SetGameOver(true);
            scat.GetComponent<SmallCatAI>().GameOver();
        }
    }

    public void SetText(string text)
    {
        endText.SetActive(true);
        endText.GetComponentInChildren<Text>().text = text;
    }

    private void GameOver(bool win)
    {
        gameOver = true;
        cat.GetComponent<CatAI>().SetGameOver(true);
        player.GetComponent<PlayerMove>().SetGameOver(true);

        cat.GetComponent<CatAI>().GameOver();
        player.GetComponent<PlayerMove>().GameOver();

        SmallCatGameOver();

        if (win)
        {
            SetText("Congratulations!\nYou reached the exit!\nPress R to return to the main menu.");
        }
        else
        {
            SetText("Too bad!\nA cat got you!\nPress R to return to the main menu.");
        }
    }

    private void RestartLevel()
    {
        Debug.Log("Restarting level.");
        SceneManager.LoadScene(Constants.MENU_SCENE); // restart the level
    }
}
