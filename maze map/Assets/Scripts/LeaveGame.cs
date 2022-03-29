using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;//포톤 기능 사용
using Photon.Realtime;


public class LeaveGame : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LeaveRoom() // 대기실 퇴장
    {
        PhotonNetwork.LeaveRoom();//방떠나기 포톤 네트워크 기능
    }

    public override void OnLeftRoom()//방을 떠나면 호출
    {
        PhotonNetwork.LoadLevel("Lobby");// Lobby 씬 불러오기
    }
}
