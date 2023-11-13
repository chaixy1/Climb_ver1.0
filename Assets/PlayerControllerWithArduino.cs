using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.IO;
using UnityEngine.Playables; // For Timeline
using UnityEngine.SceneManagement; // For scene management
public class PlayerControllerWithArduino : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float maxUpwardSpeed = 5.0f;
    public Animator playerAnimator;
    private Rigidbody rb;
    private bool isClimbing = false;
    private bool canMoveVertically = true;
    public float jumpForce = 300f;
    public Light directionalLight;

    public Vector3 gravityForce = new Vector3(0, -2f, 0);

    // Arduino Controller
    public SerialPort sp;
    public string portName = "COM5";
    public int baudRate = 9600;
    public float distanceThreshold = 0.1f;
    private float previousDistance = 0f;
    public kinecttest kinectInput; // 在 Unity 编辑器中设置此引用
    public float bigger = 1.6f;
    public GameObject uiPanel;
    public GameObject KinectPlayer;
    public PlayableDirector director; // Assign this in the inspector
    public string nextSceneName; // Set the name of the next scene here

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        OpenPort();
    }

    void Update()
    {
        //float horizontalInput = Input.GetAxis("Horizontal");

        // float horizontalInputRaw = kinectInput.GetHorizontalInput();
        // float horizontalInput = horizontalInputRaw * bigger ;


        // 获取 KinectPlayer 的当前位置
        Vector3 kinectPlayerPosition = KinectPlayer.transform.position;

        // 计算相对于原点的水平偏移
        float horizontalOffset = kinectPlayerPosition.x;

        // 应用这个偏移来控制角色的移动
        // 你可以根据需要调整偏移量的使用方式
        float horizontalInput = horizontalOffset * bigger;
        Debug.Log("horizontalInput: " + horizontalInput);
        Vector2 movement = new Vector2(-horizontalInput * moveSpeed, gravityForce.y);


        if (canMoveVertically)
        {
            rb.velocity = movement;
        }

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
                            JumpWithArduino();

                            if (rb.velocity.y > 0 && !isClimbing)
                            {
                                playerAnimator.SetBool("IsClimbing", true);
                                isClimbing = true;
                            }
                        }
                        else
                        {
                            // Reset climbing state or any other logic
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
                isClimbing = false;
            }
        }
        else
        {
            isClimbing = false;
            playerAnimator.SetBool("IsClimbing", false);

        }

    }

    void JumpWithArduino()
    {
        if (canMoveVertically)
        {
            rb.velocity = new Vector3(rb.velocity.x, 5.0f, rb.velocity.z);
            canMoveVertically = false;
            StartCoroutine(ResetYVelocity(2f));

            if (rb.velocity.y > 0 && !isClimbing)
            {
                playerAnimator.SetBool("IsClimbing", true);
                isClimbing = true;
            }
        }
    }

    private IEnumerator ResetYVelocity(float delay)
    {
        yield return new WaitForSeconds(delay);

        rb.velocity = new Vector3(rb.velocity.x, -1.0f, rb.velocity.z);
        canMoveVertically = true;
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

    void OnApplicationQuit()
    {
        ClosePort();
    }

    void ClosePort()
    {
        if (sp != null && sp.IsOpen)
        {
            sp.Close();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cube"))
        {
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("RedCube"))
        {
            rb.velocity = new Vector3(0, -5.0f, 0);
            StartCoroutine(ResetYVelocityAfterDelay(2f));
            canMoveVertically = false;
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("GreyCube"))
        {
            rb.velocity = new Vector3(0, 14.0f, 0);
            StartCoroutine(ResetYVelocityAfterDelay(1.5f));
            canMoveVertically = false;
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("LightBlock"))
        {
            if (directionalLight != null)
            {
                directionalLight.intensity += 0.2f;
            }
            Destroy(other.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("End"))
        {
            // 如果碰撞到 End，传送到指定位置
            TeleportToDestination();
            // 播放 Timeline
            if (director != null)
            {
                director.gameObject.SetActive(true);
                director.Play();
                StartCoroutine(WaitForTimeline());
            }
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            sp.Close();
            playerAnimator.SetBool("Dead", true); // 假设"Dead"是播放着陆动画的触发器名
            uiPanel.SetActive(true);
        }
    }
    IEnumerator WaitForTimeline()
    {
        // 等待 Timeline 播放完毕
        yield return new WaitForSeconds((float)director.duration);

        // 跳转到下一场景
        SceneManager.LoadScene(nextSceneName);
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

    private IEnumerator ResetYVelocityAfterDelay(float delay)
    {
        float elapsedTime = 0f;
        Vector3 initialVelocity = rb.velocity;
        Vector3 targetVelocity = new Vector3(rb.velocity.x, -1.0f, rb.velocity.z);

        while (elapsedTime < delay)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            Vector2 horizontalMovement = new Vector2(-horizontalInput * moveSpeed, 0);
            rb.velocity = horizontalMovement;
            rb.velocity = Vector3.Lerp(initialVelocity, targetVelocity, elapsedTime / delay);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Debug.Log("协程函数被调用1");
        canMoveVertically = true;
    }
}
