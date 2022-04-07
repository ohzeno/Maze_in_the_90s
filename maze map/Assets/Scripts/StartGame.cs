using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;//포톤 기능 사용

public class StartGame : MonoBehaviour
{
    public GameObject StartBtn;
    public bool timeActive = false;
    public float CountTime;
    public Text text_Timer;
    private PhotonView pv;

    public int lifetime = 5;
    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        pv = GetComponent<PhotonView>();
        if (StartBtn.activeSelf == true)
        {
            StartBtn.SetActive(false);
            Destroy(GameObject.Find("StartLine"), lifetime);
            timeActive = true;
            pv.RPC("StartTime", RpcTarget.All);
        }
    }

    [PunRPC]
    void StartTime( PhotonMessageInfo info)
    {
        GameManager.startTime = info.SentServerTime;
    }

    private void Update()
    {
        if (StartBtn.activeSelf == false)
        {
            if (timeActive)
            {
                CountTime += Time.deltaTime;
                text_Timer.text = "Time : " + CountTime.ToString("F2");
            }
        }
    }
}