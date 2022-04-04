using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [DllImport("__Internal")]
    private static extern void CallCam();
    public Text sampleText;

    private void Awake() {
        if (instance == null) { 
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        } 
        else { 
            Destroy(this.gameObject); 
        }
    }
    public void UnityCall()
    {
# if UNITY_WEBGL == true && UNITY_EDITOR == false
    CallCam();
#endif
    }

    public void GameStart()
    {
        SceneManager.LoadScene("SampleScene");
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "SampleScene") {
            Debug.Log("게임매니저 씬로드확인");
            CallCam();
        };
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
