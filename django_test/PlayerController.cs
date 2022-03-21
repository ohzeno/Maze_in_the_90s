using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Text;
using System;
using System.IO;

public class PlayerController : MonoBehaviour
{

    public static PlayerController instance;

    private BoxCollider2D boxCollider;
    public LayerMask layerMask;

    public float speed = 2.4f;
    public int walkCount = 20;
    private int currentWalkCount;

    private Vector3 vector;
    private Animator animator;

    public float runSpeed = 2.4f;
    private float applyRunSpeed;
    private bool applyRunFlag = false;
    private bool canMove = true;
    private float dirH = 0;
    private float dirV = 0;

    // 여기부터 웹소켓
    TcpClient client;
    string serverIP = "127.0.0.1";
    int port = 8000;
    byte[] receivedBuffer;
    StreamReader reader;
    bool socketReady = false;
    NetworkStream stream;

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
    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        CheckReceive();
    }

    void FixedUpdate() 
    { 
        if(socketReady) 
        { 
            if (stream.DataAvailable) 
            { 
                receivedBuffer = new byte[100];
                // stream에 있던 바이트배열 내려서 새로 선언한 바이트배열에 넣기 
                stream.Read(receivedBuffer, 0, receivedBuffer.Length);
                // byte[] to string 
                string msg = Encoding.UTF8.GetString(receivedBuffer, 0, receivedBuffer.Length).Substring(0,5);
                Debug.Log(msg);
                if (msg.Substring(0,2) == "Up") {
                    Debug.Log("Up들어옴");
                    dirV = 1;
                    dirH = 0;
                } else if (msg.Substring(0,4) == "Down") {
                    Debug.Log("Down들어옴");
                    dirV = -1;
                    dirH = 0;
                } else if (msg.Substring(0,4) == "Left") {
                    Debug.Log("Left들어옴");
                    dirV = 0;
                    dirH = -1;
                } else if (msg == "Right") {
                    Debug.Log("Right들어옴");
                    dirV = 0;
                    dirH = 1;
                } else if (msg.Substring(0,4) == "Stop") {
                    Debug.Log("Stop들어옴");
                    dirV = 0;
                    dirH = 0;
                }
            }
        }
        // if (canMove)
        // {
        //     if (dirV != 0 || dirH != 0)
        //     {
        //         canMove = false;
        //         StartCoroutine(MoveCoroutine());
        //     }
        // }
        

    } 

    void CheckReceive() 
    { 
        if (socketReady) 
            return;
        try 
        { 
            client = new TcpClient(serverIP, port);
            if (client.Connected) 
            { 
                stream = client.GetStream();
                Debug.Log("Connect Success");
                socketReady = true;
            } 
        } 
        catch (Exception e) 
        { 
            Debug.Log("On client connect exception " + e);
        } 
    } 

    void OnApplicationQuit() 
    { 
        CloseSocket();
    } 
    
    void CloseSocket() 
    { 
        if (!socketReady) 
            return;
        reader.Close();
        client.Close();
        socketReady = false;
    }

    IEnumerator MoveCoroutine()
    {
        while (dirV != 0 || dirH != 0)
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


            vector.Set(dirH, dirV, transform.position.z);

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

    }
    void Update() {
        if (canMove)
        {
            if (dirV != 0 || dirH != 0)
            {
                canMove = false;
                StartCoroutine(MoveCoroutine());
            }
        }
    }
}