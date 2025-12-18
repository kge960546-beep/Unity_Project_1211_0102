using UnityEngine;

// 스폰 패턴 묶음
[CreateAssetMenu(menuName = "Game/Stage/SpawnPattern/StagePattern SO")]
public class StagePatternSO : ScriptableObject
{
    public SpawnPatternSO[] patterns;
}
