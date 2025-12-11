using System;
using UnityEngine;

public class PlayerHp : MonoBehaviour
{
    /// <summary>
    /// onPlayerDeadEvent: 플레이어가 죽었을 때 발생하는 이벤트
    /// </summary>
    public static event Action onPlayerDeadEvent;
    /// <summary>
    /// onTakeDamage: 데미지를 입었을 때 발생하는 이벤트
    /// int: 입은 데미지 양 (finalDamage)
    /// int: 남은 체력 (currentHp)
    /// </summary>
    public event Action<int, int> onTakeDamage;

    [SerializeField] private int maxHp = 100;
    private int currentHp;
    private bool isDead = false;

    private Animator anim;

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
        //히트 이펙트 있으면 여기서 재생        

        onTakeDamage?.Invoke(finalDamage, currentHp);

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
    }

    //event를 외부에 직접 public으로 노출하는 건 외부에서 대입을 해버릴 수 있기에 위험함.
    //Subscribe ~/Unsubscribe ~메소드를 통하여 간접적으로 등록/해제를 관리하는 것이 권장됨.
}