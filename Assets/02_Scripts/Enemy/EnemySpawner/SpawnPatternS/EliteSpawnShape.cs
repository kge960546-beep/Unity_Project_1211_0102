using UnityEngine;

[CreateAssetMenu(fileName = "EliteSpawnShape", menuName = "Game/Stage/SpawnShape/EliteSpawnShape")]
public class EliteSpawnShape : SpawnShapeSO
{
    public override Vector3[]GetSpawnPositions(SpawnContext context)
    {
        return new Vector3[]
        {
            context.playerPosition
        };
    }
}
