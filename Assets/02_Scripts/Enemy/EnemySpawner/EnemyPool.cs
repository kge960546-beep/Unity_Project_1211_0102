using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Runtime.CompilerServices;
using System.ComponentModel.Design.Serialization;
using System.Xml.Schema;

//작동 테스트용 임시 오브젝트 풀링 큐(추후 수정하거나 삭제해도 됨)
public class EnemyPool : MonoBehaviour
{
    public static EnemyPool instance;

    [SerializeField] private List<EnemyData> enemyDataList;

    public Dictionary<EnemyData, GameObject> prefabMap;

    PoolingService ps;

    void Awake()
    {
        if (instance == null) instance = this;
        
        ps = GameManager.Instance.GetService<PoolingService>();

        if (ps == null)
        {
            Debug.LogError("[EnemyPool] PoolingService 못 찾음");
        }
        else
        {
            Debug.Log("[EnemyPool] PoolingService 연결 성공");
        }

        prefabMap = new();

        foreach(var data in enemyDataList)
        {
            Debug.Log($"[EnemyPool] 등록 시도 -> {data?.name}, ep:{data?.enemyPrefab}");

            if (data == null || data.enemyPrefab == null)
            {
                Debug.LogError($"EnemyData 설정 오류 : {data}");
                continue;
            }

            prefabMap[data] = data.enemyPrefab;
        }
        Debug.Log($"[EnemyPool] enemyPrefab 등록 개수 : {prefabMap.Count}");
    }
    
    public GameObject Get(EnemyData data)
    {
        Debug.Log($"[EnemyPool] Get 요청 : {data?.name}");

        if(!prefabMap.TryGetValue(data, out var prefab))
        {
            Debug.LogError($"EnemyData 매핑 없음 : {data}");
            return null;
        }
        Debug.Log($"[EnemyPool] prefab 확인 : {prefab.name}");

        var ep = ps.GetOrCreateInactivatedGameObject(prefab);

        Debug.Log($"[EnemyPool] 풀에서 반환된 객체 : {ep.name}");

        return ep;
    }
    public void Return(EnemyData data, GameObject enemy)
    {
        ps.ReturnOrDestroyGameObject(enemy);
    }
    //private void BuildEnemyDictionary()
    //{
    //    enemyID.Clear();
    //
    //    foreach(var stagePattern in enemyDataList)
    //    {
    //        if(enemyID.ContainsKey(stagePattern.enemyID))
    //        {
    //            Debug.LogError($"중복 enemyID : {stagePattern.enemyID}");
    //            continue;
    //        }
    //        enemyID.Add(stagePattern.enemyID, stagePattern);
    //    }
    //}
    //private void Initialize()
    //{
    //    foreach(var pair in enemyID)
    //    {
    //        int id = pair.Key;
    //        EnemyData stagePattern = pair.Value;
    //
    //        Queue<GameObject> que = new Queue<GameObject>();
    //
    //        for(int i = 0; i< stagePattern.poolCount; i++)
    //        {
    //            GameObject obj = Instantiate(stagePattern.enemyPrefab, transform);
    //            var pool = obj.GetComponent<PoolableEnemy>();
    //            if (pool == null) pool = obj.AddComponent<PoolableEnemy>();
    //            pool.enemyID = id;
    //            
    //            obj.SetActive(false);
    //            que.Enqueue(obj);
    //        }
    //        pools[id] = que;
    //    }
    //}
    #region oldVersion
    //public void InsertQueue(int enemyID, GameObject enemy)
    //{
    //    if(!pools.ContainsKey(enemyID))
    //        pools.Add(enemyID, new Queue<GameObject>());

    //    pools[enemyID].Enqueue(enemy);
    //    enemy.SetActive(false);
    //}
    //public GameObject GetQueue(int enemyID)
    //{
    //    if(!pools.ContainsKey(enemyID))
    //    {
    //        Debug.LogError($"EnemyPool에 {enemyID} 가 존재하지 않음");
    //        return null;
    //    }
    //    Queue<GameObject> queue = pools[enemyID];

    //    if(queue.Count > 0)
    //    {
    //        return queue.Dequeue();
    //    }
    //    else
    //    {
    //        EnemyData stagePattern = GetMonsterData(enemyID);
    //        GameObject newEnemy = Instantiate(stagePattern.enemyPrefab);

    //        return newEnemy;
    //    }
    //}
    //private EnemyData GetMonsterData(int enemyID)
    //{
    //    foreach(var stagePattern in enemyDataList)
    //    {
    //        if(stagePattern.enemyID == enemyID)
    //        {
    //            return stagePattern;
    //        }
    //    }
    //    Debug.LogError($"{enemyID}에 해당하는 MonsterData가 없습니다");
    //    return null;
    //}
    #endregion
}
