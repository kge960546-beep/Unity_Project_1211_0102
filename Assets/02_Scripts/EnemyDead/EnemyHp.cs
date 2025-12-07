using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHp : MonoBehaviour
{
    [SerializeField] private int maxHp = 100;
    private int currentHp;
    private Animator anim;
    private EnemyDeadSubject deadSubject;
    private bool isDead = false;
    private void Awake()
    {
        currentHp = maxHp;
        anim = GetComponent<Animator>();
        deadSubject = GetComponent<EnemyDeadSubject>();
    }
    public void TakeDamage(int damage)
    {
        if (isDead) return;
        currentHp -= damage;
        if (currentHp <= 0)
        {
            currentHp = 0;
            Die();
        }
    }
    private void Die()
    {
        if(isDead) return;
        isDead = true;
        anim.SetTrigger("Dead");
        if(deadSubject != null)
            deadSubject.NotifyEnemyDead();
    }
}
