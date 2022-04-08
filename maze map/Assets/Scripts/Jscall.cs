using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;

public class Jscall : MonoBehaviour
{
    public static Jscall instance;
    List<string> maplist = new List<string>{"MazeForest1", "MazeForest2", "MazeForest3", "MazeForest4", "MazeGrave1", "MazeForest1", "MazeForest2" };
    private string uid;
    public static string controlmode = "hand";
      
    [DllImport("__Internal")]
    private static extern void CallCam(string _uid);

    private void Awake() {
        if (instance == null) { 
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        } 
        else { 
            Destroy(this.gameObject); 
        }
    }
    /* public void UnityCall()
    {
# if UNITY_WEBGL == true && UNITY_EDITOR == false
    CallCam();
#endif
    } */


    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {       
        /*if (maplist.Contains(scene.name)) {
            Debug.Log("게임매니저 씬로드확인");
            uid = FirebaseWebGL.Examples.Auth.LoginHandler.UserUid;
            CallCam(uid);
        };*/
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
