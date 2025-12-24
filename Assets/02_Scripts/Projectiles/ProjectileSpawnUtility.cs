using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProjectileSpawnUtility
{
    public static void Spawn(GameObject tempSharedCommonProjectilePrefab, ProjectileLogicBase logic, ProjectileInstanceInitializationData initData)
    {
        GameObject obj = GameManager.Instance.GetService<PoolingService>().GetOrCreateInactivatedGameObject(tempSharedCommonProjectilePrefab);
        ProjectileLogicRunner plr = obj.GetComponent<ProjectileLogicRunner>();
        plr.Logic = logic;
        plr.InitData = initData;
        obj.SetActive(true);
    }
}
