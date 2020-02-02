using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int spawnInterval = 150;
    public int spawnDistance = 100;
    public GameObject[] targets;
    public GameObject[] enemyTypes;

    private int tick = 130;
    private float spawnHeight = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        tick += 1;

        if (tick > spawnInterval)
        {
            tick -= spawnInterval;

            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        var targetIndex = Random.Range(0, targets.Length - 1);
        GameObject target = targets[targetIndex];

        Vector3 normal = new Vector3(target.transform.position.x, 0, target.transform.position.z);
        normal.Normalize();

        var enemyIndex = Random.Range(0, enemyTypes.Length - 1);
        GameObject go = GameObject.Instantiate(enemyTypes[enemyIndex], new Vector3(normal.x * spawnDistance, spawnHeight, normal.z * spawnDistance), Quaternion.identity);

        bool isTower = false;

        if (targetIndex < 4)
        {
            isTower = true;
        }
        go.GetComponent<EnemyBehavior>().SetTarget(target, isTower);
    }
}
