using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public sealed class ActiveSkillWaitingState : ActiveSkillStateBase
{
    public override void Tick(ref ActiveSkillStateContext context)
    {
        if (context.timer > context.period
            && null != Physics2D.OverlapCircle(
                context.player.transform.position,
                context.logic.DefaultSearchRadius,
                1 << GameManager.Instance.GetService<LayerService>().enemyLayer))
        {
            OnExitState(ref context);
            context.currentState = ActiveSkillStateUtility.GetStateType<ActiveSkillProjectingState>();
            context.currentState.OnEnterState(ref context);
        }
        else
        {
            OnStayState(ref context);
        }
    }

    public override void OnEnterState(ref ActiveSkillStateContext context)
    {
        context.timer = 0;
    }

    public override void OnStayState(ref ActiveSkillStateContext context)
    {
        context.timer += Time.fixedDeltaTime;
    }

    public override void OnExitState(ref ActiveSkillStateContext context)
    {
    }
}
