using UnityEngine;

public class BossFlower : MonoBehaviour
{
    [HideInInspector] public IBossFlowerState currentState;

    [Header("Target")]
    public Transform player;

    [Header("Components")]
    [HideInInspector] public Animator anim;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public BoxCollider2D boxcol;
    public BossBaseDataSO bbData;

    [Header("Status")]
    public bool isDead = false;
    public int maxHp;
    public int currentHp;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        boxcol = GetComponent<BoxCollider2D>();
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GetService<GameContextService>().RegisterBossMonsterObject(gameObject);
        }
    }
    private void Start()
    {
        ChangeState(new BossFlowerIdleState());
    }
    private void Update()
    {
        if (isDead) return;

        currentState?.UpdateState(this);
    }
    private void OnEnable()
    {
        if(bbData == null)
        {
            Debug.Log("참조할 데이터가 없습니다");
            return;
        }
        isDead = false;
        maxHp = bbData.bossHp;
        currentHp = maxHp;
    }
    public void TakeDamage(int damage)
    {
        if(isDead) return;

        int finalDamage = Mathf.Max(damage, 0);
        currentHp = Mathf.Max(currentHp - finalDamage, 0);

        if (currentHp <= 0)
        {
            isDead = true;
            ChangeState(new BossFlowerDeadState());
        }
    }
    public void ChangeState(IBossFlowerState newState)
    {
        if (isDead && !(newState is BossFlowerDeadState)) return;
        currentState?.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }
}