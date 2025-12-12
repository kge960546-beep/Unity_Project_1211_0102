using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDead : MonoBehaviour
{
    public static EnemyDead instance;    

    [Header("Prefab")]
    [SerializeField] private GameObject expPrefab;
    [SerializeField] private GameObject skillRandomBoxPrefab;
    [SerializeField] private GameObject[] consumablePrefabs;

    [Header("UI")]
    [SerializeField] private GameObject clearPanel;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    /// <summary>
    /// 사망할때 어떤 아이템을 남기면서 사망할지 판단해주는 기능을 등록, 해지 하는 기능
    /// </summary>
    /// <param name="enemyHp"></param>
    public void RegisterEnemy(EnemyHp enemyHp)
    {
        if (enemyHp == null) return;
        //중복 등록 방지
        enemyHp.UnsubscribeEnemyDeadEvent(SpawnDropOnEnemyDead);
        enemyHp.SubscribeEnemyDeadEvent(SpawnDropOnEnemyDead);
    }
    public void UnregisterEnemy(EnemyHp enemyHp)
    {
        if(enemyHp == null) return;

        enemyHp.UnsubscribeEnemyDeadEvent(SpawnDropOnEnemyDead);
    }   
    private void SpawnDropOnEnemyDead(EnemyType type, Vector3 deadPos, int expDropAmount)
    {
        switch(type)
        {
            case EnemyType.Boss:
                //TODO: 시간대 별로 보스 드랍 On, Off 가능하게 변경 필요                        
                if (consumablePrefabs != null)
                {
                    foreach (GameObject item in consumablePrefabs)
                    {
                        //소비아이템 배열에 있는 프리팹들을(item) 각각 1개씩(1) 어디에 뿌릴것인지(deadPos) 
                        SpawnItemPos(item, deadPos, 1);
                    }
                }
                SpawnItemPos(skillRandomBoxPrefab, deadPos, 1);

                //TODO: 마지막 보스는 드랍x 바로 클리어 패널로 
                if (clearPanel != null) { clearPanel.SetActive(true); }
                break;
            case EnemyType.Elite:
                SpawnItemPos(skillRandomBoxPrefab, deadPos, 1);
                break;            
        }       

        SpawnItemPos(expPrefab, deadPos, expDropAmount);
    }   
    private void SpawnItemPos(GameObject prefab, Vector3 centerPos, int count)
    {
        if(prefab == null) return;

        for (int i = 0; i < count; i++)
        {
            Vector2 offset = UnityEngine.Random.insideUnitCircle * 1.0f;

            Vector3 spawnPos = centerPos + (Vector3)offset;

            Instantiate(prefab, spawnPos, Quaternion.identity);
        }
    }
}