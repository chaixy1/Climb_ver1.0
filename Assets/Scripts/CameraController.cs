using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // 玩家或要跟随的目标
    public float smoothSpeed = 0.125f; // 相机移动的平滑度
    public float distance = 5.0f; // 相机与跟踪对象之间的距离
    public float heightOffset = 2.0f; // 相机高度偏移

    void LateUpdate()
    {
        // 计算相机的目标位置，使其位于玩家中心点的上方并保持一定距离
        Vector3 targetCenter = target.position + Vector3.up * heightOffset;
        Vector3 targetPosition = targetCenter - target.forward * distance;

        // 使用平滑移动到目标位置
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // 使相机的正方向（forward）一直指向玩家的中心点
        transform.LookAt(targetCenter);
    }
}
