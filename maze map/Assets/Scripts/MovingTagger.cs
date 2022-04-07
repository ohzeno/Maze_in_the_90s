using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
public class MovingTagger : MonoBehaviour
{

    public static MovingTagger instance;
    private BoxCollider2D boxCollider;
    public LayerMask layerMask;
    private PhotonView pv;

    public float speed;
    public int walkCount;
    private int currentWalkCount;

    private Vector3 vector;
    private Animator animator;

    public float runSpeed;
    private float applyRunSpeed;
    private bool applyRunFlag = false;
    private bool canMove = true;

    public float turnSpeed = 0.0f;
    public float turnSpeedValue = 200.0f;
    public GameObject FinishAlert;
    AudioSource audioSrc;

    RaycastHit hit;
    IEnumerator Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        pv = GetComponent<PhotonView>();
        audioSrc = GetComponent<AudioSource>();

        Destroy(GameObject.Find("StartInputBox"), 5);

        turnSpeed = 0.0f;
        yield return new WaitForSeconds(0.5f);

        if (pv.IsMine)
        {
            Camera.main.GetComponent<CameraManager>().target = transform.Find("CamPivot").transform;
        }
        else
        {
            GetComponent<Rigidbody2D>().isKinematic = true;
        }
        turnSpeed = turnSpeedValue;
    }

    IEnumerator MoveCoroutine()
    {
        pv = GetComponent<PhotonView>();
        if (pv.IsMine)
        {
            while (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0)
            {

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    applyRunSpeed = runSpeed;
                    applyRunFlag = true;
                }
                else
                {
                    applyRunSpeed = 0;
                    applyRunFlag = false;
                }


                vector.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), transform.position.z);

                if (vector.x != 0)
                    vector.y = 0;


                animator.SetFloat("DirX", vector.x);
                animator.SetFloat("DirY", vector.y);

                RaycastHit2D hit;
                Vector2 start = transform.position;
                Vector2 end = start + new Vector2(vector.x * speed * walkCount, vector.y * speed * walkCount);

                boxCollider.enabled = false;
                hit = Physics2D.Linecast(start, end, layerMask);
                boxCollider.enabled = true;

                if (hit.transform != null)
                    break;

                animator.SetBool("Walking", true);
                audioSrc.Play();
                while (currentWalkCount < walkCount)
                {
                    transform.Translate(vector.x * (speed + applyRunSpeed), vector.y * (speed + applyRunSpeed), 0);
                    if (applyRunFlag)
                        currentWalkCount++;
                    currentWalkCount++;
                    yield return new WaitForSeconds(0.01f);
                }
                currentWalkCount = 0;


            }
            animator.SetBool("Walking", false);
            canMove = true;
            audioSrc.Stop();
        }
    }



    // Update is called once per frame
    void Update()
    {

        if (canMove)
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                canMove = false;
                StartCoroutine(MoveCoroutine());
            }
        }
    }

}