using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDead : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject expPrefab;
    [SerializeField] private GameObject skillRandomBoxPrefab;
    [SerializeField] private GameObject[] expendablesPrefabs;

    [Header("UI")]
    [SerializeField] private GameObject clearPanel;    

    private void OnEnable()
    {      
        EnemyHp.onEnemyDeadEvent += OnEnemy;
    }
    private void OnDisable()
    {
        EnemyHp.onEnemyDeadEvent -= OnEnemy;
    }
    private void OnEnemy(EnemyType type, Vector3 deadPos, int expDropAmnount)
    {
        if(type == EnemyType.Boss)
        {
            //시간대 별로 보스 드랍 On, Off 가능하게 변경 필요
            //마지막 보스는 드랍x
            if (clearPanel != null) { clearPanel.SetActive(true); }

            if(expendablesPrefabs != null)
            {
                foreach (GameObject item in expendablesPrefabs)
                {
                    SpawnItem(item, deadPos, 1);
                }
            }
            
            SpawnItem(skillRandomBoxPrefab, deadPos, 1);
        }

        if(type == EnemyType.Elite)
        {            
            SpawnItem(skillRandomBoxPrefab, deadPos, 1);
        }

        SpawnItem(expPrefab, deadPos, expDropAmnount);
    }   
    private void SpawnItem(GameObject prefab, Vector3 centerPos, int count)
    {
        if(prefab == null) return;

        for (int i = 0; i < count; i++)
        {
            Vector2 offset = UnityEngine.Random.insideUnitCircle * 0.5f;

            Vector3 spawnPos = centerPos + (Vector3)offset;

            Instantiate(prefab, centerPos + (Vector3)offset, Quaternion.identity);
        }
    }

}
