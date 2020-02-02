using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float xInput = 0;
    float yInput = 0;
    bool repairing = false;

    private GameObject repairTarget = null;
    private Quaternion lastRotation;

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
        Vector3 nextPosition = transform.position + v3 * -1;

        if (v3.x != 0 || v3.z != 0)
        {
            transform.LookAt(nextPosition);
            transform.Translate(v3, Space.World);
            lastRotation = transform.rotation;
        } else
        {
            if (lastRotation != null)
            {
                transform.rotation = lastRotation;
            }
            
        }
    }

    void Repair()
    {
        if (repairing && repairTarget)
        {
            DestructibleBehavior db = repairTarget.GetComponent<DestructibleBehavior>();
            Debug.Log("Player is setting block repairing");
            db.setRepairing(true);
        }
    }

    void HandleInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        xInput = x;
        yInput = y;


        if (Input.GetKeyDown("space") && !repairing)
        {
            repairing = true;
        }

        if (Input.GetKeyUp("space"))
        {
            repairing = false;

            if (repairTarget)
            {
                DestructibleBehavior db = repairTarget.GetComponent<DestructibleBehavior>();
                db.setRepairing(false);
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Destructible")
        {
            removeCurrentRepairTarget();

            repairTarget = other.gameObject;

            DestructibleBehavior db = repairTarget.GetComponent<DestructibleBehavior>();

            if (db)
            {
                db.setRepairTarget(true);
            }
            
        }
    }

    public void OnTriggerExit(Collider other)
    {
        repairing = false;

        if (other.gameObject == repairTarget)
        {
            removeCurrentRepairTarget();
        }
    }

    private void removeCurrentRepairTarget()
    {
        if (repairTarget)
        {
            DestructibleBehavior db = repairTarget.GetComponent<DestructibleBehavior>();
            if (db)
            {
                db.setRepairTarget(false);
                db.setRepairing(false);
            }

            repairTarget = null;
        }
    }
}
