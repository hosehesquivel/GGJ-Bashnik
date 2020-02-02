using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public GameObject target;
    public float speed = 45f;
    public float damage = 5.0f;
    public float distanceFromWall = 7.0f;
    public float bounceForce = 25.0f;

    public GameObject sword;
    public GameObject sword2;

    private string state = "moving";
    private GameObject lootTarget;

    private bool hasGold = false;
    private bool isDisappointed = false;
    private Vector3 origin;
    private float tick = 0;
    private float animTick = 0;
    private bool swingingDown = false;

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
            if (state == "attacking")
            {
                animTick += Time.deltaTime;

                if (animTick > .5f)
                {
                    animTick -= .5f;
                    if (swingingDown)
                    {
                        swingingDown = false;
                        sword2.gameObject.SetActive(false);
                        sword.gameObject.SetActive(true);
                    } else
                    {
                        swingingDown = true;
                        sword2.gameObject.SetActive(true);
                        sword.gameObject.SetActive(false);
                    }
                    
                }
                
                //sword.gameObject.transform.eulerAngles = new Vector3(-140, 90, 0);
            }
        }
    }

    public void HandleMove()
    {
        float step = speed * Time.deltaTime;

        tick += Time.deltaTime;

        if (tick > 1.0f)
        {
            gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, bounceForce, 0));
        }

        if (isDisappointed)
        {
            transform.position = Vector3.MoveTowards(transform.position, origin, step * .5f);
            transform.LookAt(origin);
            return;
        }

        if (this.target)
        {
            DestructibleBehavior db = target.GetComponent<DestructibleBehavior>();
            
            if (db)
            {
                if (db.getHp() > 0)
                {
                    RaycastHit hit;
                    // Does the ray intersect any objects excluding the player layer
                    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
                    {
                        if (hit.collider.gameObject.tag == "destructible")
                        {
                            target = hit.collider.gameObject;
                        }

                        if (hit.distance > distanceFromWall)
                        {
                            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
                            transform.LookAt(target.transform);
                        }
                        else
                        {
                            HandleAttack();
                        }
                    }
                }
                else
                {
                    if (hasGold)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, origin, step * 2);
                        transform.LookAt(origin);
                        state = "looted";

                        float distance = Vector3.Distance(transform.position, origin);

                        if (distance < 10)
                        {
                            GameObject.Destroy(gameObject);
                        }
                    } else
                    {
                        transform.position = Vector3.MoveTowards(transform.position, lootTarget.transform.position, step);
                        transform.LookAt(lootTarget.transform);
                        state = "looting";
                    }
                }
            }
            
        }
    }

    public void SetDisappointed(bool isDisappointed)
    {
        this.isDisappointed = isDisappointed;
    }

    private void HandleAttack()
    {
        DestructibleBehavior db = target.GetComponent<DestructibleBehavior>();
        state = "attacking";

        if (db)
        {
            db.HandleAttack(damage, gameObject);
        }
    } 

    private void HandleTargetDestroyed()
    {
        state = "moving";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "destructible")
        {
            state = "attacking";
        } else if (other.tag == "MainTower")
        {
            hasGold = true;
            GameManager.ChangeGold(-100);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Destructible" && (state == "attacking" || state == "idle"))
        {
            
        }
    }
}
