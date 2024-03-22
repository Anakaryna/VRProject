using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurretManager : MonoBehaviour
{

    public List<Transform> turretsCanonHeads;
    public Transform player;
    public int damping = 2;
    
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (var turret in turretsCanonHeads)
        {
            turret.gameObject.GetComponentInParent<ICooldown>().resetCooldown();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (!player.IsUnityNull())
        {
            var playerPos = player.position;
            int i = 0;
            foreach (var turret in turretsCanonHeads)
            {

                if (turret.IsUnityNull())
                {
                    turretsCanonHeads.RemoveAt(i);
                    continue;
                }
                
                var colliderToDisable = turret.GetChild(0).GetChild(0).GetComponent<Collider>();
                var turretPos = turret.position;
            
                RaycastHit hit;
                colliderToDisable.enabled = false;
                var rayTest = Physics.Raycast(turretPos, playerPos - turretPos, out hit, 20f);
            
                if (rayTest && hit.collider.GameObject() == player.GameObject())
                {
                    var lookPos = playerPos - turretPos;
                
                    Debug.DrawRay(turretPos, lookPos, Color.red);
                    Debug.DrawRay(turretPos, turret.forward * 20, Color.yellow);

                    if (playerPos.y < turretPos.y)
                    {
                        lookPos.y = 0;
                    }
        
                    var rotation = Quaternion.LookRotation(lookPos);
                    turret.rotation = Quaternion.Slerp(turret.rotation, rotation, Time.deltaTime * damping);

                    var turretInterface = turret.gameObject.GetComponentInParent<ICooldown>();

                    turretInterface.updateCooldown(Time.deltaTime);
                    
                    print(turretInterface.Cooldown);

                    if (turretInterface.Cooldown <= 0)
                    {
                        turretInterface.resetCooldown();
                        shoot(turretPos, turret);
                    }
                    
                    
                
                }
                else
                {
                    turret.gameObject.GetComponentInParent<ICooldown>().resetCooldown();
                }
                colliderToDisable.enabled = true;

                ++i;
            }
        }
        
    }

    public void shoot(Vector3 turretPos, Transform turret)
    {
        if (Physics.Raycast(turretPos, turret.forward, out RaycastHit hit2, 20f))
        {
            if (hit2.collider.GameObject().TryGetComponent(out ICriticalHit critical))
            {
                critical.SendCritical(0);
            } else if (hit2.collider.GameObject().TryGetComponent(out IDamagable damagable))
            {
                damagable.Hit(35);
            }
        }
    }
}
