using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampMap : MonoBehaviour
{    
    [SerializeField] private Transform target;   

    [SerializeField] private Vector3 adjustCamPos;

    [SerializeField] private Vector2 minCamLimit;
    [SerializeField] private Vector2 maxCamLimit;    
    void LateUpdate()
    {
        if (target == null) return;
        Vector3 pos = target.position;

        transform.position = new Vector3(Mathf.Clamp(pos.x, minCamLimit.x, maxCamLimit.x) + adjustCamPos.x,
                                         Mathf.Clamp(pos.y, minCamLimit.y, maxCamLimit.y) + adjustCamPos.y, -10f + adjustCamPos.z);
    }
}
