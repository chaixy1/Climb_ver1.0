using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.IO;

public class PlayerControllerWithArduino : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float maxUpwardSpeed = 5.0f;
    public Animator playerAnimator;
    private Rigidbody rb;
    private bool isClimbing = false;
    private bool canMoveVertically = true;
    public float jumpForce = 500f;
    public Light directionalLight;

    private Vector3 gravityForce = new Vector3(0, -1f, 0);

    // Arduino Controller
    public SerialPort sp;
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
        float horizontalInput = Input.GetAxis("Horizontal");
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
        else if(other.CompareTag("LightBlock"))
        {
            if (directionalLight != null)
            {
                directionalLight.intensity += 0.2f;
            }
            Destroy(other.gameObject);
        }
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
