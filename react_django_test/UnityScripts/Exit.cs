using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Net;
using System.IO;

public class Exit : MonoBehaviour
{
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [System.Serializable]
    public class FormData
    {
        public string data;
    }

    IEnumerator Exit_in(){
        gameManager.UnityCall();
        IEnumerator DelData()
        {
            FormData data1 = new FormData();
            data1.data = "zeno";
            string data2 = JsonUtility.ToJson(data1);
            string GetDataUrl = "http://127.0.0.1:8000/recog/detect/zeno/delete";
            using (UnityWebRequest request = UnityWebRequest.Post(GetDataUrl, data2))
            {
                yield return request.Send();
                if (request.isNetworkError || request.isHttpError) //불러오기 실패 시
                {
                    Debug.Log(request.error);                   
                }
                else
                {
                    if (request.isDone)
                    {
                        Debug.Log(data1.data + "삭제완료");
                    }
                }
            }
        }
        yield return StartCoroutine(DelData());
        SceneManager.LoadScene("test");
    }
    public void Exit_to()
    {
        StartCoroutine(Exit_in());
    }
}
