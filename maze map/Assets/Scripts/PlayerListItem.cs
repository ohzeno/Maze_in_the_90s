using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerListItem : MonoBehaviourPunCallbacks, IPunObservable//다른 포톤 반응 받아들이기 
{
    [SerializeField] TMP_Text text;
    public TMP_Dropdown charselect;
    Player player;//포톤 리얼타임은 Player를 선언 할 수 있게 해준다.
    public GameObject dropbox;
    public int photon_index;

    void Start()
    {
        charselect.value = GameManager.char_idx;
    }
    public void OnDropdownEvent(int index)
    {
        GameManager.char_idx = index;
    }

    public void SetUp(Player _player)
    {
        player = _player;
        text.text = _player.NickName;//플레이어 이름 받아서 그사람 이름이 목록에 뜨게 만들어준다. 

        if (_player.NickName != PhotonNetwork.NickName)
        {
            TMP_Dropdown tMP_Dropdown = dropbox.GetComponent<TMP_Dropdown>();
            tMP_Dropdown.enabled = false;
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)//플레이어가 방떠났을때 호출
    {
        if (player == otherPlayer)//나간 플레이어가 나면?
        {
            Destroy(gameObject);//이름표 삭제
        }
    }

    public override void OnLeftRoom()//방 나가면 호출
    {
        Destroy(gameObject);//이름표 호출
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(charselect.options[photon_index].text);
            // stream.SendNext(charselect.options[photon_index].image);
        }
        if (stream.IsReading)
        {
            charselect.captionText.text = (string)stream.ReceiveNext();
            //charselect.captionImage = (Image)stream.ReceiveNext();
        }
    }

}