using UnityEngine;

[CreateAssetMenu(fileName = "bossData", menuName = "Game/Enemy/bossData SO")]
public class bossData : ScriptableObject
{
    public EnemyType enemyType;
    public GameObject enemyPrefab;
    public int enemyID;
    public int poolCount;

    public int maxHp;
    public int currentHp;
    public int attack;
    public float speed;
}
    
