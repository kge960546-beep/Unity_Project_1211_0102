using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDead : MonoBehaviour
{
    public static EnemyDead instance;

    [System.Serializable]
    public struct DropItems
    {
        public string name;
        public GameObject prefab;
        [Range(1, 100)] public int weight;
    }
    [System.Serializable]
    public struct DataByStage
    {
        public string dateName;
        public List<DropItems> dropItemsTable;
    }

    [Header("경험치")]
    [SerializeField] private List<DataByStage> expPrefabs;
    private int currentExp = 0;

    [Header("Prefab")]    
    [SerializeField] private GameObject skillRandomBoxPrefab;
    [SerializeField] private GameObject[] consumablePrefabs;
    [SerializeField] private GameObject equipmentBoxPRefab;

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
    #region EnemyEvent
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
    #endregion
    /// <summary>
    /// 적이 사망했을때 어떤 아이템을 타입별로 뿌릴지 정하는 메서드
    /// </summary>
    /// <param name="type"></param>
    /// <param name="deadPos"></param>
    /// <param name="expDropAmount"></param>
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

                if(equipmentBoxPRefab != null)
                    SpawnItemPos(equipmentBoxPRefab, deadPos, 2);

                if(currentExp < expPrefabs.Count - 1)
                {
                    currentExp++;
                }

                //TODO: 마지막 보스는 드랍x 바로 클리어 패널로
                if (clearPanel != null) { clearPanel.SetActive(true); }
                break;
            case EnemyType.Elite:
                SpawnItemPos(skillRandomBoxPrefab, deadPos, 1);
                break;            
        }

        SpawnExpByPhase(deadPos, expDropAmount);
    }
    /// <summary>
    /// 페이즈별로 경험치종류를 가중치 확률로 뿌리는 메서드 
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="amount"></param>
    private void SpawnExpByPhase(Vector3 pos, int amount)
    {
        if (expPrefabs == null || expPrefabs.Count == 0) return;

        int index = Mathf.Clamp(currentExp, 0, expPrefabs.Count - 1);

        DataByStage currentData = expPrefabs[index];
        List<DropItems> currentExpTable = currentData.dropItemsTable;

        if (currentExpTable == null || currentExpTable.Count == 0) return;
        GameObject selectedExpPrefabs = GetRandomExp(currentExpTable);

        if(selectedExpPrefabs != null)
        {
            SpawnItemPos(selectedExpPrefabs, pos, amount);
        }
    }
    /// <summary>
    /// 가중치 확률의 기능 메서드
    /// </summary>
    /// <param name="table"></param>
    /// <returns></returns>
    private GameObject GetRandomExp(List<DropItems> table)
    {
        int totalWeight = 0;

        foreach (var item in table)
        {
            totalWeight += item.weight;
        }
        int randomValue = UnityEngine.Random.Range(0, totalWeight);

        foreach(var item in table)
        {
            if(randomValue < item.weight)
            {
                return item.prefab;
            }
            randomValue -= item.weight;
        }
        return table[0].prefab;
    }
    /// <summary>
    /// 타입별로 아이템을 생성하는 기능 메서드
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="centerPos"></param>
    /// <param name="count"></param>
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