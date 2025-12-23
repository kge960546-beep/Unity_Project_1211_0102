using System;
using System.Collections;
using UnityEngine;

public class PlayerHp : MonoBehaviour
{
    /// <summary>
    /// onPlayerDeadEvent: 플레이어가 죽었을 때 발생하는 이벤트
    /// </summary>
    private event Action onPlayerDeadEvent;
    /// <summary>
    /// onTakeDamage: 데미지를 입었을 때 발생하는 이벤트
    /// int: 현재 체력 (currentHp)
    /// int: 최대 체력 (maxHp)
    /// </summary>
    private event Action<int, int> onTakeDamageEvent;

    [SerializeField] private int maxHp;
    [SerializeField] private int currentHp;
    private bool isDead = false;

    private Animator anim;
    private Rigidbody2D rb;
    public PlayerDataSO playerBaseData;
    #region DeathSubscriptionAndCancellation
    /// <summary>
    /// 데드 이벤트 구독 & 해지
    /// </summary>
    /// <param name="action"></param>
    public void SubscribePlayerDeadEvent(Action action)
    {
        onPlayerDeadEvent += action;
    }
    public void UnsubscribePlayerDeadEvent(Action action)
    {
        onPlayerDeadEvent -= action;
    }
    #endregion
    #region TakeDamageSubscriptionAndCancellation
    /// <summary>
    /// 피격 이벤트 구독 & 해지
    /// </summary>
    /// <param name="action"></param>
    public void SubscribePlayerTakeDamageEvent(Action<int, int> action)
    {
        onTakeDamageEvent += action;
    }
    public void UnsubscribePlayerTakeDamageEvent(Action<int, int> action)
    {
        onTakeDamageEvent -= action;
    }
    #endregion
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        if(playerBaseData == null)
        {
            Debug.Log("참조할 데이터가 없습니다");
            return;
        }
        maxHp = playerBaseData.playerMaxHp;
        currentHp = maxHp;
    }
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        int finalDamage = Mathf.Max(damage, 0);

        currentHp = Mathf.Max(currentHp - finalDamage, 0);
        // TODO: 히트 이펙트 있으면 여기서 재생

        onTakeDamageEvent?.Invoke(currentHp, maxHp);

        if (currentHp <= 0) { Die(); }
    }
    public void Heal()
    {       
        int healAmount = Mathf.RoundToInt(maxHp * 0.3f);
        currentHp += healAmount;

        if(currentHp > maxHp)
        {
            currentHp = maxHp;
        }
    }
    private void Die()
    {
        if(isDead) return;

        isDead = true;
        if(anim != null)
            anim.SetTrigger("isDead");

        StartCoroutine(PlayerDie());
        rb.bodyType = RigidbodyType2D.Static;
        //TODO: 플레이어 컨트롤러에서 PlayerHp를 불러와 구독 해지 추가
    }
    IEnumerator PlayerDie()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        onPlayerDeadEvent?.Invoke();
    }   
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // TODO: 데미지 적 정보로 불러오기 임시로 20 데미지
            TakeDamage(20);
        }
    }
}