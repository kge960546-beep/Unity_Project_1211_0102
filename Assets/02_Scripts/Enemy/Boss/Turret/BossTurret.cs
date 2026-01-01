using UnityEngine;

public class BossTurret : MonoBehaviour
{
    [HideInInspector] public IBossTurretState currentState;

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
    public BossData bossData;

    [Header("Status")]
    public bool isDead = false;
    public int maxHp;
    public int currentHp;

    [HideInInspector] public Vector2 moveDirection;
    [HideInInspector] public float moveSpeed;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        boxcol = GetComponent<BoxCollider2D>();
        if(GameManager.Instance != null)
        {
            GameManager.Instance.GetService<GameContextService>().RegisterBossMonsterObject(gameObject);
        }        
    }
    private void Start()
    {
        ChangeState(new BossTurretWalkState());
    }
    private void Update()
    {
        if (isDead) return;
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
        if(bossData == null)
        {
            Debug.Log("참조항 데이터가 없습니다");
            return;
        }
        isDead = false;
        maxHp = bossData.maxHp;
        currentHp = maxHp;
    }
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        int finalDamage = Mathf.Max(damage, 0);
        currentHp = Mathf.Max(currentHp - finalDamage, 0);

        if (currentHp <= 0)
        {
            isDead = true;
            StopMove();
            ChangeState(new BossTurretDeadState());
        }
    }    
    public void ChangeState(IBossTurretState newState)
    {
        if (isDead && !(newState is BossTurretDeadState)) return;
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