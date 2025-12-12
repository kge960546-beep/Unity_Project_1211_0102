using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldMeat : MonoBehaviour
{
    [SerializeField] private int healAmount = 500;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //플레이어의 최대체력 회복 또는 체력회복
            UseMeat();
            Destroy(gameObject);
        }
    }   
    void UseMeat()
    {        
        //if (GameManager.instance == null) return;
        //
        //int maxHp = GameManager.instance.playerMaxHp;
        //int currentHp = GameManager.instance.playerCurrentHp;
        //
        //if (currentHp >= maxHp) return;
        //
        //GameManager.instance.currentHp = Mathf.Min(currentHp + healAmount, maxHp);
    }
}
