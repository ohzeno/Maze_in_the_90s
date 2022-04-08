using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMenual : MonoBehaviour
{
    public GameObject Menual;
    // Start is called before the first frame update
    void Start()
    {
        Menual.SetActive(true);
    }


    public void Close()
    {
        Menual.SetActive(false);
    }

    public void Open()
    {
        Menual.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
