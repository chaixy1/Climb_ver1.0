using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.IO;

public class PlayerController_level03 : MonoBehaviour
{
    // public float moveSpeed = 10.0f; // 垂直移动速度
    // public float maxUpwardSpeed = 50000.0f; // 最大向上速度
    // public Animator playerAnimator; // 玩家控制器的动画组件

    // public Vector3 InitPlace;

    // private Vector3 gravityForce = new Vector3(0, -5f, 0);  // 新增的重力向量

    // private Rigidbody rb;
    // private bool isClimbing = false;
    // private bool canMoveVertically = true; // 新增标志

    // private float jumpCounter = 0; // 新增计数器

    // void Start()
    // {
    //     rb = GetComponent<Rigidbody>();
    // }

    // void Update()
    // {
    //     float horizontalInput = Input.GetAxis("Horizontal");
    //     Vector2 horizontalMovement = new Vector2(-horizontalInput * moveSpeed, gravityForce.y); // 修改水平方向速度

    //     // 如果当前可以垂直移动，则更新水平方向的速度
    //     if (canMoveVertically)
    //     {
    //         rb.velocity = horizontalMovement;
    //     }

    //     // 检查是否按下"W"键
    //     if (Input.GetKeyDown("w"))
    //     {
    //         jumpCounter++; // 每次按下"W"键增加计数器的值
    //     }

    // }

    // void OnCollisionEnter(Collision collision)
    // {
    //     // 当与地面碰撞时开始跳跃动画
    //     if (collision.gameObject.CompareTag("Ground"))
    //     {
    //         canMoveVertically = false;
    //         Debug.Log("jumpCounter: " + jumpCounter);
    //         // 在这里开始着陆动画
    //         playerAnimator.SetBool("Dead", true); // 设置"Dead"为触发着陆动画的参数名

    //         // 根据计数器的值设置向上的速度和持续时间
    //         rb.velocity = new Vector3(rb.velocity.x, jumpCounter/4.9f, rb.velocity.z);
    //         StartCoroutine(ResetYVelocityAfterDelay(5));
    //         jumpCounter = 0; // 重置计数器
    //     }
    //     else if (collision.gameObject.CompareTag("OnTop"))
    //     {
    //         // 如果碰撞到 Cube，将玩家位置重置
    //         this.transform.position = InitPlace;
    //         playerAnimator.SetBool("Idle", true); // 播放idle动画
    //     }
    // }

    // // 协程函数
    // private IEnumerator ResetYVelocityAfterDelay(int delay)
    // {
    //     yield return new WaitForSeconds(delay);

    //     // 在延迟之后将玩家的y轴速度重置为-1
    //     rb.velocity = new Vector3(rb.velocity.x, -1.0f, rb.velocity.z);
    //     canMoveVertically = true; // 设置标志为true
    // }

    public float moveSpeed = 10.0f;
    public float maxUpwardSpeed = 50000.0f;
    public Animator playerAnimator;
    public Vector3 InitPlace;
    private Vector3 gravityForce = new Vector3(0, -5f, 0);
    private Rigidbody rb;
    private bool canMoveVertically = true;
    private float jumpCounter = 0;
    private bool isClimbing = false; // 新增 isClimbing 变量

    // 新增属性，用于设置是否由 Arduino 控制计数器
    public bool ArduinoControlledCounter = false;

    private SerialPort sp;
    public string portName = "COM5";
    public int baudRate = 9600;
    public float distanceThreshold = 0.1f;
    private float previousDistance = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        OpenPort();
    }

    void Update()
    {
        if (sp != null && sp.IsOpen)
        {
            if (sp.BytesToRead > 0)
            {
                try
                {
                    string strRec = sp.ReadLine();
                    Debug.Log("从串行端口接收: " + strRec);

                    if (float.TryParse(strRec, out float arduinoDistance))
                    {
                        if (arduinoDistance < (previousDistance - distanceThreshold))
                        {
                            IncrementJumpCounter();
                            rb.velocity = new Vector2(rb.velocity.x, maxUpwardSpeed);
                            playerAnimator.SetBool("IsClimbing", true);

                            if (rb.velocity.y > 0 && !isClimbing)
                            {
                                playerAnimator.SetBool("IsClimbing", true);
                                isClimbing = true;                             
                            }
                        }
                        else
                        {
                            isClimbing = false;
                            playerAnimator.SetBool("IsClimbing", false);
                        }

                        previousDistance = arduinoDistance;
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.Log(ex.Message);
                }
            }
            else
            {
                playerAnimator.SetBool("IsClimbing", false);
            }
        }
        else
        {
            isClimbing = false;
            playerAnimator.SetBool("IsClimbing", false);
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 horizontalMovement = new Vector2(-horizontalInput * moveSpeed, gravityForce.y);

        if (canMoveVertically)
        {
            rb.velocity = horizontalMovement;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canMoveVertically = false;
            playerAnimator.SetBool("Dead", true);

            Debug.Log("jumpCounter: " + jumpCounter);

            // 根据计数器的值设置向上的速度和持续时间
            rb.velocity = new Vector3(rb.velocity.x, jumpCounter/3f, rb.velocity.z);
            StartCoroutine(ResetYVelocityAfterDelay(5));
            jumpCounter = 0; // 重置计数器
        }
        else if (collision.gameObject.CompareTag("OnTop"))
        {
            this.transform.position = InitPlace;
            playerAnimator.SetBool("Idle", true);
        }
    }

    private void IncrementJumpCounter()
    {
        jumpCounter++;
    }

    private IEnumerator ResetYVelocityAfterDelay(int delay)
    {
        yield return new WaitForSeconds(delay);
        rb.velocity = new Vector3(rb.velocity.x, -1.0f, rb.velocity.z);
        canMoveVertically = true;
    }

    void OnApplicationQuit()
    {
        ClosePort();
    }

    void OpenPort()
    {
        sp = new SerialPort(portName, baudRate);
        sp.ReadTimeout = 1000;

        try
        {
            sp.Open();
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    void ClosePort()
    {
        if (sp != null && sp.IsOpen)
        {
            sp.Close();
        }
    }
}
