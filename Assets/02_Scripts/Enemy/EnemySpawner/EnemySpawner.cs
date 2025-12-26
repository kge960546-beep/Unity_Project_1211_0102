using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyPool enemyPool;
    [SerializeField] private Transform spawnPos;
    [SerializeField] private Rigidbody2D playerRb;
  
    private List<Coroutine> runningPatterns = new();

    TimeService ts;
    TimeService TS => ts ??= GameManager.Instance?.GetService<TimeService>();

    void Awake()
    {
        if (spawnPos == null)
            spawnPos = GameObject.FindWithTag("Player").transform;
    }
    private void Start()
    {
#if UNITY_EDITOR
        if (enemyPool == null)
            Debug.LogError("EnemySpawner : EnemyPool 미할당");
        if (spawnPos == null)
            Debug.LogError("EnemySpawner : SpawnPos 미할당");
#endif
    }
    public Coroutine RunPattern(SpawnPatternSO pattern)
    {
        var co = StartCoroutine(RunPatternRoutine(pattern));
        runningPatterns.Add(co);
        return co;
    }
    //패턴 실행
    IEnumerator RunPatternRoutine(SpawnPatternSO pattern)
    {
        float startTime = TS.accumulatedFixedDeltaTime;

        while (TS.accumulatedFixedDeltaTime < pattern.endTime)
        {
            if (TS.accumulatedFixedDeltaTime < pattern.startTime)
            {
                yield return null;
                continue;
            }
            float patternTime = TS.accumulatedFixedDeltaTime - startTime;

            SpawnContext context = new SpawnContext
            {
                playerPosition = spawnPos.transform.position,
                playerMoveDir = playerRb != null ? playerRb.velocity : Vector3.zero,
                spawnCount = pattern.spawnCount,
                radius = pattern.radius,
                patternTime = patternTime
            };

            Vector3[] positions = pattern.shape.GetSpawnPositions(context);

            Spawn(pattern, positions , context);

            yield return new WaitForSeconds(pattern.tick);
        }
    }
    //적 스폰 
    void Spawn(SpawnPatternSO pattern, Vector3[] positions, SpawnContext context)
    {
        foreach(var pos in positions)
        {
            GameObject enemy = enemyPool.GetQueue(pattern.enemyData);
            if(enemy == null) continue;

            enemy.transform.position = pos;
            enemy.SetActive(true);
        }
    }
    //모든 생성 패턴 정지
    public void StopAllPatterns()
    {
        foreach(var co in runningPatterns)
        {
            if(co != null)
                StopCoroutine(co);
        }
        runningPatterns.Clear();
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

