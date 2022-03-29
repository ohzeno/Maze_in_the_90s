using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public bool isConnect = false;
    public Transform[] spawnPoints;


    // Start is called before the first frame update
    void Start()
    {
        isConnect = true;
        StartCoroutine(CreatePlayer());
    }

    IEnumerator CreatePlayer()
    {
        yield return new WaitUntil(() => isConnect);
        spawnPoints = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();

        Vector3 pos = spawnPoints[Random.Range(0,PhotonNetwork.CurrentRoom.PlayerCount)].position;
        Quaternion rot = spawnPoints[PhotonNetwork.CurrentRoom.PlayerCount].rotation;
        GameObject playerTemp = PhotonNetwork.Instantiate("Witch3", pos,rot, 0);
    }

    void Update()
    {
        
    }
}
