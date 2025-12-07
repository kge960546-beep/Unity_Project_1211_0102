using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldBomb : MonoBehaviour
{        
    [SerializeField] private string playerTag = "Player";

    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private float bombDamage = 1000f;

    private BoxCollider2D box;
    private void Awake()
    {
        box = GetComponent<BoxCollider2D>();
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
        
        Vector2 center = box.bounds.center;
        Vector2 size = box.bounds.size;

        Collider2D[] hits = Physics2D.OverlapBoxAll(center, size, 0f, enemyLayer);

        //foreach(var hit in hits)
        //{
        //    var enemy = hit.GetComponent<EnemyHp>();
        //    if (enemy == null) continue;
        //
        //    enemy.TakeDamage(bombDamage); //ÆøÅº µ¥¹ÌÁö
        //}


    }
}
