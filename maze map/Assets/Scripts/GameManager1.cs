using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager1 : MonoBehaviour
{
    public static GameManager1 instance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void ChangeScene(string _sceneName)
    {
        SceneManager.LoadSceneAsync(_sceneName);
    }
}