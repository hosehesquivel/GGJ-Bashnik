using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float xInput = 0;
    float yInput = 0;
    bool repairing = false;

    private GameObject repairTarget = null;

    public float speed = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();

        Move();

        Repair();
    }

    void Move()
    {
        if (repairing)
        {
            return;
        }

        Vector3 v3 = new Vector3(xInput * speed * Time.deltaTime, 0, yInput * speed * Time.deltaTime);
        Vector3 nextPosition = transform.position + v3;

        if (v3.x != 0 || v3.z != 0)
        {
            transform.LookAt(nextPosition);
            transform.Translate(v3, Space.World);
        }
    }

    void Repair()
    {
        if (repairing && repairTarget)
        {
            Debug.Log("REPAIRING...");
        }
    }

    void HandleInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        xInput = x;
        yInput = y;

        
        if (Input.GetKeyDown("space"))
        {
            repairing = true;
        }

        if (Input.GetKeyUp("space"))
        {
            repairing = false;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Destructible")
        {
            repairTarget = other.gameObject;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject == repairTarget)
        {
            repairTarget = null;
        }
    }
}
