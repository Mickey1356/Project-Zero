using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        cat.GetComponent<CatAI>().UpdateCat();

        // for the perspective tilt
        Camera.main.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 5, Constants.CAMERA_LAYER);
    }

    public void SetText(string text)
    {
        Debug.Log("hello");
        endText.SetActive(true);
        endText.GetComponent<Text>().text = text;
    }
}
