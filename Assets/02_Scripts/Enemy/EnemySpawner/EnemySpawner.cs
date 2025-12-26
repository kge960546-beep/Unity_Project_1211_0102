using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<EnemyData> enemyDataList; 

    [SerializeField] private Transform spawnPos;
    //ToDo: 플레이어 좌표를 따주는 코드가 있으면 바꿔줄것 
    [SerializeField] private Rigidbody2D playerRb;

    private Dictionary<int, GameObject> enemyDataPrefab;
    //실행 중인 패턴 코루틴 관리
    private List<Coroutine> runningPatterns = new();

    PoolingService ps;

    void Awake()
    {
        ps = GameManager.Instance.GetService<PoolingService>();
        enemyDataPrefab = new();

        if (spawnPos == null)
            spawnPos = GameObject.FindWithTag("Player").transform;

        foreach (var data in enemyDataList)
        {
            if (enemyDataPrefab.ContainsKey(data.enemyID))
            {
                Debug.LogError($"중복된 enemyID : {data.enemyID}");
                continue;
            }

            enemyDataPrefab.Add(data.enemyID, data.enemyPrefab);
        }
        foreach(var data in enemyDataPrefab)
        {
            Debug.Log($"등록된 Enemy : ID = {data.Key}, Prefab ={data.Value}");
        }
    }
    private void Start()
    {
#if UNITY_EDITOR
        if (ps == null)
            Debug.LogError("EnemySpawner : PoolingService 미할당");
        if (spawnPos == null)
            Debug.LogError("EnemySpawner : SpawnPos 미할당");
#endif
    }
    // Normal / Elite Spawn
    public GameObject SpawnEnemy(EnemyData data, Vector3 position)
    {
        GameObject enemy = EnemyPool.instance.Get(data);
       
        enemy.transform.position = position;
        enemy.transform.rotation = Quaternion.identity;
        enemy.SetActive(true);

        return enemy;
    }
    // Boss Spawn
    public void SpawnBoss(BossPatternSO bossPattern)
    {
        if(bossPattern.clearOtherEnemies)
        {
            ClearAllEnemies();
        }

        if(bossPattern.isLockedArena)
        {
            //테두리 잠금
        }

        if(!enemyDataPrefab.TryGetValue(bossPattern.bossID,out GameObject prefab))
        {
            Debug.LogError($"bossID {bossPattern.bossID} 프리팹 없음");
            return;
        }
    }
    //풀 되돌리기
    public void ReturnEnemy(GameObject obj)
    {
        ps.ReturnOrDestroyGameObject(obj);
    }
    //모든 적 제거
    private void ClearAllEnemies()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach(var e in enemies)
        {
            ps.ReturnOrDestroyGameObject(e);
        }
    }
    #region Test
    //void Awake()
    //{
    //    //spawnPoint = GetComponentsInChildren<Transform>();
    //    waveCount = currentWave.waveCount;
    //}
    //private void Start()
    //{
    //    //StartCoroutine(SpawnWave());
    //}
    //void FixedUpdate()
    //{
    //    float playTime = TS.accumulatedFixedDeltaTime;
    //    
    //
    //    Debug.Log(playTime);
    //
    //    foreach (var wave in currentWave.waveInfo)
    //    {
    //        if (IsSpawning(playTime, wave))
    //        {
    //            StartCoroutine(SpawnWave(wave));
    //        }
    //    }
    //   
    //}
    //IEnumerator SpawnWave(WaveInfo wave)
    //{
    //
    //    for (int i = 0; i < wave.spawnCount; i++)
    //    {
    //        GameObject enemy = enemyPool.GetQueue(wave.enemyID);
    //        //에러 발생 지점 : 스폰포인트가 제대로 연결 안됨
    //        //enemy.transform.position = spawnPoint[Random.Range(0, spawnPoint.Length)].position;
    //        enemy.transform.position = spawnPos.GetSpawnPoint();
    //        enemy.SetActive(true);
    //        yield return new WaitForSeconds(wave.tick);
    //    }
    //}
    //private bool IsSpawning(float playTime, WaveInfo wave)
    //{
    //    return playTime >= wave.startTime && playTime < wave.endTime;
    //}
    #endregion
    #region OldSpawner
    //private void FixedUpdate()
    //{
    //    playTime = spawnTimer.playTime;        
    //}
    //public void StartSpawn()
    //{
    //    GameObject selectPrefab = enemyPrefabs[selection];
    //    GameObject enemy = Instantiate(selectPrefab);
    //
    //    if (currentWave.Length <= 0)
    //    {
    //        StartCoroutine(SpawnEnemy(0));
    //    }
    //}
    //IEnumerator SpawnEnemy(int enemyArr)
    //{
    //    while(IsSpawnable())
    //    {
    //        int enemyIndex = currentWave[enemyArr].enemyID;
    //        int spawnIndex = currentWave[enemyArr].spawnPointID;
    //
    //        GameObject clone = Instantiate(enemyPrefabs[enemyIndex], spawnPoints[spawnIndex]);
    //        MonsterData monster = clone.GetComponent<MonsterData>();
    //    }
    //    yield return new WaitForSeconds(0.5f); 
    //}
    //public bool IsSpawnable()
    //{
    //    if(playTime <= 300f || 
    //        (300f< playTime && playTime <= 600f) || 
    //        (600f < playTime && playTime <= 900f))
    //    {
    //        return true;
    //        }
    //    else
    //        return false;
    //}
    #endregion
}

