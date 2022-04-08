using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    public int count = 0;
    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public Text text;
    public Animator animStartUi;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ShowDialogue()
    {
        StartCoroutine(StartDialogueCoroutine());
    }

    IEnumerator StartDialogueCoroutine()
    {
        animStartUi.SetBool("appear", true);
        yield return new WaitForSeconds(0.01f);
    }

    public void ExitDialogue()
    {
        animStartUi.SetBool("appear", false);
        count = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            count++;
            if(count == 1)
                {
                StopAllCoroutines();
                ExitDialogue();
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(StartDialogueCoroutine());
            }
        }
    }
}
