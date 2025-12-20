using UnityEngine;

[CreateAssetMenu(fileName = "BossTimerData", menuName = "Game/Timer")]
public class BossTimerSO : ScriptableObject
{
    public float stageMaxTime; //스테이지 종료시간
    public float stageFirstBossTime;
    public GameObject firstBossPrefab;
    public GameObject lastBossPrefab;
}