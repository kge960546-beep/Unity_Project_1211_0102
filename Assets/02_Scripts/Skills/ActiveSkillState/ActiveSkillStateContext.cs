using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ActiveSkillStateContext
{
    public ActiveSkillStateBase nextState;
    public bool isStateChanged;
    public ProjectileLogicBase logic;
    public LeveledProjectionTimingTable[] leveledTimingTables;
    public Rigidbody2D skillUserRB;
    public GameObject tempSharedCommonProjectilePrefab;
    public ProjectileInstanceInitializationData cachedInitData;
    public float period;
    public int level;
    public int layer;
    [HideInInspector] public float timer;
}
