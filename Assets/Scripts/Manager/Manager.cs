using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public GameObject bigCat;
    public GameObject player;
    public GameObject exit;
    public GameObject endText;

    private Level level;
    private GameObject cat;
    private GameObject exitGo;

    private bool gameOver = false;

    public static Manager man;

    private void Awake()
    {
        level = GetComponent<Level>();
        level.Initialise();

        man = this;
        endText.SetActive(false);
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
    }

    private void Update()
    {
        if (!gameOver)
        {
            cat.GetComponent<CatAI>().UpdateCat();
            if (cat.GetComponent<CatAI>().GetGameOver() || player.GetComponent<PlayerMove>().GetGameOver())
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

    public void SetText(string text)
    {
        endText.SetActive(true);
        endText.GetComponent<Text>().text = text;
    }

    private void GameOver(bool win)
    {
        gameOver = true;
        cat.GetComponent<CatAI>().SetGameOver(true);
        player.GetComponent<PlayerMove>().SetGameOver(true);

        cat.GetComponent<CatAI>().GameOver();
        player.GetComponent<PlayerMove>().GameOver();

        if (win)
        {
            SetText("Congratulations!\nYou reached the exit!\nPress R to restart.");
        }
        else
        {
            SetText("Too bad!\nA cat got you!\nPress R to restart.");
        }
    }

    private void RestartLevel()
    {
        Debug.Log("Restarting level.");
        SceneManager.LoadScene(Constants.MAIN_SCENE); // restart the level
    }
}
