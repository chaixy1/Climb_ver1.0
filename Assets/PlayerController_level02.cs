using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_level02 : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float maxUpwardSpeed = 5.0f;
    public Animator playerAnimator;
    public Vector3 InitPlace;
    private Rigidbody rb;
    private bool isClimbing = false;
    public bool canMoveVertically = true;
    public float jumpForce = 500f;
    public Light directionalLight;

    private Vector3 gravityForce = new Vector3(0, -1f, 0);
    private Vector3 downForce = new Vector3(0, -5f, 0);
    private bool lightBlockActivated = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(-horizontalInput * moveSpeed, gravityForce.y);

        if (canMoveVertically)
        {
            rb.velocity = movement;
        }

        if (Input.GetKeyDown("w"))
        {
            rb.velocity = new Vector3(movement.x, 5.0f, rb.velocity.z);
            canMoveVertically = false;
            StartCoroutine(ResetYVelocity(2f));

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

        if (Input.GetKeyDown("w") && canMoveVertically)
        {
            playerAnimator.SetBool("IsClimbing", true);
            StartCoroutine(JumpCoroutine());
        }

        if (rb.velocity.y < 0)
        {
            playerAnimator.SetBool("IsClimbing", false);
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

    void OnCollisionEnter(Collision collision)
    {
        // 当与地面碰撞时开始跳跃动画
        if (collision.gameObject.CompareTag("Ground"))
        {
            //canMoveVertically = false;
            // 在这里开始着陆动画
            playerAnimator.SetBool("Dead", true); // 设置"Dead"为触发着陆动画的参数名
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

    public void JumpWithArduino()
    {
        StartCoroutine(JumpCoroutine());
    }

    private IEnumerator ResetYVelocity(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("协程函数被调用");

        rb.velocity = new Vector3(rb.velocity.x, -1.0f, rb.velocity.z);
        canMoveVertically = true;
    }

    public IEnumerator JumpCoroutine()
    {
        Debug.Log("JumpCoroutine被调用");
        canMoveVertically = false;

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        yield return new WaitForSeconds(1f); // 根据测试调整这个延迟时间

        if (rb.velocity.y > 0 && !isClimbing)
        {
            playerAnimator.SetBool("IsClimbing", true);
            isClimbing = true;
        }

        yield return new WaitForSeconds(0.1f); // 根据测试调整这个延迟时间

        isClimbing = false;
        playerAnimator.SetBool("IsClimbing", false);

        yield return new WaitForSeconds(0.1f); // 根据测试调整这个延迟时间

        canMoveVertically = true;
    }

}
