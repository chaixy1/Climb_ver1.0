using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //在玩家的周围范围内随机生成cube预制体，玩家和cube碰撞之后，cube消失
    public GameObject cubePrefab;
    public GameObject player;
    public float cubeSpawnRadius = 10.0f;
    public float cubeSpawnRate = 1.0f;
    public float cubeSpawnTimer = 0.0f;

    void Start()
    {

        
    }

    void Update()
    {
        cubeSpawnTimer += Time.deltaTime;
        if (cubeSpawnTimer >= cubeSpawnRate)
        {
            cubeSpawnTimer = 0.0f;
            SpawnCube();
        }
    }

    void SpawnCube()
    {
        // 随机生成cube的位置
        Vector3 spawnPosition = new Vector3(Random.Range(-cubeSpawnRadius, cubeSpawnRadius), 0.0f, Random.Range(-cubeSpawnRadius, cubeSpawnRadius));
        // 生成cube
        GameObject cube = Instantiate(cubePrefab, spawnPosition, Quaternion.identity);
        // 设置cube的父物体
        cube.transform.parent = player.transform;
    }

    public void DestroyCube(GameObject cube)
    {
        Destroy(cube);
    }

    public void DestroyAllCubes()
    {
        foreach (Transform child in player.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
