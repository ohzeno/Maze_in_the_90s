using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class EndGame : MonoBehaviour
{
    public GameObject StartBtn;
    public GameObject EndBtn;
    public GameObject TimeRecord;
    public Text text_Timer;

    private int playercnt = 0;

    // Start is called before the first frame update
    void Start()
    {
        EndBtn.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.name == "Witch3(Clone)")
        {
            EndBtn.SetActive(true);
            StartBtn.GetComponent<StartGame>().timeActive = false;
            playercnt += 1;
            Debug.Log(playercnt);
            Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (playercnt == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            TimeRecord.SetActive(true);
            EndBtn.SetActive(false);
        }
    }
}
