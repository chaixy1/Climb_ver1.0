using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // public float moveSpeed = 10.0f; // 垂直移动速度
    // public float maxUpwardSpeed = 5.0f; // 最大向上速度
    // public Animator playerAnimator; // 玩家控制器的动画组件

    // public Vector3 InitPlace;

    // private Rigidbody rb;
    // private bool isClimbing = false;
    // private bool canMoveVertically = true; // 新增标志

    // public float jumpForce = 0.5f; // 新增的跳跃力度

    // public Light directionalLight; // 通过 Unity 编辑器将 Directional Light 分配给这个变量

    // // 控制下降
    // private Vector3 gravityForce = new Vector3(0, -1f, 0);  // 新增的重力向量

    // private Vector3 downForce = new Vector3(0, -5f, 0);
    // private Vector3 upForce = new Vector3(0, 1900f, 0);

    // public float doubleTapTimeThreshold = 0.01f;  // 两次按键的时间间隔阈值
    // private float lastTapTime;  // 上一次按键的时间

    // public GameObject lightBlock;  // 小方块对象带有灯光组件
    // private bool lightBlockActivated = false;

    // void Start()
    // {
    //     rb = GetComponent<Rigidbody>();
    // }

    // void Update()
    // {
    //     float horizontalInput = Input.GetAxis("Horizontal");
    //     Vector2 horizontalMovement = new Vector2(-horizontalInput * moveSpeed, gravityForce.y);

    //     // 如果当前可以垂直移动，则更新水平方向的速度
    //     if (canMoveVertically)
    //     {
    //         rb.velocity = horizontalMovement;
    //     }

    //     // 检查是否按下"W"键
    //     if (Input.GetKeyDown("w"))
    //     {
    //         canMoveVertically = false;
    //         rb.velocity = new Vector3(rb.velocity.x, 5.0f, rb.velocity.z);

    //         // 检查是否在墙壁上并且没有开始攀爬
    //         if (rb.velocity.y > 0 && !isClimbing)
    //         {
    //             // 在这里开始攀爬动画
    //             playerAnimator.SetBool("IsClimbing", true);
    //             isClimbing = true;
    //         }
    //     }

    //     // 检查是否松开"W"键
    //     if (Input.GetKeyUp("w"))
    //     {
    //         canMoveVertically = true;
    //         isClimbing = false;
    //         playerAnimator.SetBool("IsClimbing", false); // 停止攀爬动画
    //         rb.velocity = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z); // 将垂直速度重置为0
    //     }
    // }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.gameObject.tag == "Cube")
    //     {
    //         Destroy(other.gameObject);
    //     }
    //     else if (other.CompareTag("RedCube"))
    //     {
    //         rb.velocity = new Vector3(0, -5.0f, 0);
    //         StartCoroutine(ResetYVelocityAfterDelay(2f)); // 启动协程
    //         canMoveVertically = false; // 设置标志为false
    //         Destroy(other.gameObject);
    //     }
    //     else if (other.CompareTag("GreyCube"))
    //     {
    //         rb.velocity = new Vector3(0, 10.0f, 0);
    //         StartCoroutine(ResetYVelocityAfterDelay(4f)); // 启动协程
    //         canMoveVertically = false; // 设置标志为false
    //         Destroy(other.gameObject);
    //     }
    //     else if(other.CompareTag("LightBlock"))
    //     {
    //         // // 获取小方块上的灯光组件
    //         // Light blockLight = lightBlock.GetComponent<Light>();

    //         // // 启用小方块上的灯光组件
    //         // if (blockLight != null)
    //         // {
    //         //     blockLight.enabled = true;
    //         //     lightBlockActivated = true;
    //         // }
    //         if (directionalLight != null)
    //         {
    //             directionalLight.intensity += 1.0f; // 适当调整增强的亮度值
    //             directionalLight.range += 5.0f; // 适当调整增强的范围值
    //         }
    //         Destroy(other.gameObject);
    //     }
    // }

    // void OnCollisionEnter(Collision collision)
    // {
    //     // 当与地面碰撞时开始跳跃动画
    //     if (collision.gameObject.CompareTag("Ground"))
    //     {
    //         // 在这里开始着陆动画
    //         playerAnimator.SetBool("Dead", true); // 设置"Dead"为触发着陆动画的参数名
    //     }
    //     else if (collision.gameObject.CompareTag("OnTop"))
    //     {
    //         // 如果碰撞到 Cube，将玩家位置重置
    //         this.transform.position = InitPlace;
    //         playerAnimator.SetBool("Idle", true); // 播放idle动画
    //     }
    // }

    // // 协程函数
    // private IEnumerator ResetYVelocityAfterDelay(float delay)
    // {
    //     float elapsedTime = 0f;
    //     Vector3 initialVelocity = rb.velocity;
    //     Vector3 targetVelocity = new Vector3(rb.velocity.x, -1.0f, rb.velocity.z);

    //     while (elapsedTime < delay)
    //     {
    //         float verticalInput = Input.GetAxis("Vertical");
    //         Vector2 verticalMovement = new Vector2(0, verticalInput * moveSpeed); // 获取垂直方向的输入
    //         rb.velocity = verticalMovement;

    //         rb.velocity = Vector3.Lerp(initialVelocity, targetVelocity, elapsedTime / delay);
    //         elapsedTime += Time.deltaTime;
    //         yield return null;
    //     }

    //     Debug.Log("协程函数被调用1");
    //     rb.velocity = targetVelocity;
    //     canMoveVertically = true; // 设置标志为true
    // }

    // private IEnumerator ResetYVelocity(float delay)
    // {
    //     yield return new WaitForSeconds(delay);
    //     Debug.Log("协程函数被调用");

    //     // 在延迟之后将玩家的y轴速度重置为-1
    //     rb.velocity = new Vector3(rb.velocity.x, -1.0f, rb.velocity.z);
    //     canMoveVertically = true; // 设置标志为true
    // }

    public float moveSpeed = 10.0f; // 垂直移动速度
    public float maxUpwardSpeed = 50000.0f; // 最大向上速度
    public Animator playerAnimator; // 玩家控制器的动画组件

    public Vector3 InitPlace;

    private Vector3 gravityForce = new Vector3(0, -5f, 0);  // 新增的重力向量

    private Rigidbody rb;
    private bool isClimbing = false;
    private bool canMoveVertically = true; // 新增标志

    private float jumpCounter = 0; // 新增计数器

    public float doubleTapTimeThreshold = 0.01f;  // 两次按键的时间间隔阈值
    private float lastTapTime;  // 上一次按键的时间

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 horizontalMovement = new Vector2(-horizontalInput * moveSpeed, gravityForce.y); // 修改水平方向速度

        // 如果当前可以垂直移动，则更新水平方向的速度
        if (canMoveVertically)
        {
            rb.velocity = horizontalMovement;
        }

        // 检查是否按下"W"键
        if (Input.GetKeyDown("w"))
        {
            jumpCounter++; // 每次按下"W"键增加计数器的值
        }

        // 检查是否松开"W"键
        // if (Input.GetKeyUp("w"))
        // {
        //     canMoveVertically = true;
        //     isClimbing = false;
        //     playerAnimator.SetBool("IsClimbing", false); // 停止攀爬动画
        //     rb.velocity = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z); // 将垂直速度重置为0
        // }
    }

    void OnCollisionEnter(Collision collision)
    {
        // 当与地面碰撞时开始跳跃动画
        if (collision.gameObject.CompareTag("Ground"))
        {
            canMoveVertically = false;
            Debug.Log("jumpCounter: " + jumpCounter);
            // 在这里开始着陆动画
            playerAnimator.SetBool("Dead", true); // 设置"Dead"为触发着陆动画的参数名

            // 根据计数器的值设置向上的速度和持续时间
            rb.velocity = new Vector3(rb.velocity.x, jumpCounter/4.9f, rb.velocity.z);
            StartCoroutine(ResetYVelocityAfterDelay(5));
            jumpCounter = 0; // 重置计数器
        }
        else if (collision.gameObject.CompareTag("OnTop"))
        {
            // 如果碰撞到 Cube，将玩家位置重置
            this.transform.position = InitPlace;
            playerAnimator.SetBool("Idle", true); // 播放idle动画
        }
    }

    // 协程函数
    private IEnumerator ResetYVelocityAfterDelay(int delay)
    {
        yield return new WaitForSeconds(delay);

        // 在延迟之后将玩家的y轴速度重置为-1
        rb.velocity = new Vector3(rb.velocity.x, -1.0f, rb.velocity.z);
        canMoveVertically = true; // 设置标志为true
    }
}
