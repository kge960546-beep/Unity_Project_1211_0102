using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Runtime.CompilerServices;
using System.ComponentModel.Design.Serialization;

//작동 테스트용 임시 오브젝트 풀링 큐(추후 수정하거나 삭제해도 됨)
public class EnemyPool : MonoBehaviour
{
    public static EnemyPool instance;

    [SerializeField] private List<EnemyData> enemyDataList;

    //public GameObject[] enemyPrefab;
    //public Dictionary<int,EnemyData> enemyID = new Dictionary<int,EnemyData>();
    public Dictionary<int,Queue<GameObject>> pools = new Dictionary<int,Queue<GameObject>>();

    //public Queue<GameObject> enemyPool = new Queue<GameObject>();
    public int poolCountPrefab = 250;

    void Awake()
    {
        if (instance == null) instance = this;
    }
    void Start()
    {
        //BuildEnemyDictionary();

        Initialize();
    }
    public void Initialize()
    {
        //초기화
        pools.Clear();

        foreach(var data in enemyDataList)
        {
            int id = data.enemyID;

            // Queue 생성
            if(!pools.ContainsKey(id))
            {
                pools[id] = new Queue<GameObject>();
            }    

            // pool 생성
            for(int i = 0; i<data.poolCount; i++)
            {
                GameObject enemy = Instantiate(data.enemyPrefab, transform);
                enemy.SetActive(false);
                pools[id].Enqueue(enemy);
            }
        }
    }
    //private void BuildEnemyDictionary()
    //{
    //    enemyID.Clear();
    //
    //    foreach(var data in enemyDataList)
    //    {
    //        if(enemyID.ContainsKey(data.enemyID))
    //        {
    //            Debug.LogError($"중복 enemyID : {data.enemyID}");
    //            continue;
    //        }
    //        enemyID.Add(data.enemyID, data);
    //    }
    //}
    //private void Initialize()
    //{
    //    foreach(var pair in enemyID)
    //    {
    //        int id = pair.Key;
    //        EnemyData data = pair.Value;
    //
    //        Queue<GameObject> que = new Queue<GameObject>();
    //
    //        for(int i = 0; i< data.poolCount; i++)
    //        {
    //            GameObject obj = Instantiate(data.enemyPrefab, transform);
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
    public void InsertQueue(int enemyID, GameObject enemy)
    {
        if(!pools.ContainsKey(enemyID))
            pools.Add(enemyID, new Queue<GameObject>());

        pools[enemyID].Enqueue(enemy);
        enemy.SetActive(false);
    }
    public GameObject GetQueue(int enemyID)
    {
        if(!pools.ContainsKey(enemyID))
        {
            Debug.LogError($"EnemyPool에 {enemyID} 가 존재하지 않음");
            return null;
        }
        Queue<GameObject> queue = pools[enemyID];

        if(queue.Count > 0)
        {
            return queue.Dequeue();
        }
        else
        {
            EnemyData data = GetMonsterData(enemyID);
            GameObject newEnemy = Instantiate(data.enemyPrefab);

            return newEnemy;
        }
    }
    private EnemyData GetMonsterData(int enemyID)
    {
        foreach(var data in enemyDataList)
        {
            if(data.enemyID == enemyID)
            {
                return data;
            }
        }
        Debug.LogError($"{enemyID}에 해당하는 MonsterData가 없습니다");
        return null;
    }
}
