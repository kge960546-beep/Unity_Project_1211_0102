using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Monster/ Monster Data")]
public class MonsterDataSO : ScriptableObject
{
    [Header("Stats")]
    public int maxHp = 100; // TODO:임시 지정용 몬스터 체력 / 나중에 외부 코드랑 병합할때 삭제해야 함.
    public int attackDamage = 10; // 몬스터가 플레이어와 닿았을시 얼마나 데미지를 줄 것인지에 대해 임시 지정
    public float moveSpeed = 3.0f;
}
