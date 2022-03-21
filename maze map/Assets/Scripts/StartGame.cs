using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public GameObject StartBtn;
    public bool timeActive = false;
    public float CountTime;
    public Text text_Timer;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Witch3")
        {
            StartBtn.SetActive(false);
            timeActive = true;
        }
    }

    private void Update()
    {
        if (StartBtn.activeSelf == false)
        {
            if (timeActive)
            {
                CountTime += Time.deltaTime;
                text_Timer.text = "½Ã°£ : " + CountTime.ToString("F2");
            }
        }
    }
}
