using UnityEngine;
using UnityEngine.UI;
using FirebaseWebGL.Scripts.FirebaseBridge;
using FirebaseWebGL.Scripts.Objects;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MiniJSON;


public class RankingHandler : MonoBehaviour
{
        
    public static RankingHandler instance;
    //밑에 프리팹 만들 오브젝트
    [SerializeField] Transform recordListContent;
    //프리팹
    [SerializeField] GameObject gameRecordPrefab;

    //JSON으로 변환할 게임기록데이터(파이어베이스로 보낼 때)
    [Serializable]
    public class gameRecord
    {
        public int gameMode;       
        public string gameMap;
        public string nickName;
        public float time;
    }

    //오브젝트로 변환할 게임기록데이터(파이어베이스에서 받았을 때)
    [Serializable]
    public class saveRecord
    {
        public string nickName;
        public string time;
    }

    int startIdx = 0;

    public void Update()
    {
        
    }

    //게임이 끝났을 때 EndGamd에서 받은 게임데이터
    public void GetGameData(KeyValuePair<string, string> _data)
    {
        //{name: 어쩌구, time: 어쩌구} 이렇게 생김
        //string으로 받아서 float 로 변환
        float recordToFlaot = (float.Parse(_data.Value));
        SendGameRecord(_data.Key, recordToFlaot);
    }

    //게임데이터 저장, JSON 변환, 전송
    public void SendGameRecord(string username, float time)
    {
        gameRecord gameRecordObject = new gameRecord();

        gameRecordObject.gameMode = 1;
        gameRecordObject.gameMap = "forest1";
        gameRecordObject.nickName = username;
        gameRecordObject.time = time;

        string json = JsonUtility.ToJson(gameRecordObject);

        //파이어베이스로 보냄
        FirebaseDatabase.PostGameRecord(json);
    }

    //TOP10 데이터를 받아서 랭킹 씬에 연결(씬 시작할 때)
    public void SetUp(string record)
    {
        var text = "";

        //JSON 문자열 상태에서 다시 Deserialize
        Dictionary<string, object> response = Json.Deserialize(record) as Dictionary<string, object>;

        //인덱서를 사용하여 시간 값을 리턴
        string time = response["time"].ToString();

        //시간은 12:11 이런 형식으로 변환
        //string time1 = time.Substring(0, 4);

        for (int i=0; i<10; i++)
        {
            if(time[i] == '.')
            {
                text += ':';

                for (int j = i + 1; j < i + 3; j++)
                {
                    text += time[j];
                }
                break;
            }
            else
            {
                text += time[i];
            }
        }

        response["time"] = text;

        //순위도 추가
        response["idx"] = (startIdx + 1).ToString();
        startIdx += 1;

        //Debug
        Debug.Log(response["name"]);
        Debug.Log(response["time"]);
        Debug.Log(response["idx"]);

        Debug.Log(response["name"].GetType().Name);
        Debug.Log(response["time"].GetType().Name);
        Debug.Log(response["idx"].GetType().Name);

        //RecordListItem.cs로 보내기
        Instantiate(gameRecordPrefab, recordListContent).GetComponent<RankListItem>().SetUp(response);
    }

    //탭 클릭 (모드, 맵마다 다른 요청)
    public void ChangeTab(int mode, string map)
    {
        //랭킹 인덱스 초기화
        startIdx = 0;
        FirebaseDatabase.SetByInfo(mode, map);
    }


    public void LobbyorLoginScreen()
    {
        //로그인 상태인지 확인
        FirebaseAuth.IsLoggedIn();
    }

    public void BackBtn(int status)
    {
        if (status == 1)
        {
            GameManager.instance.ChangeScene("Lobby");
        }

        else
        {
            GameManager.instance.ChangeScene("Login");
        }
    }
}
