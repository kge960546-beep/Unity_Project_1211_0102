using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldRandomBox : MonoBehaviour
{
    [SerializeField] GameObject[] randomItemPrefabs;
    [SerializeField] private int health = 1;
    [Range(0, 1)] public float drop = 0.5f; //아이템이 안나올확률 / 나올확률

    [SerializeField] private Transform player;
    [SerializeField] private float destroyDistance = 30f;

    private void Update()
    {
        if(!player) return;
        //플레이어와 너무 멀어지면 파괴
        float dist = Vector2.Distance(player.position, transform.position);
        if(dist > destroyDistance)
        {
            Destroy(gameObject);
        }
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            DestroyBox();
        }
    }
    void DestroyBox()
    {
        if(Random.value <= drop)
        {
            int index = Random.Range(0, randomItemPrefabs.Length);
            Instantiate(randomItemPrefabs[index], transform.position, Quaternion.identity);           
        }
        Destroy(gameObject);
    }   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Weapon"))
        {
            TakeDamage(1);
        }
    }
}
