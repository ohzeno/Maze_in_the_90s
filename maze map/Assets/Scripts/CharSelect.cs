using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class CharSelect : MonoBehaviour
{
    public TMP_Dropdown charselect;

    public Image lobby_image;
    public void OnDropdownEvent(int index)
    {
        {
            GameManager.char_idx = index;
            Debug.Log(index);
            lobby_image.sprite = charselect.captionImage.sprite;
        }
    }
}
