using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    GameManager gameManager;
    public GameObject EndBtn;
    bool timeActive = false;
    public float CountTime;
    public Text text_Timer;
    public GameObject StartBtn;
    public int End = 0;
    public GameObject Exit;

    // Start is called before the first frame update
    void Start()
    {
        EndBtn.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Witch3" && End == 0)
        {
            EndBtn.SetActive(true);
            text_Timer.text = "00:00:00";
            StartBtn.GetComponent<StartGame>().timeActive = false;
            End = 1;
            Exit.SetActive(true);
        }
    }
}
