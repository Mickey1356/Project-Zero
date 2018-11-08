using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowlScript : MonoBehaviour
{
    private float growlTime = 2f;

    private List<GameObject> sCatsAffected;
    private GameObject bigCat;

    private bool bigCatAffected = false;

    private Vector3 oPos;

    private void Awake()
    {
        sCatsAffected = new List<GameObject>();
    }

    // Use this for initialization
    void Start()
    {
        Invoke("DestroyThis", growlTime);
        oPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = oPos;
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "SmallCat")
        {
            SmallCatAI scatai = col.gameObject.GetComponent<SmallCatAI>();
            if (!scatai.GetHiding())
            {
                scatai.Freeze();
                sCatsAffected.Add(col.gameObject);
            }
        }
        else if (col.gameObject.tag == "Cat")
        {
            bigCat = col.gameObject;
            col.gameObject.GetComponent<CatAI>().Freeze();
            bigCatAffected = true;
        }
    }

    private void DestroyThis()
    {
        if(bigCatAffected)
        {
            bigCat.GetComponent<CatAI>().Unfreeze();
        }
        foreach(GameObject cat in sCatsAffected)
        {
            cat.GetComponent<SmallCatAI>().Unfreeze();
        }
        Destroy(gameObject);
    }
}
