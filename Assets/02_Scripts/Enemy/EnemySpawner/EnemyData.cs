using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Game/Enemy/EnemyData SO")]
public class EnemyData : ScriptableObject
{
    public GameObject enemyPrefab;
    public string enemyName;
    public int poolCount;

    public int maxHp;
    public int attack;
    public float speed;
}
    
