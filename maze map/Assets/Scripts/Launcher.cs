using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;//���� ��� ���
using TMPro;//�ؽ�Ʈ �޽� ���� ��� ���
using Photon.Realtime;
using System.Linq;

public class Launcher : MonoBehaviourPunCallbacks//�ٸ� ���� ���� �޾Ƶ��̱�
{
    public static Launcher Instance;//Launcher��ũ��Ʈ�� �޼���� ����ϱ� ���� ����
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

    public Jscall jscall;
    void Awake()
    {
        Instance = this;//�޼���� ���
    }
    void Start()
    {
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();//������ ���� ������ ���� ������ ������ ����
    }

    public override void OnConnectedToMaster()//�����ͼ����� ����� �۵���
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();//������ ���� ����� �κ�� ����
        PhotonNetwork.AutomaticallySyncScene = true;//�ڵ����� ��� ������� scene�� ���� �����ش�. 
    }

    public override void OnJoinedLobby()//�κ� ����� �۵�
    {
        MenuManager.Instance.OpenMenu("lobby");//�κ� ������ Ÿ��Ʋ �޴� Ű��
        Debug.Log("Joined Lobby");
        //PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
        PhotonNetwork.NickName = FirebaseWebGL.Examples.Auth.LobbyHandler.userName;
        Debug.Log(PhotonNetwork.NickName);

        //���»�� �̸� �������� ���ںٿ��� �����ֱ�
    }
    public void CreateRoom()//�游���
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;//�� �̸��� ���̸� �� �ȸ������
        }
        if (byte.Parse(roomPopulationInputField.text) > 6)
        {
            return;
        }
        if (!string.IsNullOrWhiteSpace(userNameInputField.text))
        {
            PhotonNetwork.NickName = userNameInputField.text;
        }
        string[] propertiesListedInLobby = new string[3];
        propertiesListedInLobby[0] = "Mode";
        propertiesListedInLobby[1] = "Map";
        propertiesListedInLobby[2] = "Tagger";


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
        });//���� ��Ʈ��ũ������� roomNameInputField.text�� �̸����� ���� �����.

        MenuManager.Instance.OpenMenu("loading");//�ε�â ����
    }

    public override void OnJoinedRoom()//�濡 ������ �۵�
    {
        GameManager.records = new Dictionary<string, string>();
        MenuManager.Instance.OpenMenu("room");//�� �޴� ����
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;//�� �� �̸�ǥ��
        inRoomModeDropdown.value = (int)PhotonNetwork.CurrentRoom.CustomProperties["Mode"];
        inRoomMapDropdown.value = (int)PhotonNetwork.CurrentRoom.CustomProperties["Map"];
        Player[] players = PhotonNetwork.PlayerList;
        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);//�濡 ���� �����ִ� �̸�ǥ�� ����
        }
        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);

            //GameObject dassadasf = PhotonNetwork.Instantiate("PlayerListItem", Vector3.one, Quaternion.identity);
            //dassadasf.transform.parent = playerListContent.transform;
            //dassadasf.GetComponent<PlayerListItem>().SetUp(players[i]);
            //���� �濡 ���� �濡�ִ� ��� ��� ��ŭ �̸�ǥ �߰� �ϱ�

        }
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);//���常 ���ӽ��� ��ư ������ ����
        //inGameOptionButton.SetActive(PhotonNetwork.IsMasterClient);//���常 ���ӽ��� ��ư ������ ����
    }

    public override void OnMasterClientSwitched(Player newMasterClient)//������ ������ ������ �ٲ������
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);//���常 ���ӽ��� ��ư ������ ����
        //inGameOptionButton.SetActive(PhotonNetwork.IsMasterClient);//���常 ���ӽ��� ��ư ������ ����
    }

    public override void OnCreateRoomFailed(short returnCode, string message)//�� ����� ���н� �۵�
    {
        errorText.text = "Room Creation Failed: " + message;
        MenuManager.Instance.OpenMenu("error");//���� �޴� ����
    }


    public void StartGame()
    {
        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.CurrentRoom.IsOpen = false;
        if ((int)PhotonNetwork.CurrentRoom.CustomProperties["Mode"] == 1)
        {
            PhotonNetwork.LoadLevel(MapDropdown.maze_list[(int)PhotonNetwork.CurrentRoom.CustomProperties["Map"]]);//1�� ������ ���忡�� scene ��ȣ�� 1�����̱� �����̴�. 0�� �ʱ� ��.
        }
        else if((int)PhotonNetwork.CurrentRoom.CustomProperties["Mode"] == 2)
        {
            int a = Random.Range(1, PhotonNetwork.CurrentRoom.Players.Count + 1);
            PhotonNetwork.CurrentRoom.CustomProperties["Tagger"] = a;
            PhotonNetwork.CurrentRoom.SetCustomProperties(PhotonNetwork.CurrentRoom.CustomProperties);
            PhotonNetwork.LoadLevel(MapDropdown.hideAndSeek_list[(int)PhotonNetwork.CurrentRoom.CustomProperties["Map"]]);//1�� ������ ���忡�� scene ��ȣ�� 1�����̱� �����̴�. 0�� �ʱ� ��.
        }        
        
    }

    public void LeaveRoom() // ���� ����
    {
        PhotonNetwork.LeaveRoom();//�涰���� ���� ��Ʈ��ũ ���
        MenuManager.Instance.OpenMenu("loading");//�ε�â ����
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);//���� ��Ʈ��ũ�� JoinRoom��� �ش��̸��� ���� ������ �����Ѵ�. 
        if (!string.IsNullOrWhiteSpace(userNameInputField.text))
        {
            PhotonNetwork.NickName = userNameInputField.text;

        }
        MenuManager.Instance.OpenMenu("loading");//�ε�â ����
    }

    public override void OnLeftRoom()//���� ������ ȣ��
    {
        MenuManager.Instance.OpenMenu("lobby");//�涰���� ������ Ÿ��Ʋ �޴� ȣ��
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)//������ �� ����Ʈ ���
    {
        foreach (Transform trans in roomListContent)//�����ϴ� ��� roomListContent
        {
            Destroy(trans.gameObject);//�븮��Ʈ ������Ʈ�� �ɶ����� �������
        }
        for (int i = 0; i < roomList.Count; i++)//�氹����ŭ �ݺ�
        {
            if (roomList[i].RemovedFromList)//����� ���� ��� ���Ѵ�. 
                continue;
            if (roomList[i].PlayerCount != roomList[i].MaxPlayers)
            {
                Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
            }
            //instantiate�� prefab�� roomListContent��ġ�� ������ְ� �� �������� i��° �븮��Ʈ�� �ȴ�. 
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)//�ٸ� �÷��̾ �濡 ������ �۵�
    {
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
        //instantiate�� prefab�� playerListContent��ġ�� ������ְ� �� �������� �̸� �޾Ƽ� ǥ��. 
    }
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);
        inRoomModeDropdown.value = (int)PhotonNetwork.CurrentRoom.CustomProperties["Mode"];
        inRoomMapDropdown.value = (int)PhotonNetwork.CurrentRoom.CustomProperties["Map"];
    }
}
