using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ActiveSkillStateContext
{
    public ActiveSkillStateBase currentState;
    public ProjectileLogicBase logic;
    public LeveledProjectionTimingTable[] leveledTimingTables;
    public GameObject player;
    public GameObject tempSharedCommonProjectilePrefab;
    public float searchRadius;
    public float period;
    public int level;
    [HideInInspector] public float timer;
}
