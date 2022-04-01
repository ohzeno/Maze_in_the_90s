using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;//포톤 기능 사용
using TMPro;//텍스트 메쉬 프로 기능 사용
using Photon.Realtime;
using System.Linq;

public class Launcher : MonoBehaviourPunCallbacks//다른 포톤 반응 받아들이기
{
    public static Launcher Instance;//Launcher스크립트를 메서드로 사용하기 위해 선언

    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_InputField roomPopulationInputField;
    [SerializeField] TMP_Dropdown roomModeDropdown;
    [SerializeField] TMP_Dropdown roomMapDropdown;
    [SerializeField] TMP_Dropdown inRoomModeDropdown;
    [SerializeField] TMP_Dropdown inRoomMapDropdown;
    [SerializeField] TMP_InputField userNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject playerListItemPrefab;
    [SerializeField] GameObject startGameButton;
    [SerializeField] GameObject inGameOptionButton;

    void Awake()
    {
        Instance = this;//메서드로 사용
    }
    void Start()
    {
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();//설정한 포톤 서버에 때라 마스터 서버에 연결
    }

    public override void OnConnectedToMaster()//마스터서버에 연결시 작동됨
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();//마스터 서버 연결시 로비로 연결
        PhotonNetwork.AutomaticallySyncScene = true;//자동으로 모든 사람들의 scene을 통일 시켜준다. 
    }

    public override void OnJoinedLobby()//로비에 연결시 작동
    {
        MenuManager.Instance.OpenMenu("lobby");//로비에 들어오면 타이틀 메뉴 키기
        Debug.Log("Joined Lobby");
        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
        //들어온사람 이름 랜덤으로 숫자붙여서 정해주기
    }
    public void CreateRoom()//방만들기
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;//방 이름이 빈값이면 방 안만들어짐
        }
        if (!string.IsNullOrWhiteSpace(userNameInputField.text))
        {
            PhotonNetwork.NickName = userNameInputField.text;
        }
        string[] propertiesListedInLobby = new string[2];
        propertiesListedInLobby[0] = "Mode";
        propertiesListedInLobby[1] = "Map";

        ExitGames.Client.Photon.Hashtable openWith = new ExitGames.Client.Photon.Hashtable();
        openWith.Add("Mode", roomModeDropdown.value);
        openWith.Add("Map", roomMapDropdown.value);
        PhotonNetwork.CreateRoom(roomNameInputField.text, new RoomOptions
        {
            MaxPlayers = byte.Parse(roomPopulationInputField.text),
            IsVisible = true,
            IsOpen = true,
            CustomRoomProperties = openWith,
            CustomRoomPropertiesForLobby = propertiesListedInLobby
        });//포톤 네트워크기능으로 roomNameInputField.text의 이름으로 방을 만든다.

        MenuManager.Instance.OpenMenu("loading");//로딩창 열기
    }

    public override void OnJoinedRoom()//방에 들어갔을때 작동
    {
        MenuManager.Instance.OpenMenu("room");//룸 메뉴 열기
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;//들어간 방 이름표시
        inRoomModeDropdown.value = (int)PhotonNetwork.CurrentRoom.CustomProperties["Mode"];
        inRoomMapDropdown.value = (int)PhotonNetwork.CurrentRoom.CustomProperties["Map"];
        Player[] players = PhotonNetwork.PlayerList;
        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);//방에 들어가면 전에있던 이름표들 삭제
        }
        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
            //내가 방에 들어가면 방에있는 사람 목록 만큼 이름표 뜨게 하기
        }
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);//방장만 게임시작 버튼 누르기 가능
        //inGameOptionButton.SetActive(PhotonNetwork.IsMasterClient);//방장만 게임시작 버튼 누르기 가능
    }

    public override void OnMasterClientSwitched(Player newMasterClient)//방장이 나가서 방장이 바뀌었을때
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);//방장만 게임시작 버튼 누르기 가능
        //inGameOptionButton.SetActive(PhotonNetwork.IsMasterClient);//방장만 게임시작 버튼 누르기 가능
    }

    public override void OnCreateRoomFailed(short returnCode, string message)//방 만들기 실패시 작동
    {
        errorText.text = "Room Creation Failed: " + message;
        MenuManager.Instance.OpenMenu("error");//에러 메뉴 열기
    }


    public void StartGame()
    {
        if ((int)PhotonNetwork.CurrentRoom.CustomProperties["Mode"] == 0)
        {
            PhotonNetwork.LoadLevel(MapDropdown.maze_list[(int)PhotonNetwork.CurrentRoom.CustomProperties["Map"]]);//1인 이유는 빌드에서 scene 번호가 1번씩이기 때문이다. 0은 초기 씬.
        }
        else
        {
            PhotonNetwork.LoadLevel(MapDropdown.hideAndSeek_list[(int)PhotonNetwork.CurrentRoom.CustomProperties["Map"]]);//1인 이유는 빌드에서 scene 번호가 1번씩이기 때문이다. 0은 초기 씬.
        }
    }

    public void LeaveRoom() // 대기실 퇴장
    {
        PhotonNetwork.LeaveRoom();//방떠나기 포톤 네트워크 기능
        MenuManager.Instance.OpenMenu("loading");//로딩창 열기
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);//포톤 네트워크의 JoinRoom기능 해당이름을 가진 방으로 접속한다. 
        if (!string.IsNullOrWhiteSpace(userNameInputField.text))
        {
            PhotonNetwork.NickName = userNameInputField.text;

        }
        MenuManager.Instance.OpenMenu("loading");//로딩창 열기
    }

    public override void OnLeftRoom()//방을 떠나면 호출
    {
        MenuManager.Instance.OpenMenu("lobby");//방떠나기 성공시 타이틀 메뉴 호출
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
            if (roomList[i].PlayerCount != roomList[i].MaxPlayers)
            {
                Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
            }
            //instantiate로 prefab을 roomListContent위치에 만들어주고 그 프리펩은 i번째 룸리스트가 된다. 
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)//다른 플레이어가 방에 들어오면 작동
    {
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
        //instantiate로 prefab을 playerListContent위치에 만들어주고 그 프리펩을 이름 받아서 표시. 
    }
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);
        inRoomModeDropdown.value = (int)PhotonNetwork.CurrentRoom.CustomProperties["Mode"];
        inRoomMapDropdown.value = (int)PhotonNetwork.CurrentRoom.CustomProperties["Map"];
    }
}
