using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TurretScript : MonoBehaviour, IDamagable, ICooldown
{
    

    public GameObject turret;
    public int Health { get; set; }

    public int startingHealth = 350;
    public float startingCooldown = 30f;
    
    public float Cooldown { get; set; }

    public void updateCooldown(float value)
    {
        Cooldown -= value;
    }

    public void resetCooldown()
    {
        Cooldown = startingCooldown;
    }

    public void Hit(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            SelfDestroy();
        }
    }
    
    public void CriticalHit(int damage)
    {
        SelfDestroy();
    }

    public void SelfDestroy()
    {
        Destroy(turret);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Health = startingHealth;
        Cooldown = startingCooldown;
    }

    private void OnEnable()
    {
        Health = startingHealth;
        Cooldown = startingCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
