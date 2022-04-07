using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMenual : MonoBehaviour
{
    public GameObject menual;
    public GameObject menualhead;

    // Start is called before the first frame update
    void Start()
    { 
        if (Jscall.controlmode == "hand")
        {
            menualhead.SetActive(false);
        }
        else if (Jscall.controlmode == "pose")
        {
            menual.SetActive(false);
        }
    }

    public void Close()
    {
        if (Jscall.controlmode == "hand")
        {
            menual.SetActive(false);
        }
        else if (Jscall.controlmode == "pose")
        {
            menualhead.SetActive(false);
        }        
    }

    public void Open()
    {
        if (Jscall.controlmode == "hand")
        {
            menual.SetActive(true);
        }
        else if (Jscall.controlmode == "pose")
        {
            menualhead.SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
