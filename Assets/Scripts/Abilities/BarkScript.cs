using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarkScript : MonoBehaviour
{

    private float barkTime = 0.2f;

    // Use this for initialization
    void Start()
    {
        Invoke("DestroyThis", barkTime);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "SmallCat")
        {
            SmallCatAI scatai = col.gameObject.GetComponent<SmallCatAI>();
            if (!scatai.GetHiding())
            {
                scatai.KillCat();
            }
        }
    }

    private void DestroyThis()
    {
        Destroy(gameObject);
    }
}
