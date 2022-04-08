using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;//포톤 기능 사용
using Photon.Realtime;
using System.Runtime.InteropServices;
using UnityEngine.Networking;

public class LeaveGame : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;

    [DllImport("__Internal")]
    private static extern void RemoveCam();
    private string uid;
    [System.Serializable]
    public class FormData
    {
        public string data;
    }

    IEnumerator Exit_in()
    {
        IEnumerator DelData()
        {
            uid = FirebaseWebGL.Examples.Auth.LoginHandler.UserUid;
            FormData data1 = new FormData();
            data1.data = uid;
            string data2 = JsonUtility.ToJson(data1);
            string GetDataUrl = $"https://j6e101.p.ssafy.io/recog/detect/{uid}/delete";
            //string GetDataUrl = $"http://127.0.0.1:8000/recog/detect/{uid}/delete";
            using (UnityWebRequest request = UnityWebRequest.Post(GetDataUrl, data2))
            {
                yield return request.Send();
                if (request.isNetworkError || request.isHttpError) //불러오기 실패 시
                {
                    Debug.Log(request.error);
                }
                else
                {
                    if (request.isDone)
                    {
                        Debug.Log(data1.data + "삭제완료");
                    }
                }
            }
        }
        yield return StartCoroutine(DelData());
        RemoveCam();
    }
    public void Exit_to()
    {
        StartCoroutine(Exit_in());

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LeaveTutorial()
    {
        Exit_to();
        PhotonNetwork.LoadLevel("Lobby");
    }

    public void LeaveRoom() // 대기실 퇴장
    {
        PhotonNetwork.LeaveRoom();//방떠나기 포톤 네트워크 기능
        Exit_to();        
    }

    public override void OnLeftRoom()//방을 떠나면 호출
    {
        PhotonNetwork.LoadLevel("Lobby");// Lobby 씬 불러오기
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)//포톤의 룸 리스트 기능
    {
        foreach (Transform trans in roomListContent)//존재하는 모든 roomListContent
        {
            Destroy(trans.gameObject);//룸리스트 업데이트가 될때마다 싹지우기
        }
        for (int i = 0; i < roomList.Count; i++)//방갯수만큼 반복
        {
            if (roomList[i].RemovedFromList)//사라진 방은 취급 안한다. 
                continue;
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
            //instantiate로 prefab을 roomListContent위치에 만들어주고 그 프리펩은 i번째 룸리스트가 된다. 
        }
    }
}
