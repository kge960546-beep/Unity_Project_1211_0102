using UnityEngine;

[CreateAssetMenu(menuName = "Game/Stage/SpawnPattern/CircleWallGap SO")]
public class CicleWallGapShapeSO1 : SpawnPatternSO
{
    public override ISpawnShape CreateShape()
    {
        return new CicleWallGapShape();
    }
}
