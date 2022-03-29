using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class NicknameUI : MonoBehaviour, IPunObservable
{
    public TextMeshProUGUI txt;
    private void Start()
    {
        txt.text = PhotonNetwork.NickName;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) stream.SendNext(txt.text);
        else txt.text = (string)stream.ReceiveNext();
    }
}
