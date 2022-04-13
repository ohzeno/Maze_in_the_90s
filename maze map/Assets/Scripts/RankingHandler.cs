using UnityEngine;
using UnityEngine.UI;
using FirebaseWebGL.Scripts.FirebaseBridge;
using FirebaseWebGL.Scripts.Objects;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MiniJSON;


public class RankingHandler : MonoBehaviour
{
        
    public static RankingHandler instance;
    
    //�ؿ� ������ ���� ������Ʈ
    [SerializeField] Transform recordListContent;
    //������
    [SerializeField] GameObject gameRecordPrefab;

    //JSON���� ��ȯ�� ���ӱ�ϵ�����(���̾�̽��� ���� ��)
    [Serializable]
    public class gameRecord
    {
        public int totalPlayers;
        public int rank;
        public string gameMode;       
        public string gameMap;
        public string nickName;
        public float time;
    }

    //������Ʈ�� ��ȯ�� ���ӱ�ϵ�����(���̾�̽����� �޾��� ��)
    [Serializable]
    public class saveRecord
    {
        public string nickName;
        public string time;
    }

    int startIdx = 0;

    void Awake()
    {
        instance = this;
    }

    //������ ������ �� EndGamd���� ���� ���ӵ�����
    public void GetGameData(int players, int rank, KeyValuePair<string, string> _data, int mode, int map)
    {
        Debug.Log("!!!!!!!!!!!");
        Debug.Log(players);
        Debug.Log(rank);
        Debug.Log(mode);
        Debug.Log(map);
        //{name: ��¼��, time: ��¼��} �̷��� ����
        //string���� �޾Ƽ� float �� ��ȯ
        float recordToFlaot = (float.Parse(_data.Value));
        SendGameRecord(players, rank, _data.Key, recordToFlaot, mode, map);
    }

    //���ӵ����� ����, JSON ��ȯ, ����
    public void SendGameRecord(int players, int rank, string username, float time, int mode, int map)
    {

        gameRecord gameRecordObject = new gameRecord();

        //MapDropdown.cs�� �ִ� �� ���� ����
        //public static string[] mode_list = new string[3] { "-", "Maze", "Hide and Seek" };
        //public static string[] maze_list = new string[6] { "-", "MazeForest1", "MazeForest2", "MazeForest3", "MazeForest4", "MazeGrave1" };
        //public static string[] hideAndSeek_list = new string[3] { "-", "MazeForest1", "MazeForest2" };

        gameRecordObject.totalPlayers = players;
        gameRecordObject.rank = rank;
        gameRecordObject.nickName = username;
        gameRecordObject.time = time;
        gameRecordObject.gameMode = MapDropdown.mode_list[mode];

        if (mode == 1)
        {
            gameRecordObject.gameMap = MapDropdown.maze_list[map];
        }
        else if (mode == 2)
        {
            gameRecordObject.gameMap = MapDropdown.hideAndSeek_list[map];
        }

        string json = JsonUtility.ToJson(gameRecordObject);

        //���̾�̽��� ����
        if (username == FirebaseWebGL.Examples.Auth.LobbyHandler.userName)
        {
            FirebaseDatabase.PostMyRecord(json);
        }

        FirebaseDatabase.PostGameRecord(json);
    }

    //TOP10 �����͸� �޾Ƽ� ��ŷ ���� ����(��Ŭ���ϸ� ����)
    public void SetUp(string record)
    {
        Debug.Log("setup start!");
        var text = "";

        //JSON ���ڿ� ���¿��� �ٽ� Deserialize
        Dictionary<string, object> response = Json.Deserialize(record) as Dictionary<string, object>;

        //�ε����� ����Ͽ� �ð� ���� ����
        string time = response["time"].ToString();

        //�ð��� 12:11 �̷� �������� ��ȯ
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

        //������ �߰�
        response["idx"] = (startIdx + 1).ToString();
        startIdx += 1;

        //Debug
        Debug.Log(response["name"]);
        Debug.Log(response["time"]);
        Debug.Log(response["idx"]);

        Debug.Log(response["name"].GetType().Name);
        Debug.Log(response["time"].GetType().Name);
        Debug.Log(response["idx"].GetType().Name);

        //RecordListItem.cs�� ������
        Instantiate(gameRecordPrefab, recordListContent).GetComponent<RankListItem>().SetUp(response);
    }

    //�ʱ�ȭ
    public void ClearContents()
    {
        Debug.Log("clearing start!");
        
        //���� �����յ��� ����
 
        if (recordListContent.transform.childCount > 0)
        {
            for (int i = 0; i < recordListContent.transform.childCount; i++)
                Destroy(recordListContent.transform.GetChild(i).gameObject);
        }
        
        //��ŷ �ε��� �ʱ�ȭ
        startIdx = 0;
        Debug.Log(startIdx);
        Debug.Log("layout cleared!");
    }

    //�� Ŭ�� (���, �ʸ��� �ٸ� ��û)
     public void ChangeTab(string mode, string map)
    {
        FirebaseDatabase.SetByInfo(mode, map);
        Debug.Log("go to firebase");
    }


    public void LobbyorLoginScreen()
    {
        //�α��� �������� Ȯ��
        FirebaseAuth.IsLoggedIn();
    }

    public void BackBtn(int status)
    {
        if (status == 1)
        {
            GameManager.instance.ChangeScene("Lobby");
            CheckAuthState();
        }

        else
        {
            GameManager.instance.ChangeScene("Login");
        }
    }

    public void CheckAuthState() =>
           FirebaseAuth.CheckAuthState();
}
