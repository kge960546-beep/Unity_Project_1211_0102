using UnityEngine;

public class BossBear : MonoBehaviour
{
    protected IBossBearState currentState;

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
    protected float lastRushTime = -99f;
    protected float spawnTime;

    [Header("Components")]
    [HideInInspector] public Animator anim;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public BoxCollider2D boxcol;

    [Header("Status")]
    public bool isDead = false;

    public Vector2 moveDirection;
    protected float moveSpeed;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        boxcol = GetComponent<BoxCollider2D>();
        GameManager.Instance.GetService<GameContextService>().RegisterBossMonsterObject(gameObject);
    }

    private void Start()
    {
        spawnTime = Time.time;
        ChangeState(new BossBearWalkState());
    }

    private void Update()
    {
        if (isDead && !(currentState is BossBearDeadState))
        {
            ChangeState(new BossBearDeadState());
            return;
        }

        currentState.UpdateState(this);

        float dist = Vector2.Distance(transform.position, player.position);

        // Rush 조건
        if (!(currentState is BossBearRushState))
        {
            if (Time.time >= spawnTime + rushDelay &&
                Time.time >= lastRushTime + rushCooldown &&
                dist < rushRange && dist > closeFlipStopRange)
            {
                ChangeState(new BossBearRushState(rushSpeed, rushDuration));
                return;
            }
        }
        else
        {
            lastRushTime = Time.time;
        }

        // 플레이어와 가까우면 이동 멈춤
        if (dist <= closeFlipStopRange)
            StopMove();

        // 좌우 반전 (돌진 상태일 때는 회전하지 않음)
        if (!(currentState is BossBearRushState))
        {
            if (Mathf.Abs(moveDirection.x) > 0.05f && dist > closeFlipStopRange)
                transform.localScale = new Vector3(moveDirection.x > 0 ? 1 : -1, 1, 1);
        }
    }

    private void FixedUpdate()
    {
        transform.position += (Vector3)(moveDirection * moveSpeed * Time.fixedDeltaTime);
    }

    public void ChangeState(IBossBearState newState)
    {
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
    }

    // 벽 충돌 감지
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentState is BossBearRushState)
        {
            // "Enemy" 레이어의 벽에 부딪히면 돌진 종료 후 추격으로 전환
            if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                StopMove();
                ChangeState(new BossBearChaseState());
            }
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
