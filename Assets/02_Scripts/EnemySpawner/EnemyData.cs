using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Game/Enemy/EnemyData SO")]
public class EnemyData : ScriptableObject
{
    public GameObject enemyPrefab;
    public string name;
    public int enemyID;
    public int poolCount;
    public int hp;
}
    
