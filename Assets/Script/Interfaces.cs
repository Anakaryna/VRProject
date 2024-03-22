using UnityEditor;

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