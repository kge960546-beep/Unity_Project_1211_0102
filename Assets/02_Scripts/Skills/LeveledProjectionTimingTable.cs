using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct LeveledProjectionTimingTable
{
    [field: SerializeField] public int RequiredLevel { get; private set; }
    [field: SerializeField] public List<float> ProjectionTimingTable { get; private set; }
}
