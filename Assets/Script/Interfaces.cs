using UnityEditor;
using UnityEngine;

public interface ICooldown 
{
    public float Cooldown { get; set; }

    void updateCooldown(float value);
    void resetCooldown();
}

public interface IDamagable
{
    public int Health { get; set; }
    void Hit(int damage);
    
    void CriticalHit(int damage);

    void SelfDestroy();
}

public interface ICriticalHit
{
    void SendCritical(int damage);
}

public interface IPlayable
{
    void SwitchScene(SceneAsset scene);
}

public interface IProjectile
{
    public Vector3 Target { get; set; }
    public Vector3 Origin { get; set; }
    
    public float Speed { get; set; }
    public float MaxDistance { get; set; }
    public float TrailTime { get; set; }
    public float TrailWidth { get; set; }
}