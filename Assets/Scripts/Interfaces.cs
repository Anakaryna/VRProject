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
    public FixedJoint Grab(Rigidbody body);
    public void Release(FixedJoint fixedJoint, Vector3 handsPosition);
}

public interface IStorable
{
    public FixedJoint Stored { get; set; }
    public FixedJoint Store(Vector3 releasePoint, GameObject storage);
    public bool StorageRelease();
}