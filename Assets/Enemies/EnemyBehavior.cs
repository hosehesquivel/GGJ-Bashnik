using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public GameObject target;
    public float speed = 45f;
    public float damage = 5.0f;
    public float distanceFromWall = 4.5f;

    private string state = "moving";
    private GameObject lootTarget;

    private bool hasGold = false;
    private Vector3 origin;

    public void SetTarget(GameObject target, bool isTower)
    {
        this.target = target;
        transform.LookAt(target.transform);

        if (isTower)
        {
            gameObject.layer = 9;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] arr = GameObject.FindGameObjectsWithTag("MainTower");
        lootTarget = arr[0];

        origin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            HandleMove();

        }
    }

    public void HandleMove()
    {
        if (this.target)
        {
            DestructibleBehavior db = target.GetComponent<DestructibleBehavior>();
            float step = speed * Time.deltaTime;

            
            if (db)
            {
                if (db.hp > 0)
                {

                    float distance = Vector3.Distance(transform.position, target.transform.position);

                    if (distance > distanceFromWall)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
                    }
                    else
                    {
                        HandleAttack();
                    }
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, lootTarget.transform.position, step);

                    if (hasGold)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, origin, step * 2);
                    }
                }
            }
            
        }
    }

    private void HandleAttack()
    {
        DestructibleBehavior db = target.GetComponent<DestructibleBehavior>();

        if (db)
        {
            db.HandleAttack(damage);

            state = "idle";
        }
    } 

    private void HandleTargetDestroyed()
    {
        state = "moving";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Destructible")
        {
            state = "attacking";
        } else if (other.tag == "MainTower")
        {
            hasGold = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Destructible" && (state == "attacking" || state == "idle"))
        {
            Debug.Log("Exit");
            
        }
    }
}
