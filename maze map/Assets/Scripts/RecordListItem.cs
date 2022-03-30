using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecordListItem : MonoBehaviour
{
    // 기록 프리팹 설정
    StartGame startgame;
    public TextMeshProUGUI record1;
    public TextMeshProUGUI record2;
    public TextMeshProUGUI record3;

    // Start is called before the first frame update
    void Start()
    {
    }
    public void SetUp(int rank,KeyValuePair<string, string> record)
    {
        Debug.Log(record);
        record1.text = rank.ToString()+"등";
        record2.text = record.Key.ToString();
        record3.text = record.Value.ToString();//플레이어 이름 받아서 그사람 이름이 목록에 뜨게 만들어준다. 
    }

    // Update is called once per frame
    void Update()
    {

    }
}