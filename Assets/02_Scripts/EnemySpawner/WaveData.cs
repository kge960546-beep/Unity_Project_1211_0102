using UnityEngine;

public class WaveData : MonoBehaviour
{
    public WaveInfo[] waveInfo;
    public int waveCount
    {
        get { return waveInfo.Length; }
    }
}

[System.Serializable]
public struct WaveInfo
{
    public float startTime;         // 스폰이 시작하는 시간
    public float endTime;           // 스폰이 끝나는 시간
    public float tick;              // 스폰 주기
    public int maxEnemySpawnLimit;  // 동시에 스폰해 있을 수 있는 최대 수 
    public int enemyID;             // 적 ID
    public int spawnPointID;        // 스폰위치 ID
}
