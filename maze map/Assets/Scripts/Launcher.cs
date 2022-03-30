using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;//���� ��� ���
using TMPro;//�ؽ�Ʈ �޽� ���� ��� ���
using Photon.Realtime;
using System.Linq;

public class Launcher : MonoBehaviourPunCallbacks//�ٸ� ���� ���� �޾Ƶ��̱�
{
    public static Launcher Instance;//Launcher��ũ��Ʈ�� �޼���� ����ϱ� ���� ����

    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_InputField userNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject playerListItemPrefab;
    [SerializeField] GameObject startGameButton;

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
        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
        Debug.Log(PhotonNetwork.NickName);
        //���»�� �̸� �������� ���ںٿ��� �����ֱ�
    }
    public void CreateRoom()//�游���
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;//�� �̸��� ���̸� �� �ȸ������
        }

        if (string.IsNullOrWhiteSpace(userNameInputField.text))
        {
            Debug.Log(PhotonNetwork.NickName);
        }
        else
        {
            PhotonNetwork.NickName = userNameInputField.text;
        }
        PhotonNetwork.CreateRoom(roomNameInputField.text);//���� ��Ʈ��ũ������� roomNameInputField.text�� �̸����� ���� �����.
        MenuManager.Instance.OpenMenu("loading");//�ε�â ����
    }

    public override void OnJoinedRoom()//�濡 ������ �۵�
    {
        MenuManager.Instance.OpenMenu("room");//�� �޴� ����
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;//�� �� �̸�ǥ��
        Player[] players = PhotonNetwork.PlayerList;
        
        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);//�濡 ���� �����ִ� �̸�ǥ�� ����
        }
        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
            //���� �濡 ���� �濡�ִ� ��� ��� ��ŭ �̸�ǥ �߰� �ϱ�
        }
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);//���常 ���ӽ��� ��ư ������ ����
    }

    public override void OnMasterClientSwitched(Player newMasterClient)//������ ������ ������ �ٲ������
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);//���常 ���ӽ��� ��ư ������ ����
    }

    public override void OnCreateRoomFailed(short returnCode, string message)//�� ����� ���н� �۵�
    {
        errorText.text = "Room Creation Failed: " + message;
        MenuManager.Instance.OpenMenu("error");//���� �޴� ����
    }


    public void StartGame()
    {
        PhotonNetwork.LoadLevel("MazeForest1");//1�� ������ ���忡�� scene ��ȣ�� 1�����̱� �����̴�. 0�� �ʱ� ��.
    }

    public void LeaveRoom() // ���� ����
    {
        PhotonNetwork.LeaveRoom();//�涰���� ���� ��Ʈ��ũ ���
        MenuManager.Instance.OpenMenu("loading");//�ε�â ����
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);//���� ��Ʈ��ũ�� JoinRoom��� �ش��̸��� ���� ������ �����Ѵ�. 
        if (string.IsNullOrWhiteSpace(userNameInputField.text))
        {
            Debug.Log(PhotonNetwork.NickName);
        }
        else
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
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
            //instantiate�� prefab�� roomListContent��ġ�� ������ְ� �� �������� i��° �븮��Ʈ�� �ȴ�. 
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)//�ٸ� �÷��̾ �濡 ������ �۵�
    {
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
        //instantiate�� prefab�� playerListContent��ġ�� ������ְ� �� �������� �̸� �޾Ƽ� ǥ��. 
    }
}