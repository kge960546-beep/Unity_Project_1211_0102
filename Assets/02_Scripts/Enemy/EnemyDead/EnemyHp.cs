using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EnemyType
{
    Normal,
    Elite,
    Boss
}
public class EnemyHp : MonoBehaviour, IDamageable
{
    /// <summary>
    /// 적의 타입, 적의 죽은 위치, 떨어뜨릴 아이템 갯수
    /// </summary>
    private event Action<EnemyType, Vector3, int> onEnemyDeadEvent; //적이 죽었을때 적 타입과 위치를 전달
    /// <summary>
    /// 피격 이벤트: 얼마나 데미지가 주어졌는지(finalDamage), 현재 체력이 얼마나 남았는지 currentHp
    /// </summary>
    private event Action<int, int> onEnemyTakeDamageEvent;

    [Header("Enemy Setting")]
    [SerializeField] private EnemyType enemyType = EnemyType.Normal; //적 타입
    [SerializeField] private int maxHp;
    [SerializeField] private int dropExp = 1;

    //임시 레이어 지정
    [Header("Layer")]
    [SerializeField] int itemLayer;
    [SerializeField] int itemLayers;

    private int currentHp;
    private bool isDead = false;

    private Animator anim;
    private EnemyData enemyData;
    private KillCount kill;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        
        kill = FindAnyObjectByType<KillCount>();

        itemLayer = LayerMask.NameToLayer("Item");
        itemLayers = LayerMask.NameToLayer("PlayerProjectile");
    }
    private void Start()
    {
        if(enemyData == null)
        {
            Debug.Log("참조할 데이터가 없습니다");
            return;
        }       
        
    }
    #region EnemyDeadEvent
    /// <summary>
    /// 적 죽음 구독 해지
    /// </summary>
    /// <param name="action"></param>
    public void SubscribeEnemyDeadEvent(Action<EnemyType, Vector3, int> action)
    {
            
        onEnemyDeadEvent += action;
    }
    public void UnsubscribeEnemyDeadEvent(Action<EnemyType, Vector3, int> action)
    {
        onEnemyDeadEvent -= action;
    }
    #endregion
    #region EnemyTakeDamageEvent
    /// <summary>
    /// 적 피격 구독 해지
    /// </summary>
    /// <param name="action"></param>
    public void SubscribeEnemyTakeDamageEvent(Action<int, int> action)
    {       
        onEnemyTakeDamageEvent += action;
    }
    public void UnsubscribeEnemyTakeDamageEvent(Action<int, int> action)
    {
        onEnemyTakeDamageEvent -= action;
    }
    #endregion
     
    private void OnEnable()
    {        
        currentHp = maxHp;
        isDead = false;
        StopAllCoroutines();

        if(EnemyDead.instance != null)
        {
            EnemyDead.instance.RegisterEnemy(this);
        }
    }
    private void OnDisable()
    {
        if (EnemyDead.instance != null)
        {
            EnemyDead.instance.UnregisterEnemy(this);
        }

        //풀링시 남아있는 구독자가 있을수 있으니 강제 제거
        onEnemyDeadEvent = null;
        onEnemyTakeDamageEvent = null;
    }
    public void Init(EnemyData data, EnemyType type, int exp)
    {
        enemyData = data;
        enemyType = type;
        dropExp = exp;

        maxHp = enemyData.maxHp;
        currentHp = maxHp;
        isDead = false;
    }
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        int finalDamage = Mathf.Max(damage, 0);
        currentHp = Mathf.Max(currentHp - finalDamage, 0);
        // TODO: 히트 이펙트 있으면 여기서 재생        

        onEnemyTakeDamageEvent?.Invoke(currentHp, maxHp);

        if (currentHp <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        if (isDead) return;

        isDead = true;

        if (anim != null)
            anim.SetTrigger("isDead");

        onEnemyDeadEvent?.Invoke(enemyType, transform.position, dropExp);

        StartCoroutine(DeadTime());
        kill.AddKillCount(1);
    }
    IEnumerator DeadTime()
    {
        yield return new WaitForSeconds(0.6f);
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("콜리이더가 잘 작동 합니다");
        if (isDead) return;

        //TODO: 레이어 정의하면 수정

        int layer = collision.gameObject.layer;
        if (layer == itemLayer || layer == itemLayers)
        {

            //TODO: 투사체나 무기 데미지 불러오기 임시로 20데미지
            Debug.Log("데미지가 들어갔냐?");
            TakeDamage(20);
        }
    }   
}