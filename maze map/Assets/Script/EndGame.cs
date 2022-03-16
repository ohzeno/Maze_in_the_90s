using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{

    public GameObject EndBtn;
    bool timeActive = false;
    public float CountTime;
    public Text text_Timer;

    // Start is called before the first frame update
    void Start()
    {
        EndBtn.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Witch3")
        {
            EndBtn.SetActive(true);
            text_Timer.text = "0";
        }
    }
}
