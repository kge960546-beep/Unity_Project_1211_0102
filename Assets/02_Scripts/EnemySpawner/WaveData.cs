using UnityEngine;

//안쓰는 코드
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
    public int enemyID;             // 적 ID
}
