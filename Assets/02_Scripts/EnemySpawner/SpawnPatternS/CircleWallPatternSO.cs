using UnityEngine;

[CreateAssetMenu(menuName = "Game/Stage/SpawnPattern/CicleWall SO")]
public class CircleWallPatternSO : SpawnPatternSO
{
    public override ISpawnShape CreateShape()
    {
        return new CircleWallShape();
    }
}
