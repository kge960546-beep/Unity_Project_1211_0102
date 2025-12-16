using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Stage/SpawnPattern/SpiralPattern SO")]
public class SpiralPatternSO : SpawnPatternSO
{
    public override ISpawnShape CreateShape()
    {
        return new SpiralShape();
    }
}
