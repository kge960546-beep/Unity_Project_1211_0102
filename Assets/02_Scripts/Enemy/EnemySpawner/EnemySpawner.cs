using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public BossData testBossData;

    [SerializeField] private List<EnemyData> enemyDataList; 

    [SerializeField] private Transform spawnPos;
    //ToDo: 플레이어 좌표를 따주는 코드가 있으면 바꿔줄것 
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private BossWarningUI countdownUI;
    [SerializeField] private GameObject barricadePrefab;

    private Dictionary<int, GameObject> enemyDataPrefab;
    //실행 중인 패턴 코루틴 관리
    private List<Coroutine> runningPatterns = new();
    private List<GameObject> barricades = new();

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
    //void Update()
    //{
        
    //    // 테스트용: B 키를 누르면 강제 생성
    //    if (Input.GetKeyDown(KeyCode.B))
    //    {
    //        Debug.Log("[TEST] B Key 입력됨");
    //        // 여기 BossData를 직접 Drag&Drop 하거나, 참조 받아야 함
    //        ForceSpawnBarricade(testBossData);
    //    }
    //}
    //// 강제 바리게이트 스폰 테스트
    //public void ForceSpawnBarricade(BossData bossData)
    //{
    //    Debug.Log("[TEST] ForceSpawnBarricade 실행");

    //    if (bossData == null)
    //    {
    //        Debug.LogError("BossData가 null");
    //        return;
    //    }

    //    if (bossData.barricadePattern == null)
    //    {
    //        Debug.LogError("BarricadePattern이 null");
    //        return;
    //    }

    //    // 기본 중심은 BossData에서 지정한 center
    //    Vector3 center = bossData.barricadeCenter != null
    //        ? bossData.barricadeCenter.position
    //        : Vector3.zero;

    //    Debug.Log($"Barricade Center = {center}");

    //    ActivateBarricade(bossData);
    //}
    // Normal / Elite Spawn
    public GameObject SpawnEnemy(EnemyData data, Vector3 position)
    {
        GameObject enemy = EnemyPool.instance.Get(data);
       
        enemy.transform.position = position;
        enemy.transform.rotation = Quaternion.identity;
        enemy.SetActive(true);

        return enemy;
    }
    //public IEnumerator PrepareBossSpawn(BossPatternSO bossPattern, BossData bossData)
    //{
    //    Debug.Log("[BossSpawn] PrepareBossSpawn 시작");

    //    Vector3[] positions = bossData.spawnShape.GetSpawnPositions(bossData.context);
    //    Vector3 bossPos = positions[0];


    //}
    // Boss Spawn
    public IEnumerator SpawnBossRoutine(BossPatternSO bossPattern, BossData bossData)
    {

        bossData.currentHp = bossData.maxHp;

        if (bossPattern.isLockedArena)
            ActivateBarricade(bossData);

        GameObject boss = SpawnEnemy(bossData, bossPattern.spawnPoint);

        while (bossData.currentHp > 0)
            yield return null;

        DeactivateBarricade();
    }
    private void SpawnBarricade(GameObject prefab, Vector3 worldPos)
    {
        var inst = Instantiate(prefab, worldPos, Quaternion.identity);
        barricades.Add(inst);
    }
    private void ActivateBarricade(BossData bossData)
    {
        Debug.Log("[BossSpawn] ActivateBarricade 호출");

        var pattern = bossData.barricadePattern;

        if(pattern == null)
        {
            Debug.LogWarning("BarricadePattern 지정되지 않음");
            return;
        }

        //기존 바리게이드 제거
        DeactivateBarricade();

        Vector3 center = bossData.barricadeCenter.position;

        switch(pattern.patternType)
        {
            case BarricadePattern.Circle:
                SpawnCirclePattern(pattern, center); 
                break;

            case BarricadePattern.Square:
                SpawnSquarePattern(pattern, center);
                break;

            case BarricadePattern.HorizontalLIne:
                SpawnHorizontalLine(pattern, center);
                break;

            case BarricadePattern.VerticalLIne:
                SpawnVerticalLinePattern(pattern, center);
                break;
        }
    }
    private void DeactivateBarricade()
    {
        foreach(var bar in barricades)
        {
            if(bar != null)
            {
                Destroy(bar);
            }
        }
        barricades.Clear();
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

        foreach (var e in enemies)
        {
            if (e.name == "Barricade")
                continue;


            ps.ReturnOrDestroyGameObject(e);
        }
    }
    // 패턴 강제 정지
    public void StopAllSpawnPatterns()
    {
        foreach(var coroutine in runningPatterns)
        {
            if(coroutine != null)
                StopCoroutine(coroutine);
        }

        runningPatterns.Clear();
    }
    #region BarricadeSpawn
    private void SpawnCirclePattern(BarricadePatternSO pattern, Vector3 center)
    {
        for(int i = 0; i< pattern.circleCount; i++)
        {
            float angle = Mathf.PI * 2f * (i/(float)pattern.circleCount);

            Vector3 offset = new(Mathf.Cos(angle) * pattern.circleRadius, Mathf.Sin(angle) * pattern.circleRadius, 0f);

            SpawnBarricade(pattern.barricadePrefab, center +  offset);
        }
    }
    private void SpawnSquarePattern(BarricadePatternSO pattern, Vector3 center)
    {
        int countX = Mathf.RoundToInt(pattern.squaureWidth / pattern.spacing);
        int countY = Mathf.RoundToInt(pattern.squaereHeight / pattern.spacing);

        for (int x = -countX; x <= countX; x++)
        {
            // 상단
            SpawnBarricade(pattern.barricadePrefab, center + new Vector3(x * pattern.spacing, pattern.squaereHeight * 0.5f, 0f));
            // 하단
            SpawnBarricade(pattern.barricadePrefab, center + new Vector3(x * pattern.spacing, -pattern.squaereHeight * 0.5f, 0f));
        }

        for (int y = -countY; y <= countY; y++)
        {
            // 상단
            SpawnBarricade(pattern.barricadePrefab, center + new Vector3(-pattern.squaureWidth * 0.5f, y * pattern.spacing, 0f));
            // 하단
            SpawnBarricade(pattern.barricadePrefab, center + new Vector3(pattern.squaureWidth * 0.5f, y * pattern.spacing, 0f));
        }
    }
    private void SpawnHorizontalLine(BarricadePatternSO pattern, Vector3 center)
    {
        for (int i = 0; i < pattern.lineCount; i++)
        {
            float t = (i - pattern.lineCount / 2f) * pattern.spacing;
            Vector3 pos = center + new Vector3(t, 0, 0);

            SpawnBarricade(pattern.barricadePrefab, pos);
        }
    }
    private void SpawnVerticalLinePattern(BarricadePatternSO pattern, Vector3 center)
    {
        for (int i = 0; i < pattern.lineCount; i++)
        {
            float t = (i - pattern.lineCount / 2f) * pattern.spacing;
            Vector3 pos = center + new Vector3(0, t, 0);

            SpawnBarricade(pattern.barricadePrefab, pos);
        }
    }
    #endregion
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

