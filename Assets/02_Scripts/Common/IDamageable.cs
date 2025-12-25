using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(int damage, GameObject source, bool isCritical);
}
