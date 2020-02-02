using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleBehavior : MonoBehaviour
{
    public float maxHp = 10;
    public float damageInterval = 1;

    public float repairInterval = 1;
    public float repairPerTick = 2;

    public GameObject destroyedObject;
    public GameObject healthyView;

    private float hp { get; set; } = 10;
    private string state = "idle";
    private bool isBeingAttacked = false;
    private float tick = 0;
    private float damagePerTick = 0;
    private bool isBeingRepaired = false;
    private GameObject beingAttackedBy = null;
    private Material originalMaterial;

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
        originalMaterial = gameObject.GetComponent<MeshRenderer>().material;    }

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

        if (hp < 0)
        {
            HandleDeath();
        }
    }

    public void HandleAttack(float damage, GameObject source)
    {
        this.damagePerTick = damage;
        isBeingAttacked = true;

        beingAttackedBy = source;
    }

    public void setRepairing(bool isBeingRepaired)
    {
        if (!this.isBeingRepaired && isBeingRepaired)
        {
            tick = 0;

            MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();

            Material redMaterial = Resources.Load<Material>("Materials/Green");
            mr.material = redMaterial;
        }

        this.isBeingRepaired = isBeingRepaired;
    }

    public float getHp()
    {
        return hp;
    }

    public void setRepairTarget(bool isTarget)
    {
        MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
        
        if (isTarget)
        {
            Material redMaterial = Resources.Load<Material>("Materials/Red");
            mr.material = redMaterial;

        } else
        {
            mr.material = originalMaterial;
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
        }
    }

    private void HandleRepaired()
    {
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

        if (healthyView)
        {
            healthyView.SetActive(true);
        }
    }

    private void HandleDeath()
    {
        isBeingAttacked = false;
        destroyedObject.SetActive(true);

        MeshRenderer mr = GetComponent<MeshRenderer>();
        mr.enabled = false;

        gameObject.layer = 14;

        if (healthyView)
        {
            healthyView.SetActive(false);
        }
    }
}
