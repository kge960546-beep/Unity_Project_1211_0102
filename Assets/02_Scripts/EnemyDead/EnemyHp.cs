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
public class EnemyHp : MonoBehaviour
{
    public static event Action<EnemyType, Vector3, int> onEnemyDeadEvent; //적이 죽었을때 적 타입과 위치를 전달

    [Header("Enemy Setting")]
    [SerializeField] private EnemyType enemyType = EnemyType.Normal; //적 타입
    [SerializeField] private int maxHp = 100;
    [SerializeField] private int dropExp = 1;
    
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

        currentHp = Mathf.Max(currentHp - damage, 0);
        //히트 이펙트 있으면 여기서 재생             

        if (currentHp <= 0) 
        {           
            Die(); 
        }
    }
    private void Die()
    {
        if (isDead) return;

        isDead = true;

        if (anim != null)
            anim.SetTrigger("isDead");

        onEnemyDeadEvent?.Invoke(enemyType, transform.position, dropExp);

        StartCoroutine(DeadTime());
    }
    IEnumerator DeadTime()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
