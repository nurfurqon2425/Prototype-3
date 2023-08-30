using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] obstaclePrefab;
    private Vector3 spawnPos = new Vector3(30, 0, 0);
    private float startDelay = 5.0f;
    public float repeatRate = 2.0f;
    private PlayerController playerControllerScript;

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        InvokeRepeating("SpawnObstacle", startDelay, repeatRate);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnObstacle()
    {
        int prefabIndex = Random.Range(0, obstaclePrefab.Length);

        if (playerControllerScript.gameOver == false)
        {
            Instantiate(obstaclePrefab[prefabIndex], spawnPos, obstaclePrefab[prefabIndex].transform.rotation);         
        }
    }
}
