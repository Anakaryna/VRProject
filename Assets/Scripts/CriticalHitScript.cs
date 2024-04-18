using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalHitScript : MonoBehaviour, ICriticalHit
{
    public GameObject o;

    public void SendCritical(int damage)
    {
        if (o.TryGetComponent(out IDamagable damagable))
        {
            damagable.CriticalHit(damage);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
