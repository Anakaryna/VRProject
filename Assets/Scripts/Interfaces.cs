using Unity.VisualScripting;
using UnityEngine;

public interface IDamagable
{
    public int Health { get; set; }
    
    void Hit(int damage);
    
    void CriticalHit(int damage);

    void SelfDestroy();
}

// used for the critical hitboxes to call CriticalHit() in IDamagable.
// Usefull for critical spots, useless for move based/luck based/other critical hits : use directly CriticalHit() instead
public interface ICriticalHit
{
    void SendCritical(int damage);
}

public interface IGrabbable
{
    public FixedJoint GrabbedFixedJoint { get; set; }
    public FixedJoint Grab(Rigidbody body, out bool makeTransfer);
    public void Release(FixedJoint fixedJoint, Vector3 handsPosition, out bool stored);
}

public interface IStorable
{
    public FixedJoint Stored { get; set; }
    public FixedJoint Store(Vector3 releasePoint, GameObject storage);

    public bool StorageRelease();
}

public interface IProjectile
{
    public Vector3 Target { get; set; }
    public Vector3 Origin { get; set; }
    
    public float Speed { get; set; }
    public float MaxDistance { get; set; }
}

public interface IGun
{
    public int AmmoInCurrMag { get; set; }
    public void Fire();
    public void Reload();
}