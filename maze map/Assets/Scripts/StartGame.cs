using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;//포톤 기능 사용

public class StartGame : MonoBehaviour
{
    public GameObject StartBtn;
    public bool timeActive = false;
    public float CountTime;
    public Text text_Timer;
    public int lifetime = 5;
    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (StartBtn.activeSelf == true)
        {
            if (collision.gameObject.name == "Witch3(Clone)")
            {
                StartBtn.SetActive(false);
                StartCoroutine("timer");
            }
        }
    }
    IEnumerator timer()
    {
        Debug.Log(Time.time);
        Destroy(GameObject.Find("StartLine"), lifetime);
        yield return StartCoroutine("timeStart");
    }
    IEnumerator timeStart()
    {
        Debug.Log(Time.time);
        timeActive = true;
        yield return null;
    }

    private void Update()
    {
        if (StartBtn.activeSelf == false)
        {
            if (timeActive)
            {
                CountTime += Time.deltaTime;
                text_Timer.text = "Time : " + CountTime.ToString("F2");
            }
        }
    }
}