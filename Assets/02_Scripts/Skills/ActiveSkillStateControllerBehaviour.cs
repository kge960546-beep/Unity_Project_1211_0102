using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkillStateControllerBehaviour : MonoBehaviour
{
    [SerializeField] private ActiveSkillStateContext context;
    public ActiveSkillStateContext Context => context;

    private void OnEnable()
    {
        // TODO: init context dynamically with skill input

        context.nextState = ActiveSkillStateUtility.GetStateType<ActiveSkillWaitingState>();
        context.isStateChanged = true;
    }

    private void FixedUpdate()
    {
        ActiveSkillStateBase currentState = context.nextState;

        if (context.isStateChanged)
        {
            currentState.OnEnterState(ref context);
            context.isStateChanged = false;
        }

        currentState.Tick(ref context);
        context.timer += Time.fixedDeltaTime;

        if (currentState != context.nextState)
        {
            context.isStateChanged = true;
            currentState.OnExitState(ref context);
        }
    }
}
