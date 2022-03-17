using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MovingScenes : MonoBehaviour
{
    public void MovingtoMapScene()
    {
        SceneManager.LoadScene("MapScene");
    }
    
}
