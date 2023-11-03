using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.IO;

public class ArduinoController : MonoBehaviour
{
    public float moveSpeed = 10.0f; // 横向移动速度
    public float maxUpwardSpeed = 10.0f; // 最大向上速度
    public Animator playerAnimator; // 引用玩家的动画控制器

    private Rigidbody rb;

    private bool isJumping = false;
    private bool isClimbing = false;

    private SerialPort sp;
    public string portName = "COM5"; // 串行端口名称
    public int baudRate = 9600; // 波特率
    public float distanceThreshold = 0.1f; // Arduino距离阈值
    private float previousDistance = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        OpenPort(); // 游戏开始时打开串行端口
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
                            rb.velocity = new Vector2(rb.velocity.x, maxUpwardSpeed);
                            isJumping = true;

                            if (rb.velocity.y > 0 && !isClimbing)
                            {
                                playerAnimator.SetBool("IsClimbing", true);
                                
                                isClimbing = true;
                            }
                        }
                        else
                        {
                            if(isClimbing)
                            {
                                playerAnimator.SetBool("IsClimbing",false);
                                
                                isClimbing=false;
                            }
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
                // 没有可用数据时的逻辑，你可以选择留空或执行其他操作
                playerAnimator.SetBool("IsClimbing", false);
                isJumping = false;
            }
        }
        else
        {
            isJumping = false;
            isClimbing = false;
            playerAnimator.SetBool("IsClimbing", false);
            
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            playerAnimator.SetBool("Dead", true); // 假设"Dead"是播放着陆动画的触发器名
        }
        else if (collision.gameObject.CompareTag("OnTop"))
        {
            // 如果碰撞到 Cube，向后移动 5.0f
            rb.velocity = new Vector2(-5.0f, rb.velocity.y);
        }
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
