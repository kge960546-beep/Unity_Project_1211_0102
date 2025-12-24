using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _0003_Bat_ProjectileLogic : ProjectileLogicBase
{
    // TODO not implemented yet
    protected override bool IsTargetInRangeInternal(Vector2 projectorPosition, float projectorAzimuth)
    {
        return true;
    }

    protected override void CallbackAtOnEnableInternal(ref ProjectileInstanceContext instanceData, ProjectileInstanceInitializationData initData) { }
    protected override void CallbackAtOnDisableInternal(ref ProjectileInstanceContext instanceData) { }
    protected override void CallbackAtFixedUpdateInternal(ref ProjectileInstanceContext instanceData) { }
    protected override void CallbackAtOnTriggerEnter2DInternal(ref ProjectileInstanceContext instanceData, Collider2D collider) { }
    protected override void CallbackAtOnTriggerStay2DInternal(ref ProjectileInstanceContext instanceData, Collider2D collider) { }
    // TODO use LineRenderer with widthCurve after implementing OnDisable callback
}