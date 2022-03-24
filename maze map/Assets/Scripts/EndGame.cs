using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    public GameObject StartBtn;
    public GameObject EndBtn;
    public GameObject TimeRecord;

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
            StartBtn.GetComponent<StartGame>().timeActive = false;

            // 플레이어가다들어온다면 
            TimeRecord.SetActive(true);
            EndBtn.SetActive(false);
            
        }
    }
}
