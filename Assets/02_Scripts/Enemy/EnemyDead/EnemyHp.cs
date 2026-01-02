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
    /// 피격 이벤트:  현재 체력이 얼마나 남았는지 currentHp, 최대체력이 얼만지 maxHp
    /// </summary>
    private event Action<int, int> onEnemyTakeDamageEvent;

    [Header("Enemy Setting")]
    [SerializeField] private EnemyType enemyType = EnemyType.Normal; //적 타입
    [SerializeField] private int maxHp;
    [SerializeField] private int dropExp = 1;    

    //임시 레이어 지정
    [Header("Layer")]
    [SerializeField] int playerLayer;   

    private int currentHp;
    private bool isDead = false;

    private Animator anim;
    public bossData enemyData;
    private MonsterController mController;
    private KillCount kill;
    private PlayerHp playerHp;
    private void Awake()
    {        
        anim = GetComponent<Animator>();
        mController = GetComponent<MonsterController>();

        kill = FindAnyObjectByType<KillCount>();
        
        if(playerHp == null)
            playerHp = FindAnyObjectByType<PlayerHp>();

        playerLayer = LayerMask.NameToLayer("Player");
        
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

        gameObject.layer = GameManager.Instance.GetService<LayerService>().enemyLayer;

        if (EnemyDead.instance != null)
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
    public void Init(bossData data, EnemyType type, int exp)
    {
        enemyData = data;
        enemyType = type;
        dropExp = exp;        

        maxHp = enemyData.maxHp;
        currentHp = maxHp;
        isDead = false;
    }
    public void TakeDamage(int damage, GameObject source, bool isCritical)
    {
        if (isDead) return;

        int finalDamage = Mathf.Max(damage, 0);
        currentHp = Mathf.Max(currentHp - finalDamage, 0);
        // TODO: 히트 이펙트 있으면 여기서 재생
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
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
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
        if (anim != null)
            anim.SetTrigger("isDead");        

        onEnemyDeadEvent?.Invoke(enemyType, transform.position, dropExp);

        gameObject.layer = GameManager.Instance.GetService<LayerService>().defaultLayer;
        StartCoroutine(DeadTime());
        kill.AddKillCount(1);
    }
    IEnumerator DeadTime()
    {
        yield return new WaitForSeconds(0.6f);
        gameObject.SetActive(false);
    }
}