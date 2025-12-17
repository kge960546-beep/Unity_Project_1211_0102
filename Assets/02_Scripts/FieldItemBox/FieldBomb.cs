using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldBomb : MonoBehaviour
{
    [Header("setting")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private int bombDamage = 1000;
    [SerializeField] private float bombDamageRange = 10.0f;

    private BoxCollider2D box;
    private void Awake()
    {        
        box = GetComponent<BoxCollider2D>();
        if (box != null) return;

        if(box == null)
        {
            box = gameObject.AddComponent<BoxCollider2D>();            
        }
        box.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(playerTag)) return;

        BombDamage();
        gameObject.SetActive(false);
    }
    private void BombDamage()
    {
        if (box == null) return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, bombDamageRange, enemyLayer);

        foreach(var hit in hits)
        {
            var enemy = hit.GetComponent<EnemyHp>();
            if (enemy == null) continue;
           
            enemy.TakeDamage(bombDamage);//ÆøÅº µ¥¹ÌÁö
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, bombDamageRange);
    }
}
