using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.IO;
using UnityEngine.UI;

public class ArduinoController : MonoBehaviour
{
    public float moveSpeed = 10.0f; // 横向移动速度
    public float maxUpwardSpeed = 10.0f; // 最大向上速度
    public Animator playerAnimator; // 引用玩家的动画控制器

    private Rigidbody rb;

    private bool isJumping = false;
    private bool isClimbing = false;

    private SerialPort sp;
    public string portName = "COM3"; // 串行端口名称
    public int baudRate = 9600; // 波特率
    public float distanceThreshold = 0.1f; // Arduino距离阈值
    private float previousDistance = 0f;

    public float jumpForce = 5f;

    public PlayerController_level02 playerController;

    public GameObject uiPanel;

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
                            playerAnimator.SetBool("IsClimbing", true);

                            if (rb.velocity.y > 0 && !isClimbing)
                            {
                                playerAnimator.SetBool("IsClimbing", true);

                                isClimbing = true;
                            }
                        }
                        else
                        {
                            // if(isClimbing)
                            // {
                            //     playerAnimator.SetBool("IsClimbing",false);

                            //     isClimbing=false;
                            // }
                            //playerAnimator.SetBool("IsClimbing",false);
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
            sp.Close();
            playerAnimator.SetBool("Dead", true); // 假设"Dead"是播放着陆动画的触发器名
            uiPanel.SetActive(true);
        }
        else if (collision.gameObject.CompareTag("OnTop"))
        {
            // 如果碰撞到 Cube，向后移动 5.0f
            rb.velocity = new Vector2(-5.0f, rb.velocity.y);
        }
        else if (collision.gameObject.CompareTag("End"))
        {
            // 如果碰撞到 End，传送到指定位置
            TeleportToDestination();

        }
    }

    public Transform destinationTransform; // 通过Unity编辑器指定的目标位置

    void TeleportToDestination()
    {
        // 在这里添加传送到指定位置的逻辑
        // 例如，将当前物体的位置设置为目标位置
        if (destinationTransform != null)
        {
            transform.position = destinationTransform.position;
        }
        playerAnimator.SetBool("Idle", true);
        rb.velocity = new Vector2(rb.velocity.x, 0f);
    }

    Transform GetDestinationTransform()
    {
        // 在这里添加获取目标位置的逻辑
        // 例如，通过目标物体的标签或名称来查找目标物体
        GameObject destinationObject = GameObject.FindWithTag("DestinationObject");
        if (destinationObject != null)
        {
            return destinationObject.transform;
        }

        // 如果找不到目标物体，可以返回null或执行其他逻辑
        return null;
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
