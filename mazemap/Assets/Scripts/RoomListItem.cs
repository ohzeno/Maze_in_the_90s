using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;//텍스트 메쉬 프로 기능 사용
using UnityEngine;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    // Start is called before the first frame update
    public RoomInfo info;
    public void SetUp(RoomInfo _info)// 방정보 받아오기
    {
        info = _info;
        text.text = _info.Name;
    }

    // Update is called once per frame
    public void OnClick()
    {
        Launcher.Instance.JoinRoom(info);//런처스크립트 메서드로 JoinRoom 실행
    }
}
