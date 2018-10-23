using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.tag);
        Manager.man.SetText("You win. Restart the game.");
        Time.timeScale = 0;
    }
}
