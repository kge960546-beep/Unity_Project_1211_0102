using UnityEngine;

[CreateAssetMenu(fileName = "BossPatternSO", menuName = "Game/Stage/SpawnPattern/BossSpawnPattern")]
public class BossPatternSO : SpawnPatternSO
{
    public int bossID;

    [Header("Boss Options")]
    public bool clearOtherEnemies = true;
    public bool isLockedArena = true;
    public bool isShowWaring = true;

    public Vector3 spawnPoint;
}