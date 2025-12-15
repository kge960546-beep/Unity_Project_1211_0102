using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private WaveData currentWave;
    [SerializeField] private EnemyPool enemyPool;

    //스폰 위치
    public Transform[] spawnPoint;
    private SpawnPoint spawnPos;

    private float timer;
    //public int selection;
    private int waveCount;

    [SerializeField] private SpawnTimer spawnTimer;

    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        waveCount = currentWave.waveCount;
    }
    private void Start()
    {
        StartCoroutine(SpawnWave());
    }
    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        spawnTimer.playTime += Time.fixedDeltaTime;

        //Debug.Log(spawnTimer.playTime);

        foreach(var wave in currentWave.waveInfo)
        {
            if(spawnTimer.playTime >= wave.startTime && spawnTimer.playTime <= wave.endTime)
            {
                if(timer >= wave.tick)
                {
                    timer = 0;

                    //StartCoroutine(SpawnWave());
                }
            }
        }
    }
    IEnumerator SpawnWave()
    {
        foreach(var wave in currentWave.waveInfo)
        {
            for(int i = 0; i< wave.maxEnemySpawnLimit; i++)
            {
                GameObject enemy = enemyPool.GetQueue(wave.enemyID);
                enemy.transform.position = spawnPoint[Random.Range(0, spawnPoint.Length)].position;
                enemy.SetActive(true);
                yield return new WaitForSeconds(wave.tick);
            }
        }
    }
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

