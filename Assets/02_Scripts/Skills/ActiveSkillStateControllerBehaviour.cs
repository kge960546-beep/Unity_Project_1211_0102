using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkillStateControllerBehaviour : MonoBehaviour
{
    [SerializeField] private ActiveSkillStateContext context;
    public ActiveSkillStateContext Context => context;

    private void OnEnable()
    {
        context.currentState = ActiveSkillStateUtility.GetStateType<ActiveSkillWaitingState>();
        context.currentState.Initialize(ref context);
    }

    private void FixedUpdate()
    {
        context.currentState.Tick(ref context);
    }
}
