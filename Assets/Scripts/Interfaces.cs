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

public interface Grabable
{
    public bool Grabbed { get; set; }
}