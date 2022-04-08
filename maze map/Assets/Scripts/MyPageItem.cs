using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

public class MyPageItem : MonoBehaviour
{
    // 기록 프리팹 설정
    [SerializeField]
    private Text modeText;
    [SerializeField]
    private Text mapText;
    [SerializeField]
    private Text playersText;
    [SerializeField]
    private Text rankText;
    [SerializeField]
    private Text timeText;

    // Start is called before the first frame update
    void Start()
    {
    }


    public void SetUp(Dictionary<string, object> _info)// 랭킹 정보 받아오기
    {
        string strA = (string)_info["mode"];
        string strB = (string)_info["map"];
        string strC = (string)_info["players"];
        string strD = (string)_info["rank"];
        string strE = (string)_info["time"];

        modeText.text = strA;
        mapText.text = strB;
        playersText.text = strC;
        rankText.text = strD;
        timeText.text = strE;
    }

    // Update is called once per frame
    void Update()
    {

    }
}