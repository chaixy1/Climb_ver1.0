using UnityEngine;

public class TextShake : MonoBehaviour
{
    public float shakeAmount = 0.5f;
    Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        float shakeX = Random.value * shakeAmount * 2 - shakeAmount;
        float shakeY = Random.value * shakeAmount * 2 - shakeAmount;
        transform.localPosition = new Vector3(originalPosition.x + shakeX, originalPosition.y + shakeY, originalPosition.z);
    }
}
