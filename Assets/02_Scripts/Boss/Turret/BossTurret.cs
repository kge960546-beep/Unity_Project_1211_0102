using UnityEngine;

public class BossTurret : MonoBehaviour
{
    protected IBossTurretState currentState;

    [Header("Target")]
    public Transform player;

    [Header("Move Settings")]
    public float walkSpeed = 3.0f;
    public float chaseSpeed = 4.5f;
    public float chaseRange = 20.0f;
    public float closeFlipStopRange = 1.3f;

    [Header("Components")]
    [HideInInspector] public Animator anim;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public BoxCollider2D boxcol;

    [Header("Status")]
    public bool isDead = false;

    protected Vector2 moveDirection;
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
        ChangeState(new BossTurretWalkState());
    }

    private void Update()
    {
        // 사망 체크
        if (isDead && !(currentState is BossTurretDeadState))
        {
            ChangeState(new BossTurretDeadState());
            return;
        }

        currentState?.UpdateState(this);

        float dist = Vector2.Distance(transform.position, player.position);

        // 너무 가까우면 멈추기
        if (dist <= closeFlipStopRange)
        {
            StopMove();
        }

        // 이동 방향 기준 플립
        if (Mathf.Abs(moveDirection.x) > 0.05f && dist > closeFlipStopRange)
        {
            transform.localScale = new Vector3(moveDirection.x > 0 ? 1 : -1, 1, 1);
        }
    }

    private void FixedUpdate()
    {
        transform.position += (Vector3)(moveDirection * moveSpeed * Time.fixedDeltaTime);
    }


    public void ChangeState(IBossTurretState newState)
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
