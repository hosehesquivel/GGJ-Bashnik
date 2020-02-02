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
    public GameObject gate;
    public GameObject tooltip;

    public GameObject repairParticle;
    public GameObject repairedParticle;
    public GameObject explosionParticle;
    public GameObject fireParticle;

    private float hp { get; set; } = 10;
    private bool isBeingAttacked = false;
    private bool isBeingRepaired = false;
    private float tick = 0;
    private float damagePerTick = 0;
    private GameObject beingAttackedBy = null;
    private Material originalMaterial;
    private GameObject myTooltip;
    private GameObject myRepairParticle;
    private GameObject myExplosionParticle;
    private GameObject myFireParticle;
    private GameObject myRepairedParticle;

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
        originalMaterial = gameObject.GetComponent<MeshRenderer>().material;

        myFireParticle = ShowParticle(fireParticle);

        //myFireParticle.SetActive(false);
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

        if (hp < maxHp)
        {
            myFireParticle.SetActive(true);
        } else
        {
            myFireParticle.SetActive(false);
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

            myRepairParticle = ShowParticle(repairParticle);
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
    

    private GameObject ShowParticle(GameObject prefab)
    {
        Bounds bnds = new Bounds(transform.position, Vector3.zero);

        GameObject particle = GameObject.Instantiate(prefab, transform.position, Quaternion.identity);

        particle.transform.position = new Vector3(particle.transform.position.x, bnds.size.y + 10, particle.transform.position.z);

        return particle;
    }

    private void HideParticle(GameObject go)
    {
        if (go)
        {
            GameObject.Destroy(go);
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

        if (gate)
        {
            gate.SetActive(true);
        }

        gameObject.layer = 11;

        HideParticle(myRepairParticle);
    }

    private void HandleDeath()
    {
        isBeingAttacked = false;
        destroyedObject.SetActive(true);

        MeshRenderer mr = GetComponent<MeshRenderer>();
        mr.enabled = false;

        if (gate)
        {
            gate.SetActive(false);
        }

        gameObject.layer = 14;

        myExplosionParticle = ShowParticle(explosionParticle);
    }
}
