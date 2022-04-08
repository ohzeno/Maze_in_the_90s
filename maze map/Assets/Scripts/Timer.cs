using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;//포톤 기능 사용

public class Timer : MonoBehaviour
{
    public static Timer instance;
    public bool timeActive = false;
    public float CountTime;
    public Text text_Timer;
    private PhotonView pv;
    private int playercnt = 0;
    public GameObject TimeRecord;
    [SerializeField] Transform recordListContent;
    [SerializeField] GameObject recordListItemPrefab;

    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        timeActive = true;
        pv.RPC("StartTime", RpcTarget.All);
    }

    // Update is called once per frame
    private void Update()
    {
        if (timeActive)
        {
            CountTime += Time.deltaTime;
            text_Timer.text = "Time : " + CountTime.ToString("F2");
            if (CountTime > 200) //시간 초과 시 생존자 승리
            {
                timeActive = false; //타이머 끄기
                foreach (int i in PhotonNetwork.CurrentRoom.Players.Keys)//존재하는 모든 roomListContent
                {
                    if (!GameManager.Tagged[i]) // 잡히지 않은 경우
                    {
                        if (i== (int)PhotonNetwork.CurrentRoom.CustomProperties["Tagger"])
                        {
                            GameManager.records.Add(PhotonNetwork.CurrentRoom.Players[i].ToString().Substring(4), "Loser");
                        }
                        else
                        {
                            GameManager.records.Add(PhotonNetwork.CurrentRoom.Players[i].ToString().Substring(4), "Winner" );
                        }
                    }
                }
                foreach (KeyValuePair<string, string> record in GameManager.records)//존재하는 모든 roomListContent
                {
                    playercnt += 1;
                    Instantiate(recordListItemPrefab, recordListContent).GetComponent<RecordListItem>().SetUp(playercnt, record);
                }
                TimeRecord.SetActive(true);
            }
        }
    }

    private void Awake()
    {
        instance = this;
    }


    public void crush(int i)
    {
        timeActive = false; //타이머 끄기
        GameManager.records.Add(PhotonNetwork.CurrentRoom.Players[i].ToString().Substring(4), "Winner");
        foreach (KeyValuePair<string, string> record in GameManager.records)//모두 잡았을 때
        {
            playercnt += 1;
            Instantiate(recordListItemPrefab, recordListContent).GetComponent<RecordListItem>().SetUp(playercnt, record);
        }
        TimeRecord.SetActive(true);
    }

    [PunRPC]
    void StartTime(PhotonMessageInfo info)
    {
        GameManager.startTime = info.SentServerTime;
    }

}
