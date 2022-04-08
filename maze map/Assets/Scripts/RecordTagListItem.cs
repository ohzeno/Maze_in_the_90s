using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecordTagListItem : MonoBehaviour
{
    // ��� ������ ����
    public TextMeshProUGUI record1;
    public TextMeshProUGUI record2;
    public TextMeshProUGUI record3;

    // Start is called before the first frame update
    void Start()
    {
    }
    public void SetUp(int rank, KeyValuePair<string, string> record)
    {
        record1.text =  "#" + rank.ToString();
        record2.text = record.Key.ToString();
        record3.text = record.Value.ToString(); 
    }

    // Update is called once per frame
    void Update()
    {

    }
}
