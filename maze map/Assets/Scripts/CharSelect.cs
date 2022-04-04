using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class CharSelect : MonoBehaviour, IPunObservable
{
    private PhotonView pv;

    public TMP_Dropdown charselect;

    public static int char_index;
    private void Start()
    {
        pv = GetComponent<PhotonView>();
        charselect.value = 0;
    }
    
    public void OnDropdownEvent(int index)
    {
        if (pv.IsMine)
        {
            char_index = index;
        }        
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(charselect.options[char_index].text);
            // stream.SendNext(charselect.options[char_index].image);
        }
        if (stream.IsReading)
        {
            charselect.captionText.text = (string)stream.ReceiveNext();
            //charselect.captionImage = (Image)stream.ReceiveNext();
        }
    }
}
