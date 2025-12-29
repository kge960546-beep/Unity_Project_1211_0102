using UnityEngine;

public class ProjectileCollisionDamageBehaviour : MonoBehaviour
{
    public bool IsInflictingDamageOnTriggerStay{ set; private get; }
    public float CriticalRate { set; private get; }
    public int OrdinaryDamage { set; private get; }
    public int CriticalDamage { set; private get; }
    [Tooltip("Random value with uniform distribution from (1 - ratio) to (1 + ratio)")] public int DamageVariationRatio { set; private get; }

    public struct Cooldown
    {
        public const float EVERY_TICK = 0;
        public const float ONLY_ONCE = -1;
        public float interval;
        public float timer;
    }
    private Cooldown damageCooldown;

    private void OnEnable()
    {
        damageCooldown.timer = 0f;
    }

    private void FixedUpdate()
    {
        damageCooldown.timer += Time.fixedDeltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // expects to be filtered with collision matrix
        if (collision.gameObject.TryGetComponent(out IDamageable damagable)) InflictDamage(damagable);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // expects to be filtered with collision matrix
        if (!IsInflictingDamageOnTriggerStay) return;
        if (collision.gameObject.TryGetComponent(out IDamageable damagable)) InflictDamage(damagable);
    }

    private void InflictDamage(IDamageable target)
    {
        RandomService rs = GameManager.Instance.GetService<RandomService>();
        bool isCritical = rs.random.NextFloat(0f, 1f) < CriticalRate;
        float randomRatio = GameManager.Instance.GetService<RandomService>().random.NextFloat(1f - DamageVariationRatio, 1f + DamageVariationRatio);
        int damage = isCritical ? CriticalDamage : OrdinaryDamage;
        damage = (int)(damage * randomRatio);
        GameManager.Instance.GetService<DamageManagementService>().QueueDamage(damage, gameObject, target, isCritical);
    }
}
