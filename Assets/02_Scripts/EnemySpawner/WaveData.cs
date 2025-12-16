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
    public int spawnCount;          // 동시에 스폰 수 (수정필요)
    public float radius;            // 스폰 범위
    public int enemyID;             // 적 ID
    public SpawnPatternSO shape;    // 스폰 패턴
}
