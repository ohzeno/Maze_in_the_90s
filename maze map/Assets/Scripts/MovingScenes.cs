using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class MovingScenes : MonoBehaviour
{
    public string SceneSelect;
    public void MovingScene()
    {
        PhotonNetwork.LoadLevel(SceneSelect);
    }
}
