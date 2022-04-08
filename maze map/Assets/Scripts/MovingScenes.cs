using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class MovingScenes : MonoBehaviour
{
    public GameObject ControlBox;
    public string SceneSelect;
    public void MovingScene(string _control)
    {
        Jscall.controlmode = _control;
        PhotonNetwork.LoadLevel(SceneSelect);
    }

    public void ControlSelect(string _control)
    {
        Jscall.controlmode = _control;
        ControlBox.SetActive(false);
    }
}
