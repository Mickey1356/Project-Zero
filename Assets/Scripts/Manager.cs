using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject bigCat;
    public GameObject player;

    private Level level;
    private GameObject cat;

    private void Awake()
    {
        level = GetComponent<Level>();
        level.Initialise();
    }

    private void Start()
    {
        level.CreateLevel();

        cat = Instantiate(bigCat);
        cat.GetComponent<CatAI>().Initialise(level.GetLevel(), player);
        cat.transform.position = level.GetCatSpawn();

        player.transform.position = level.GetPlayerSpawn();
        //cat.GetComponent<CatAI>().UpdateCat();
    }

    private void Update()
    {
        cat.GetComponent<CatAI>().UpdateCat();

        // for the perspective tilt
        Camera.main.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 5, Constants.CAMERA_LAYER);
    }
}
