using System;
using System.Collections;
using UnityEngine;

public class PlayerHp : MonoBehaviour, IDamageable
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

    [SerializeField] private GameObject bloodSplashVFX;

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

        if (playerBaseData == null)
        {
            Debug.Log("참조할 데이터가 없습니다");
            return;
        }
        maxHp = playerBaseData.playerMaxHp;
        currentHp = maxHp;

        onTakeDamageEvent?.Invoke(currentHp, maxHp);
    } 

    /// <summary>
    /// 현재체력을 최대체력이 변했을시 반영시키는 메서드
    /// </summary>
    /// <param name="newMaxHp"></param>
    /// <param name="isMaintainProportion"></param>
    /// <param name="isnotify"></param>
    public void ReflectMaxHp(int newMaxHp, bool isMaintainProportion = true, bool isnotify = true)
    {
        newMaxHp = Mathf.Max(1, newMaxHp);

        if(isMaintainProportion && maxHp > 0)
        {
            float ratio = (float)currentHp / maxHp;
            maxHp = newMaxHp;
            currentHp = Mathf.Clamp(Mathf.RoundToInt(maxHp * ratio), 1, maxHp);
        }
        else
        {
            maxHp = newMaxHp;
            currentHp = Mathf.Min(currentHp, maxHp);            
        }
        if(isnotify)
        {
            onTakeDamageEvent?.Invoke(currentHp, maxHp);
        }
    }
    // 실제 적용되는 데미지
    public void TakeDamage(int damage, GameObject source, bool isCritical)
    {
        LayerService ls = GameManager.Instance.GetService<LayerService>();

        if (isDead) return;
        if (source.layer != ls.enemyLayer && source.layer != ls.enemyProjectileLayer) return;
        int finalDamage = Mathf.Max(damage, 0);

        currentHp = Mathf.Max(currentHp - finalDamage, 0);
        
        if (!bloodSplashVFX.activeSelf) bloodSplashVFX.SetActive(true);

        onTakeDamageEvent?.Invoke(currentHp, maxHp);

        if (currentHp <= 0) { Die(); }
    }
    public void Heal()
    {       
        int healAmount = Mathf.RoundToInt(maxHp * 0.3f);
        onTakeDamageEvent?.Invoke(currentHp, maxHp);

        currentHp += healAmount;

        if (currentHp > maxHp)
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
}