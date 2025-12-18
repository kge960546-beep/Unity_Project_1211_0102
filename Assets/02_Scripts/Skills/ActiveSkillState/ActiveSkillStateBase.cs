using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class ActiveSkillStateUtility
{
    private static readonly Dictionary<Type, ActiveSkillStateBase> states =
        Assembly.GetExecutingAssembly().GetTypes()
            .Where(
                t =>
                    t.IsSubclassOf(typeof(ActiveSkillStateBase))
                    && t.IsSealed)
            .ToDictionary(
                k => k,
                v => (ActiveSkillStateBase)Activator.CreateInstance(v));

    public static T GetStateType<T>() where T : ActiveSkillStateBase
    {
        return (T)states[typeof(T)];
    }
}

public abstract class ActiveSkillStateBase
{
    public void Initialize(ref ActiveSkillStateContext context) { OnEnterState(ref context); }
    public abstract void Tick(ref ActiveSkillStateContext context);
    public abstract void OnEnterState(ref ActiveSkillStateContext context);
    public abstract void OnStayState(ref ActiveSkillStateContext context);
    public abstract void OnExitState(ref ActiveSkillStateContext context);
}
