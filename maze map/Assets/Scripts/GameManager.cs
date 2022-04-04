using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class GameManager : MonoBehaviour
{
    public static Dictionary<string, string> records = new Dictionary<string, string>();
    public static double startTime;
    public bool isConnect = false;
    private string[] CharList = new string[] { "Baker", "CafeMaid", "Casual", "Cop", "Dog", "FElder", "FStudent", "FWorker", "FYouth", "MElder", "MStudent1", "MStudent2", "MWorker", "MYouth", "Punk", "Template", "Traditional", "Trendy", "Witch3" };

    void Start()
    {        
        isConnect = true;
        StartCoroutine(CreatePlayer());
    }

    IEnumerator CreatePlayer()
    {
        yield return new WaitUntil(() => isConnect);
        Vector3 pos = new Vector3(-1030 + Random.Range(-150, 150) * 1.0f, 800 + Random.Range(-80, 80) * 1.0f, 0.0f);
        GameObject playerTemp = PhotonNetwork.Instantiate(CharList[PlayerListItem.char_index], pos, Quaternion.identity, 0);
    }

    void Update()
    {
        
    }
}
