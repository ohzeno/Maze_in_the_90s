using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MovingScenes : MonoBehaviour
{
    public string SceneSelect;
    public void MovingScene()
    {
        SceneManager.LoadScene(SceneSelect);
    }
}
