using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("EnemyDataList")]
    public List<BossData> bossDataList;
    [SerializeField] private List<bossData> enemyDataList;
    //[SerializeField] private CameraController cameraController;

    [Header("Spawn Setting")]
    [SerializeField] private Transform spawnPos;
    [SerializeField] private Rigidbody2D playerRb; //ToDo: 플레이어 좌표를 따주는 코드가 있으면 바꿔줄 것 
    [SerializeField] private GameObject barricadePrefab;
    [SerializeField] private SpawnPatternController patternController;

    [Header("UI Setting")]
    [SerializeField] private WaveWarningUI bossWarningUI;
    [SerializeField] private BossHPUI bossHPUI;
    [SerializeField] private BossHpSlider slider;

    private Dictionary<int, GameObject> enemyDataPrefab;
    //실행 중인 패턴 코루틴 관리
    private List<Coroutine> runningPatterns = new();
    private List<GameObject> barricades = new();

    private BossData currentBoss;

    PoolingService ps;
    TimeService ts;

    void Awake()
    {
        ps = GameManager.Instance.GetService<PoolingService>();
        ts = GameManager.Instance.GetService<TimeService>();
        enemyDataPrefab = new();
        
        //if(cameraController == null )
        //    cameraController = FindObjectOfType<CameraController>();

        if (patternController == null)
            patternController = FindObjectOfType<SpawnPatternController>();

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
    // 일반 & 엘리트 스폰
    public GameObject SpawnEnemy(bossData data, Vector3 position)
    {
        GameObject enemy = EnemyPool.instance.Get(data);
       
        enemy.transform.position = position;
        enemy.transform.rotation = Quaternion.identity;
        enemy.SetActive(true);

        return enemy;
    }
    // 보스 스폰
    public IEnumerator SpawnBossRoutine(BossPatternSO bossPattern, BossData bossData)
    {
        Debug.Log("[BossSpawn] SpawnBossRoutine 실행");

        bossData.currentHp = bossData.maxHp;
#if UNITY_EDITOR
        Debug.Log($"[BossSpawn] Set HP = {bossData.currentHp}");
#endif
        SetCurrentBoss(bossData);

        bossWarningUI.ShowBossWarning();

        ts.PauseStageTimer();

        //cameraController.ZoomOutForBoss();

        if (bossPattern.isLockedArena)
            ActivateBarricade(spawnPos);

        ClearAllEnemies();

        yield return new WaitForSeconds(3f);

        if (bossHPUI != null)
            bossHPUI.Bind(bossData);

        if (slider != null)
            slider.BindingBoss();

        GameObject boss = SpawnEnemy(bossData, bossPattern.spawnPoint);

        while (bossData.currentHp > 0)
            yield return null;

        //cameraController.ResetZoom();

        ts.ResumeStageTimer();

        DeactivateBarricade();
    }
    private void SpawnBarricade(GameObject prefab, Vector3 worldPos)
    {
        var inst = Instantiate(prefab, worldPos, Quaternion.identity);
        barricades.Add(inst);
    }
    private void ActivateBarricade(Transform centerTransform)
    {
        Debug.Log(currentBoss == null ? "bossDataList is NULL" : "bossDataList OK");
        Debug.Log(currentBoss?.barricadePattern == null ? "pattern is NULL" : "pattern OK");

        var pattern = currentBoss.barricadePattern;

        if(pattern == null)
        {
            Debug.LogWarning("BarricadePattern 지정되지 않음");
            return;
        }

        //기존 바리게이드 제거
        DeactivateBarricade();

        Vector3 center = centerTransform.position;

            switch (pattern.patternType)
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
    // 모든 적 제거
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
    // 스폰 패턴 강제 정지
    public void StopAllSpawnPatterns()
    {
        foreach(var coroutine in runningPatterns)
        {
            if(coroutine != null)
                StopCoroutine(coroutine);
        }

        runningPatterns.Clear();
    }
    public void SetCurrentBoss(BossData boss)
    {
        currentBoss = boss;
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
}

