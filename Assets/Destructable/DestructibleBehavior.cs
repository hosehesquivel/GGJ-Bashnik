using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleBehavior : MonoBehaviour
{
    public float hp = 10;
    public float damageInterval = 3;

    private string state = "idle";
    private bool beingAttacked = false;
    private float tick = 0;
    private float damagePerTick = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (beingAttacked)
        {
            HandleDamage();
        }

        if (hp < 0)
        {
            HandleDeath();
        }
    }

    public void HandleAttack(float damage)
    {
        this.damagePerTick = damage;
        beingAttacked = true;
    }

    private void HandleDamage()
    {
        tick += Time.deltaTime;
        if (tick > damageInterval)
        {
            tick -= damageInterval;
            hp -= damagePerTick;

            Debug.Log("HP: " + hp);

            if (hp <= 0)
            {
                HandleDeath();
            }
        }
    }

    private void HandleDeath()
    {
        beingAttacked = false;

        Rigidbody rb = GetComponent<Rigidbody>();
        MeshCollider mc = GetComponent<MeshCollider>();

        mc.enabled = false;
    }
}
