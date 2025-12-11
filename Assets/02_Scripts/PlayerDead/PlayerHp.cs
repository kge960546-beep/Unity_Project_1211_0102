using System;
using UnityEngine;

public class PlayerHp : MonoBehaviour
{
    /// <summary>
    /// onPlayerDeadEvent: 플레이어가 죽었을 때 발생하는 이벤트
    /// </summary>
    private static event Action onPlayerDeadEvent;
    /// <summary>
    /// onTakeDamage: 데미지를 입었을 때 발생하는 이벤트
    /// int: 입은 데미지 양 (finalDamage)
    /// int: 남은 체력 (currentHp)
    /// </summary>
    private event Action<int, int> onTakeDamageEvent;

    [SerializeField] private int maxHp = 100;
    private int currentHp;
    private bool isDead = false;

    private Animator anim;
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
    public void SubscriptionPlayerTakeDamageEvent(Action<int, int> action)
    {
        onTakeDamageEvent += action;
    }
    public void UnsubscriptionPlayerTakeDamageEvent(Action<int, int> action)
    {
        onTakeDamageEvent -= action;
    }
    #endregion
    private void Awake()
    {
        currentHp = maxHp;

        anim = GetComponent<Animator>();        
    }
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        int finalDamage = Mathf.Max(damage, 0);

        currentHp = Mathf.Max(currentHp - finalDamage, 0);
        // TODO: 히트 이펙트 있으면 여기서 재생        

        onTakeDamageEvent?.Invoke(finalDamage, currentHp);

        if (currentHp <= 0) { Die(); }
    }
    private void Die()
    {
        if(isDead) return;

        isDead = true;
        if(anim != null)
            anim.SetTrigger("isDead");

        onPlayerDeadEvent?.Invoke();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(isDead) return;
        LayerMask.GetMask("Enemy");
        if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // TODO: 데미지 적 정보로 불러오기 임시로 20 데미지
            TakeDamage(20);
        }
    }    
}