using UnityEngine;

[CreateAssetMenu(menuName = "Game/Stage/SpawnPattern/SwarmShape SO")]
public class SwarmShapeSO : SpawnPatternSO
{
    public override ISpawnShape CreateShape()
    {
        return new SwarmShape();
    }
}
