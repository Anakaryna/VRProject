using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour, IProjectile
{
    public Vector3 Target { get; set; }
    public Vector3 Origin { get; set; }
    public float Speed { get; set; }
    public float MaxDistance { get; set; } = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Target - Origin), 1);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * (Time.deltaTime * Speed);
        
        if (Vector3.Distance(transform.position, Origin) > MaxDistance)
        {
            Destroy(gameObject);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out ICriticalHit critical))
        {
            critical.SendCritical(75);
        }
        else if(other.gameObject.TryGetComponent(out IDamagable damagable))
        {
            damagable.Hit(35);
        }
        Destroy(gameObject);
    }
}