using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayController_level01 : MonoBehaviour
{
    // public float moveSpeed = 10.0f;
    // public float jumpForce = 500f;
    // public Animator playerAnimator;
    // private Rigidbody rb;
    // private bool canMoveVertically = true;
    // private bool isClimbing = false;
    // private Vector3 gravityForce = new Vector3(0, -9.8f, 0);

    // void Start()
    // {
    //     rb = GetComponent<Rigidbody>();
    // }

    // void Update()
    // {
    //     float horizontalInput = Input.GetAxis("Horizontal");
    //     Vector2 movement = new Vector2(-horizontalInput * moveSpeed, gravityForce.y);

    //     if (canMoveVertically)
    //     {
    //         rb.velocity = movement;
    //     }

    //     if (Input.GetKeyDown("w") && canMoveVertically)
    //     {
    //         playerAnimator.SetBool("IsClimbing", true);
    //         StartCoroutine(JumpCoroutine());
    //     }

    //     gravityForce.y = Mathf.Lerp(gravityForce.y, -9.8f, 0.1f * Time.deltaTime);
    // }

    // private IEnumerator JumpCoroutine()
    // {
    //     canMoveVertically = false;

    //     rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

    //     yield return new WaitForSeconds(1f); // 根据测试调整这个延迟时间

    //     if (rb.velocity.y > 0 && !isClimbing)
    //     {
    //         playerAnimator.SetBool("IsClimbing", true);
    //         isClimbing = true;
    //     }

    //     yield return new WaitForSeconds(0.1f); // 根据测试调整这个延迟时间

    //     isClimbing = false;
    //     playerAnimator.SetBool("IsClimbing", false);

    //     yield return new WaitForSeconds(0.1f); // 根据测试调整这个延迟时间

    //     canMoveVertically = true;
    // }

    public float moveSpeed = 10.0f;
    public float jumpForce = 500f;
    public Animator playerAnimator;
    private Rigidbody rb;
    private bool canMoveVertically = true;
    private bool isClimbing = false;
    private Vector3 gravityForce = new Vector3(0, -9.8f, 0);

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
            playerAnimator.SetBool("IsClimbing", true);
            StartCoroutine(JumpCoroutine());
        }
    }

    private IEnumerator JumpCoroutine()
    {
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

        // 等待一段时间，逐渐将速度调整为负的重力值
        yield return new WaitForSeconds(0.5f); // 调整这个延迟时间
        StartCoroutine(GradualGravity());
    }

    private IEnumerator GradualGravity()
    {
        float elapsedTime = 0f;
        Vector3 initialVelocity = rb.velocity;
        Vector3 targetVelocity = new Vector3(rb.velocity.x, -9.8f, rb.velocity.z);

        while (elapsedTime < 1f) // 调整这个时间来控制逐渐变化的速度
        {
            rb.velocity = Vector3.Lerp(initialVelocity, targetVelocity, elapsedTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
