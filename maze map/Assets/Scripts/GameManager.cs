using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static Dictionary<string, string> records = new Dictionary<string, string>();
    public static List<object> dataList = new List<object>();
    public static double startTime;
    public bool isConnect = false;
    private string[] CharList = new string[] { "Baker", "CafeMaid", "Casual", "Cop", "Dog", "FElder", "FStudent", "FWorker", "FYouth", "MElder", "MStudent1", "MStudent2", "MWorker", "MYouth", "Punk", "Traditional", "Trendy", "Witch3" };

    public static bool[] Tagged = new bool[7];
    public static int mykey;
    public static bool tagger = false;
    private Dictionary<int, (int, int)> HideAndSeekPos = new Dictionary<int, (int, int)>()
    {
        {1,(3575,1226) },
        {2,(5117,1226) },
        {3,(2840,-212) },
        {4,(5837,-212) },
        {5,(3575,-1650) },
        {6,(5117,-1650) },
    };
    private int x;
    private int y;

    public static int char_idx = 0;

    void Start()
    {
        isConnect = true;
        StartCoroutine(CreatePlayer());
    }

    IEnumerator CreatePlayer()
    {
        yield return new WaitUntil(() => isConnect);
        if ((int)PhotonNetwork.CurrentRoom.CustomProperties["Mode"] == 1)
        {
            Vector3 pos = new Vector3(-1030 + Random.Range(-150, 150) * 1.0f, 800 + Random.Range(-80, 80) * 1.0f, 0.0f);
            GameObject playerTemp = PhotonNetwork.Instantiate(CharList[char_idx], pos, Quaternion.identity, 0);
        }
        else
        {
            foreach (int i in PhotonNetwork.CurrentRoom.Players.Keys)
            {
                if (PhotonNetwork.NickName + "'" == PhotonNetwork.CurrentRoom.Players[i].ToString().Substring(5))
                {
                    mykey = i;
                    (x, y) = HideAndSeekPos[i];
                    Vector3 pos = new Vector3(x * 1.0f, y * 1.0f, 0.0f);
                    if (PhotonNetwork.NickName + "'" == PhotonNetwork.CurrentRoom.Players[(int)PhotonNetwork.CurrentRoom.CustomProperties["Tagger"]].ToString().Substring(5))
                    {
                        tagger = true;
                        GameObject playerTemp = PhotonNetwork.Instantiate("Template", pos, Quaternion.identity, 0);
                    }
                    else
                    {
                        GameObject playerTemp = PhotonNetwork.Instantiate(CharList[char_idx], pos, Quaternion.identity, 0);
                    }
                    break;
                }
            }
        }
    }
    public void ChangeScene(string _sceneName)
    {
        SceneManager.LoadSceneAsync(_sceneName);
    }

    void Update()
    {

    }
}
