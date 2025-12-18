using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class TurretController : MonoBehaviour
{
    [Header("Turret Data (SO)")]
    public EnemyData monsterData;

    [Header("Target (Range Check Only)")]
    public Transform player; // 사거리 체크용

    [Header("Fire Settings")]
    public GameObject bulletPrefab; // 탄환 프리팹
    public Transform firePoint;          // 레이저 발사 위치
    public float fireInterval = 1.5f;    // 발사 간격
    public float fireRange = 8.0f;        // 발사 사거리

    [Header("Fixed Fire Direction")]
    public Vector2 fireDirection = Vector2.right; // 고정 발사 방향

    Rigidbody2D rb;
    Animator animator;

    int currentHp;
    bool isDead = false;
    float lastFireTime = -999f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // 고정된 포탑의 위치 
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.velocity = Vector2.zero;

        currentHp = monsterData.maxHp;

        // 방향 정규화 
        fireDirection = fireDirection.normalized;
    }

    private void Update()
    {
        if (isDead) return;
        if (player == null) return;

        float distanceToPlayer =
            Vector2.Distance(transform.position, player.position);

        // 사거리 안에 들어오면 발사
        if (distanceToPlayer <= fireRange)
        {
            if (Time.time - lastFireTime >= fireInterval)
            {
                
                lastFireTime = Time.time;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHp -= damage;

        if (currentHp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger("isDead");

        GetComponent<Collider2D>().enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        // 사거리
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fireRange);

        // 발사 방향 확인용 
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(
            firePoint != null ? firePoint.position : transform.position,
            (firePoint != null ? firePoint.position : transform.position)
            + (Vector3)fireDirection * 2f
        );
    }
}
