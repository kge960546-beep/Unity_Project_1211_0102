//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class BossSpawnController : MonoBehaviour
//{
//    public EnemySpawner enemySpawner;

//    public GameObject barricadePrefab;
//    private GameObject barricadeInstance;

//    public BossWarningUI countdownUI;

//    [Header("Setting")]
//    public float preSpawnDelay = 3.0f;
//    public BossPatternSO bossPattern;

//    private void Awake()
//    {
//        if (countdownUI == null)
//            countdownUI = FindObjectOfType<BossWarningUI>(true);

//        if (enemySpawner == null)
//            enemySpawner = FindObjectOfType<EnemySpawner>();
//    }
//    public IEnumerator SpawnBossWithBarricade(bossDataList bossDataList)
//    {
//        Debug.Log("[BossSpawn] SpawnBossWithBarricade 호출");

//        Vector3[] positions = bossDataList.spawnShape.GetSpawnPositions(bossDataList.context);

//        yield return StartCoroutine(countdownUI.ShowBossWarningRoutine(3));
    
//        ActivateBarricade(bossDataList.barricadeCenter);

//        enemySpawner.SpawnBoss(bossPattern);

//        yield return StartCoroutine(WaitBossDead(bossDataList));

//        DeactivateBarricade();
//    }
//    private void ActivateBarricade(Transform center)
//    {
//        Debug.Log("[BossSpawn] ActivateBarricade 호출");
//        if (barricadeInstance == null)
//        {
//            Debug.LogError("[BossSpawn] 바리게이트가 프리팹 인스턴스 생성");
//            barricadeInstance = Instantiate(barricadePrefab);
//        }

//        barricadeInstance.transform.position = center.position;
//        barricadeInstance.SetActive(true);

//        Debug.Log($"[BossSpawn] 바리게이트 위치 설정 : {center.position}");
//    }
//    private void DeactivateBarricade()
//    {
//        if (barricadeInstance != null)
//            barricadeInstance.SetActive(false);
//    }
//    private IEnumerator WaitBossDead(bossDataList bossDataList)
//    {
//        int health = bossDataList.currentHp;

//        while(bossDataList != null && health > 0)
//            yield return null;
//    }
//}
