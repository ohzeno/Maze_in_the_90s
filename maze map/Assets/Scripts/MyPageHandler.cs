using UnityEngine;
using UnityEngine.UI;
using FirebaseWebGL.Scripts.FirebaseBridge;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MiniJSON;


namespace FirebaseWebGL.Examples.Auth
{
    public class MyPageHandler : MonoBehaviour
    {
        public static MyPageHandler instance;
        //밑에 프리팹 만들 오브젝트(전적)
        [SerializeField] Transform recordListContent;
        //프리팹
        [SerializeField] GameObject gameRecordPrefab;
        //밑에 프리팹 만들 오브젝트(랭킹)
        [SerializeField] Transform rankListContent;
        //프리팹
        [SerializeField] GameObject gameRankPrefab;



        void Start()
        {
            GetRecords(FirebaseWebGL.Examples.Auth.LobbyHandler.userName);
            GetRanks(FirebaseWebGL.Examples.Auth.LobbyHandler.userName);
        }


        //유저 전적을 연결(마이페이지 전환될 때)
        public void SetUp(string record)
        {
            var text = "";

            //JSON 문자열 상태에서 다시 Deserialize
            Dictionary<string, object> response = Json.Deserialize(record) as Dictionary<string, object>;

            //인덱서를 사용하여 시간 값을 리턴
            string time = response["time"].ToString();

            //시간은 12:11 이런 형식으로 변환
            //string time1 = time.Substring(0, 4);

            if (time.Length >= 6)
            {
                for (int i = 0; i < time.Length; i++)
                {
                    if (time[i] == '.')
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
            }

            else
            {
                for (int i = 0; i < time.Length; i++)
                {
                    if (time[i] == '.')
                    {
                        text += ':';
                    }
                    else
                    {
                        text += time[i];
                    }
                }

                response["time"] = text;
            }


            //Debug
            Debug.Log(response["mode"]);
            Debug.Log(response["map"]);
            Debug.Log(response["players"]);
            Debug.Log(response["rank"]);
            Debug.Log(response["time"]);

            Debug.Log(response["mode"].GetType().Name);
            Debug.Log(response["map"].GetType().Name);
            Debug.Log(response["players"].GetType().Name); // int이므로 string으로 변환해야 함
            Debug.Log(response["rank"].GetType().Name); //얘도
            Debug.Log(response["time"].GetType().Name);

            string textPlayers = response["players"].ToString();
            string textRank = response["rank"].ToString();

            response["players"] = textPlayers;
            response["rank"] = textRank;

            //MyPageItem.cs로 보내기
            Instantiate(gameRecordPrefab, recordListContent).GetComponent<MyPageItem>().SetUp(response);
        }

        public void GetRecords(string username) =>
               FirebaseDatabase.GetRecords(username);

        public void GetRanks(string username) =>
               FirebaseDatabase.GetRanks(username);
    }
}