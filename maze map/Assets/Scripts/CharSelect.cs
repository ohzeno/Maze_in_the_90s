using System.Collections;
using System.Collections.Generic;
using FirebaseWebGL.Scripts.FirebaseBridge;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class CharSelect : MonoBehaviour
{
    public TMP_Dropdown charselect;

    public Image lobby_image;

    public void Start()
    {
        Debug.Log("set character");
        charselect.value = 3;
        //charselect.value = FirebaseWebGL.Examples.Auth.LobbyHandler.UserChar;
    }

    public void OnDropdownEvent(int index)
    {

        GameManager.char_idx = index;
        Debug.Log(index);
        lobby_image.sprite = charselect.captionImage.sprite;
    }

    public void UpdateCharacter(int charIdx) =>
               FirebaseDatabase.UpdateCharacter(charIdx);
}