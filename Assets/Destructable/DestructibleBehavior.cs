using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleBehavior : MonoBehaviour
{
    public float maxHp = 10;
    public float damageInterval = 1;

    public float repairInterval = 1;
    public float repairPerTick = 2;

    public Vector3 offset;

    public GameObject destroyedObject;
    public GameObject healthyView;
    public GameObject tooltip;

    private float hp { get; set; } = 10;
    private bool isBeingAttacked = false;
    private bool isBeingRepaired = false;
    private float tick = 0;
    private float damagePerTick = 0;
    private GameObject beingAttackedBy = null;
    private Material originalMaterial;
    private GameObject myTooltip;

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
        originalMaterial = gameObject.GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (isBeingRepaired)
        {
            HandleRepair();
        }
        else if (isBeingAttacked)
        {
            HandleDamage();
        }
    }

    public void HandleAttack(float damage, GameObject source)
    {
        this.damagePerTick = damage;
        isBeingAttacked = true;

        beingAttackedBy = source;
    }

    public void setRepairing(bool shouldRepair)
    {
        if (!this.isBeingRepaired && shouldRepair)
        {
            tick = 0;

            MeshRenderer mr = healthyView.GetComponent<MeshRenderer>();

            Material redMaterial = Resources.Load<Material>("Materials/Green");
            mr.material = redMaterial;

            if (myTooltip)
            {
                myTooltip.GetComponent<Tooltip>().setState("repairing");
                myTooltip.GetComponent<Tooltip>().setScale(hp/maxHp);
            }
        }

        this.isBeingRepaired = shouldRepair;

        if (!shouldRepair)
        {
            if (myTooltip)
            {
                myTooltip.GetComponent<Tooltip>().setState("repairable");
            }
        }
    }

    public float getHp()
    {
        return hp;
    }

    public void setRepairTarget(bool isTarget)
    {
        MeshRenderer mr = healthyView.GetComponent<MeshRenderer>();
        
        if (isTarget)
        {
            Material redMaterial = Resources.Load<Material>("Materials/Red");
            mr.material = redMaterial;

            Bounds bnds = new Bounds(transform.position, Vector3.zero);

            myTooltip = GameObject.Instantiate(tooltip, transform.position, Quaternion.identity);

            myTooltip.transform.position = new Vector3(myTooltip.transform.position.x, bnds.size.y + 10, myTooltip.transform.position.z);

        } else
        {
            mr.material = originalMaterial;

            if (myTooltip)
            {
                GameObject.Destroy(myTooltip);
            }
        }
        
    }

    private void HandleDamage()
    {
        tick += Time.deltaTime;
        if (tick > damageInterval)
        {
            tick -= damageInterval;
            hp -= damagePerTick;

            Debug.Log("Damaging - HP: " + hp);

            if (hp <= 0)
            {
                HandleDeath();
            }
        }
    }

    private void HandleRepair()
    {
        tick += Time.deltaTime;
        
        if (tick > repairInterval)
        {
            tick -= repairInterval;
            hp += repairPerTick;
            hp = Mathf.Min(maxHp, hp);

            Debug.Log("Repairing - HP: " + hp);

            if (hp == maxHp)
            {
                HandleRepaired();
            }

            if (myTooltip)
            {
                myTooltip.GetComponent<Tooltip>().setScale((float)hp/maxHp);
            }
        }
    }

    private void HandleRepaired()
    {
        Debug.Log("Repaired");
        isBeingAttacked = false;
        destroyedObject.SetActive(false);

        if (beingAttackedBy)
        {
            EnemyBehavior eb = beingAttackedBy.GetComponent<EnemyBehavior>();
            beingAttackedBy = null;
            eb.SetDisappointed(true);
            isBeingRepaired = false;
        }

        MeshRenderer mr = GetComponent<MeshRenderer>();
        mr.enabled = true;

        gameObject.layer = 11;
    }

    private void HandleDeath()
    {
        Debug.Log("Death");
        isBeingAttacked = false;
        destroyedObject.SetActive(true);

        MeshRenderer mr = GetComponent<MeshRenderer>();
        mr.enabled = false;

        gameObject.layer = 14;
    }
}
