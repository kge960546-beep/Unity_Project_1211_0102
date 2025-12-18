using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]


public class MonsterController : MonoBehaviour
{
    [Header("Monster Data (SO)")]
    public EnemyData enemyData;

    [Header("Target")]
    public Transform player;
    private string playerTag = "Player";

    [Header("Move")]
    [SerializeField] float moveSpeed; // 몬스터의 실제 이동 속도 

    [Header("Damage Settings")]
    public float damageInterval = 1.0f; // 플레이어와 닿아있는 동안 몇 초마다 데미지를 주는가?  

    [Header("Separation Settings")]
    public float separationRadius = 1.2f; // 다른 몬스터 오브젝트를 밀어내기 위한 반경 
    public float separationForce = 1.1f; // 밀어내는 힘의 세기 

    [Header("Flip Settings")]
    public float flipStopDistance = 0.6f; // 플레이어가 이 반경 안에 있다면 몬스터 오브젝트의 플립을 정지 

    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer sr;
    private EnemyHp enemyHp;  
    

    int currentHp;
    bool isDead = false;
    float lastDamageTime = -999f; // 플레이어와 몬스터의 콜라이더가 닿아있는 동안 데미지를 주는 마지막 시간 초기화 

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyHp = GetComponent<EnemyHp>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        rb.bodyType = RigidbodyType2D.Kinematic;             
    }
    private void Update()
    {
        if (isDead) return;
        if (player == null) return;

        ChasePlayer();
       
        float distanceToPlayer = Vector2.Distance(transform.position,player.position);

        // 플레이어와 떨어져 있을때 방향전환 
        if (distanceToPlayer > flipStopDistance) 
        {
            Flip(player.position.x);
        }
    }
    private void FixedUpdate()
    {
        
    }
    private void OnEnable()
    {
        var go = GameObject.FindGameObjectWithTag(playerTag);
        player = go != null ? go.transform : null;

        if(enemyHp != null && enemyData != null)
        {
            enemyHp.Init(enemyData, EnemyType.Normal, exp: 1);
        }
        if(enemyHp != null)
        {
            enemyHp.SubscribeEnemyTakeDamageEvent(TakeDamage);            
        }
    }
    public void TakeDamage(int currentHp, int maxHp)
    {
        Debug.Log($"Enemy HP: {currentHp}/{maxHp}");
        if(sr != null)
            StartCoroutine(HitEffect());        
    }    
    IEnumerator HitEffect()
    {
        sr.enabled = false;
        yield return new WaitForSeconds(0.05f);
        sr.enabled = true;
    }
    void ChasePlayer() 
    {
       // 플레이어를 향해 추격하기 위한 방향 벡터 
       Vector2 toPlayer = ((Vector2)player.position - (Vector2)transform.position).normalized;
       
       // 주변에 있는 다른 오브젝트로 부터 겹치지 않게 밀려나는 방향 
       Vector2 separation = CalculateSeparation();
       
       // 최종이동 방향 =  플레이어를 향하는 힘 + 서로 밀어내는 힘
       Vector2 finalDir= (toPlayer + separation).normalized;
        
       rb.position += (finalDir * moveSpeed * Time.deltaTime);
    }
   

    Vector2 CalculateSeparation() 
    {
        // 오브젝트가 겹치지 않게 하기 위한 최종분리 벡터 
        Vector2 separation = Vector2.zero;

        // 현재 위치 기준으로 separationRadius 안에 있는 모든 콜라이더 탐색
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position,separationRadius);

        foreach (var hit in hits) 
        {
            // 자기 자신은 제외 
            if (hit. gameObject == gameObject)
                continue;

            if (hit.CompareTag("Enemy")) 
            {
                //  오브젝트와 가까울수록 밀려나기 위한 거리를 역수 방식으로 계산
                Vector2 diff = (Vector2)(transform.position - hit.transform.position);

                float distance = diff.magnitude;
                if (distance > 0f) 
                {
                    // 가까운 오브젝트 일수록 강하게 밀림 
                    separation += diff.normalized / distance;
                }
            }
        }
        return separation * separationForce;
    }

    void Flip(float targetX) 
    {
        if (targetX > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }

    // TODO:플레이어와 닿았을때 플레이어의 체력을 감소 시키는 임시 로직
    private void OnTriggerStay2D(Collider2D other)
    {
        if (isDead) return;

        if (other.CompareTag("Player")) 
        {
            if (Time.time - lastDamageTime >= damageInterval) 
            {
                PlayerHp hp = other.GetComponent<PlayerHp>();
                if (hp != null) 
                {
                    hp.TakeDamage(enemyData.attack);
                    lastDamageTime = Time.time;
                }
            }
        }
    }

    // 몬스터가 플레이어를 추격할때 서로 겹치지 않고 밀어내야 하는 로직이 필요함 
    public void SetMoveSpeed(float speed) 
    {
       moveSpeed = speed;   
    }
    public float GetMoveSpeed() 
    {
        return moveSpeed;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, separationRadius);
    }
}
