using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10.0f; // 妯悜绉诲姩閫熷害
    public float maxUpwardSpeed = 5.0f; // 鏈€澶у悜涓婇€熷害
    public Animator playerAnimator; // 寮曠敤鐜╁鐨勫姩鐢绘帶鍒跺櫒

    public Vector3 InitPlace;

    private Rigidbody rb;
    
    private bool isJumping = false;
    private bool isClimbing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 鑾峰彇鐜╁杈撳叆
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // 鎺у埗鐜╁鍦ㄥ涓婄殑妯悜杩愬姩
        Vector2 movement = new Vector2(-horizontalInput * moveSpeed, rb.velocity.y); // 淇濇寔鍨傜洿閫熷害涓嶅彉
        rb.velocity = movement;

        //// 妫€鏌ユ槸鍚︽寜涓媁锛屽苟涓旇鐜╁鍚戜笂绉诲姩
        //if (Input.GetKey("w") && !isJumping)
        //{
        //    rb.velocity = new Vector2(rb.velocity.x, maxUpwardSpeed);
        //    isJumping = true;
        //}

        //if (!Input.GetKey("w"))
        //{
        //    isJumping = false;
        //}

        if (Input.GetKeyDown("w"))
        {
            if (!isJumping)
            {
                rb.velocity = new Vector2(rb.velocity.x, maxUpwardSpeed);
                isJumping = true;
            }

            // 妫€鏌ユ槸鍚﹀湪绌轰腑骞朵笖鏈紑濮嬫攢鐖?
            if (rb.velocity.y > 0 && !isClimbing)
            {
                // 鍦ㄨ繖閲岃Е鍙戞挱鏀炬攢鐖姩鐢?
                playerAnimator.SetBool("IsClimbing", true);
                isClimbing = true;
            }
        }
        else
        {
            isJumping = false;
            isClimbing = false;
            playerAnimator.SetBool("IsClimbing", false); // 鍋滄鏀€鐖姩鐢?
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        // 褰撲笌鍦伴潰纰版挒鏃堕噸缃烦璺冪姸鎬侊紝骞舵挱鏀惧姩鐢?
        if (collision.gameObject.CompareTag("Ground"))
        {
            // 鍦ㄨ繖閲岃Е鍙戞挱鏀惧湴闈㈢鎾炴椂鐨勫姩鐢?
            playerAnimator.SetBool("Dead",true); // 鍋囪"Land"鏄挱鏀剧潃闄嗗姩鐢荤殑瑙﹀彂鍣ㄥ悕绉?
        }
        else if (collision.gameObject.CompareTag("OnTop"))
        {
            // 如果碰撞到 Cube，向后移动 5.0f
           // rb.velocity = new Vector2(-10.0f, rb.velocity.y);
           this.transform.position = InitPlace;
           playerAnimator.SetBool("Idle", true); // 播放idle动画
        }
    }
}
