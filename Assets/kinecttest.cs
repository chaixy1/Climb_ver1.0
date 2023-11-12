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
        //��ʼʱ��¼��ǰλ��
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        // ������һ��ʱ������ƶ�����
        if (timer >= checkInterval)
        {
            // �����ƶ�����
            Vector3 movementDirection = transform.position - lastPosition;
            if (movementDirection.x < -0.2) // ���X���ϵ��ƶ�Ϊ��ֵ����ʾ�����ƶ�
            {
                Debug.Log(1);
            }
            else if (movementDirection.x > 0.2) // ���X���ϵ��ƶ�Ϊ��ֵ����ʾ�����ƶ�
            {
                Debug.Log(0);
            }

            // ����λ�ò����ü�ʱ��
            lastPosition = transform.position;
            timer = 0f;
        }
    }
}
