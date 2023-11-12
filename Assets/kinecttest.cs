using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kinecttest : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 lastPosition;
    private float checkInterval = 1.0f;
    private float timer;
    void Start()
    {
        //初始时记录当前位置
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        // 当超过一秒时，检查移动方向
        if (timer >= checkInterval)
        {
            // 计算移动方向
            Vector3 movementDirection = transform.position - lastPosition;
            if (movementDirection.x < -0.2) // 如果X轴上的移动为负值，表示向左移动
            {
                Debug.Log(1);
            }
            else if (movementDirection.x > 0.2) // 如果X轴上的移动为正值，表示向右移动
            {
                Debug.Log(0);
            }

            // 更新位置并重置计时器
            lastPosition = transform.position;
            timer = 0f;
        }
    }
}
