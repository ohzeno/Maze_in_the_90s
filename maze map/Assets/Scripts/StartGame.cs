using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;//Æ÷Åæ ±â´É »ç¿ë
using System.Runtime.InteropServices;

public class StartGame : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void CallCamera(string userName, int score);


    public GameObject StartBtn;
    public bool timeActive = false;
    public float CountTime;
    public Text text_Timer;
    private PhotonView pv;

    public int lifetime = 5;
    // Start is called before the first frame update
    void Start()
    {
        UnityCall();
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

    public void UnityCall()
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
    CallCamera("¶Ñ·ç·ç", lifetime);
#endif
    }
}