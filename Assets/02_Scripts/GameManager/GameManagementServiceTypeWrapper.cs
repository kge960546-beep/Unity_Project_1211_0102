using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameManagementServiceWrapper
{
    public string name;
    public Type ResolvedType => string.IsNullOrEmpty(name) ? null : Type.GetType(name);
}
