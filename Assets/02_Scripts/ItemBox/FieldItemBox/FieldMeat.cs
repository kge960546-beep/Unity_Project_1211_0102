using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldMeat : MonoBehaviour
{
    [SerializeField] private int healAmount = 500;

    private string playerTag = "Player";

    private CircleCollider2D circleCol;

    private PlayerDataSO playerData;
    private void Awake()
    {
        circleCol = GetComponent<CircleCollider2D>();

        if (circleCol != null) return;

        if (circleCol == null)
        {
            circleCol = gameObject.AddComponent<CircleCollider2D>();
        }
        circleCol.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(playerTag)) return;

        UseMeat();
        Destroy(gameObject);

    }   
    void UseMeat()
    {
        //TODO: 플레이어 체력 데이터 가져와서 회복시키기
        playerData.playerMaxHp = healAmount;
        Debug.Log($"체력이 {healAmount} 만큼 회복했습니다");
    }
}
