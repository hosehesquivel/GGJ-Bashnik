using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int spawnInterval = 300;
    public GameObject[] targets;
    public GameObject[] enemyTypes;

    private int tick = 0;

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
        var enemyIndex = Random.Range(0, enemyTypes.Length - 1);
        GameObject go = GameObject.Instantiate(enemyTypes[enemyIndex], new Vector3(Random.Range(0,5),0, Random.Range(0, 5)), Quaternion.identity);

        var targetIndex = Random.Range(0, targets.Length - 1);

        go.GetComponent<EnemyBehavior>().SetTarget(targets[targetIndex]);
    }
}
