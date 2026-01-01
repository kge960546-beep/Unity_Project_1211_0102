using UnityEngine;

public class StageManager : MonoBehaviour
{
    public EnemySpawner spawner;

    public int spawnBossCount = 0;
    public bool isBossPhase = false;

    public void OnBossSpawn()
    {
        isBossPhase = true;
        spawner.StopAllCoroutines();
    }
}
