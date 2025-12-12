using UnityEngine;

public class BossFlower : MonoBehaviour
{
    protected IBossFlowerState currentState;

    [Header("Target")]
    public Transform player;

    [Header("Components")]
    [HideInInspector] public Animator anim;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public BoxCollider2D boxcol;

    [Header("Status")]
    public bool isDead = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        boxcol = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        ChangeState(new BossFlowerIdleState());
    }

    private void Update()
    {
        if (isDead && !(currentState is BossFlowerDeadState))
        {
            ChangeState(new BossFlowerDeadState());
            return;
        }

        currentState?.UpdateState(this);

        // 플레이어의 이동경로 쪽으로 플립 
        if (player != null)
        {
            float dir = player.position.x - transform.position.x;

            if (Mathf.Abs(dir) > 0.1f)
            {
                transform.localScale = new Vector3(dir > 0 ? 1 : -1, 1, 1);
            }
        }
    }

    public void ChangeState(IBossFlowerState newState)
    {
        currentState?.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }
}
