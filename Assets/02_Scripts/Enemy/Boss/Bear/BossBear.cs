using UnityEngine;

public class BossBear : MonoBehaviour
{
    [HideInInspector] public IBossBearState currentState;

    [Header("Target")]
    public Transform player;

    [Header("Move Settings")]
    public float walkSpeed = 4.0f;
    public float chaseSpeed = 5.0f;
    public float chaseRange = 25.0f;
    public float closeFlipStopRange = 1.3f;

    [Header("Rush Settings")]
    public float rushSpeed = 13.0f;
    public float rushDuration = 5.0f;
    public float rushRange = 20.0f;
    public float rushCooldown = 8.0f;
    public float rushDelay = 5.0f;
    [HideInInspector] public float lastRushTime = -99f;
    [HideInInspector] public float spawnTime;

    [Header("Components")]
    [HideInInspector] public Animator anim;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public BoxCollider2D boxcol;
    public bossData enemyData;

    [Header("Status")]
    public bool isDead = false;
    public int maxHp;
    public int currentHp;

    [Header("Damage Settings")]
    public float damageInterval = 1.0f; // 플레이어와 닿아있는 동안 몇 초마다 데미지를 주는가?  

    public Vector2 moveDirection;
    protected float moveSpeed;
    private EnemyHp enemyHp;

    float lastDamageTime = -999f; // 플레이어와 몬스터의 콜라이더가 닿아있는 동안 데미지를 주는 마지막 시간 초기화 

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        boxcol = GetComponent<BoxCollider2D>();
        enemyHp = GetComponent<EnemyHp>();

        if(GameManager.Instance != null)
        {
            GameManager.Instance.GetService<GameContextService>().RegisterBossMonsterObject(gameObject);
        }

    }
    private void Start()
    {
        spawnTime = Time.time;        
        ChangeState(new BossBearWalkState());
    }
    private void Update()
    {
        if (isDead)  return;

        currentState?.UpdateState(this);       
    }
    private void FixedUpdate()
    {
        if (isDead) return;
        rb.velocity = moveDirection * moveSpeed;
        //rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);       
    }
    private void OnEnable()
    {
        if(enemyData == null)
        {
            Debug.Log("참조할 데이터가 없습니다");
            return;
        }

        if(player == null)
        {
            var playerT = GameObject.FindGameObjectWithTag("Player");
            if (playerT != null) player = playerT.transform;
        }

        isDead = false;
        maxHp = enemyData.maxHp;
        currentHp = maxHp;

        if(enemyHp != null)
        {
            enemyHp.SubscribeEnemyTakeDamageEvent(OnTakeDamage);
        }
    }
    private void OnDisable()
    {
        if(enemyData != null)
        {
            enemyHp.UnsubscribeEnemyTakeDamageEvent(OnTakeDamage);
        }
    }
    public void OnTakeDamage(int currentHp, int maxHp)
    { 
        // TODO: 히트 이펙트 있으면 여기서 재생

        if (currentHp <= 0)
        {
            isDead = true;
            StopMove();
            ChangeState(new BossBearDeadState());
        }
    }
    public void ChangeState(IBossBearState newState)
    {
        if (isDead && !(newState is BossBearDeadState)) return;

        currentState?.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }
    public void SetMove(Vector2 direction, float speed)
    {
        moveDirection = direction.normalized;
        moveSpeed = speed;
    }
    public void StopMove()
    {
        moveDirection = Vector2.zero;
        moveSpeed = 0f;

        if (rb != null) rb.velocity = Vector2.zero;
    }
    // 벽 충돌 감지
    private void OnTriggerStay2D(Collider2D other)
    {
        if (isDead) return;
        if (currentState is BossBearRushState)
        {
            // "Enemy" 레이어의 벽에 부딪히면 돌진 종료 후 추격으로 전환
            if (other.gameObject.layer == LayerMask.NameToLayer("Boss"))
            {
                StopMove();
                ChangeState(new BossBearChaseState());
            }
        }

        LayerService ls = GameManager.Instance.GetService<LayerService>();
        DamageManagementService dms = GameManager.Instance.GetService<DamageManagementService>();

        if (other.gameObject.layer == ls.playerLayer
            && Time.time - lastDamageTime >= damageInterval
            && other.gameObject.TryGetComponent(out IDamageable damageable))
        {
            dms.QueueDamage(enemyData.attack, gameObject, damageable, false);
            lastDamageTime = Time.time;
        }
    }
    protected virtual void OnDrawGizmosSelected()
    {
        if (player == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, closeFlipStopRange);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, player.position);
    }
}