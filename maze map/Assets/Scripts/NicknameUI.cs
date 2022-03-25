using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NicknameUI : MonoBehaviour
{
    public TextMeshProUGUI nickname;
    public Camera camera;
    private Transform target;
    // Start is called before the first frame update
    void Start()
    {
        target = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetposition = camera.WorldToScreenPoint(target.position);
        float x = targetposition.x;

        nickname.transform.position = new Vector3(x, targetposition.y + 50, nickname.transform.position.z);
    }
}
