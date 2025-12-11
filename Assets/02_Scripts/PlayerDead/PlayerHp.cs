using System;
using UnityEngine;

public class PlayerHp : MonoBehaviour
{
    public static event Action onPlayerDeadEvent;
    public event Action<int, int> OnTakeDamage;

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

        OnTakeDamage?.Invoke(finalDamage, currentHp);

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
}