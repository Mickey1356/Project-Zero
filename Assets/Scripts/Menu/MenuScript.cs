using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public Text[] options;
    public Text[] infos;

    private Color selColor = new Color(0, 1f, 1f);

    private int selOption = 0; // 0 is play, 1 is instructions, 2 is credits
    private bool backMenuReq = false;

    void Start()
    {
        foreach (var t in infos) t.enabled = false;
        infos[2].enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!backMenuReq)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                selOption += 1;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                selOption += 2;
            }
            selOption = selOption % 3;
        }

        foreach (var t in options) t.color = Color.white;
        options[selOption].color = selColor;

        if (Input.GetKeyDown(KeyCode.Return) && !backMenuReq)
        {
            switch (selOption)
            {
                case 0:
                    SceneManager.LoadScene(Constants.MAIN_SCENE); // restart the level
                    break;
                case 1:
                    foreach (var t in options) t.enabled = false;
                    infos[0].enabled = true;
                    infos[2].enabled = false;
                    infos[3].enabled = true;

                    backMenuReq = true;
                    break;
                case 2:
                    foreach (var t in options) t.enabled = false;
                    infos[1].enabled = true;
                    infos[2].enabled = false;
                    infos[3].enabled = true;

                    backMenuReq = true;
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && backMenuReq)
        {
            foreach (var t in options) t.enabled = true;
            foreach (var t in infos) t.enabled = false;
            infos[2].enabled = true;
            backMenuReq = false;
        }
    }
}
