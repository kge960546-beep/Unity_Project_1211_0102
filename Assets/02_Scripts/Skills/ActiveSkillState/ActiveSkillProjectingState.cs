using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class ActiveSkillProjectingState : ActiveSkillStateBase
{
    public override void Tick(ref ActiveSkillStateContext context)
    {
        float prevTimer = context.timer;
        context.timer += Time.fixedDeltaTime;
        float currentTimer = context.timer;

        int currentLevel = context.level;

        List<float> targetTable =
            context.leveledTimingTables
                .Where(t => t.RequiredLevel <= currentLevel)
                .Aggregate((tLeft, tRight) => tLeft.RequiredLevel < tRight.RequiredLevel ? tLeft : tRight)
                .ProjectionTimingTable;

        int count = targetTable.Count;
        if (0 == count || targetTable[count - 1] + Time.fixedDeltaTime < currentTimer)
        {
            OnExitState(ref context);
            context.currentState = ActiveSkillStateUtility.GetStateType<ActiveSkillWaitingState>();
            context.currentState.OnEnterState(ref context);
        }
        else
        {
            ProjectileInstanceInitializationData initData = new ProjectileInstanceInitializationData();
            initData.projectorPosition = context.player.transform.position;
            //initData.targetingDirection = ????
            initData.level = context.level;
            initData.layer = GameManager.Instance.GetService<LayerService>().playerProjectileLayer;

            for (int i = 0; i < targetTable.Count; i++)
            {
                float t = targetTable[i];
                if (t < prevTimer || currentTimer < t) continue;
                initData.sequenceNumber = i;
                ProjectileSpawnUtility.Spawn(context.tempSharedCommonProjectilePrefab, context.logic, initData);
            }
            // OnStayState(ref context);
        }
    }

    public override void OnEnterState(ref ActiveSkillStateContext context)
    {
        context.timer = 0;
    }

    public override void OnStayState(ref ActiveSkillStateContext context)
    {
        // refactor this structure
    }

    public override void OnExitState(ref ActiveSkillStateContext context)
    {
    }
}
