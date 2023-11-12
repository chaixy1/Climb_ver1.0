using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_level02 : MonoBehaviour
{
    public GameObject redCubePrefab;
    public GameObject greyCubePrefab;
    public GameObject player;

    public float spawnInterval = 10f;
    public float cubeSpeed = 10f;
    public float playerSpeed = 1f;
    public float jumpDistance = 1.0f;

    private float nextSpawnTime = 5.0f;

    private Vector3 downForce = new Vector3(0, -2.0f, 0);
    private Vector3 upForce = new Vector3(0, 20.0f, 0);

    private void Update()
    {
        // 生成小方块
        if (Time.time >= nextSpawnTime)
        {
            SpawnCube();
            nextSpawnTime = Time.time + spawnInterval;
        }

        // 玩家向下运动
       //player.transform.Translate(Vector3.down * playerSpeed * Time.deltaTime);

        // 检测玩家与小方块的碰撞
        Collider[] colliders = Physics.OverlapBox(player.transform.position, player.transform.localScale / 2);
       

    }

    private void OnTriggerEnter(Collider other){
       
        if (other.CompareTag("RedCube"))
            {
                Debug.Log("碰到红色小方块");
                // 碰到红色小方块，玩家向下快速移动一段距离
                //player.transform.Translate(Vector3.down * jumpDistance);
                player.GetComponent<Rigidbody>().AddForce(downForce);
                //销毁小方块
                Destroy(GetComponent<Collider>().gameObject);
            }

    }

    private void SpawnCube()
    {
        // 在限定的区间内生成小方块
        //float spawnX = Random.Range(-46f, -53f);
        // 在指定的xyz区域内生成小方块
        float spawnX = Random.Range(-46f, -53f);
        float spawnY = Random.Range(0f, 50f);
        float spawnZ = Random.Range(8.7f, 8.8f);
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, spawnZ);

        // 随机选择生成红色或灰色小方块
        GameObject cubePrefab = Random.value < 0.2f ? redCubePrefab : greyCubePrefab;

        // 实例化小方块并设置速度
        GameObject cube = Instantiate(cubePrefab, spawnPosition, cubePrefab.transform.rotation);
        cube.GetComponent<Rigidbody>().velocity = Vector3.up * cubeSpeed;
    }
}
