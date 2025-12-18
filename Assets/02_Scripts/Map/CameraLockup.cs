using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLockup : MonoBehaviour
{    
    public Transform target;    

    [SerializeField] float cameraZ;
    [SerializeField] float cameraX;
    private void Awake()
    {       
        cameraX = transform.position.x;
        cameraZ = transform.position.z;
    }
    private void Update()
    {
        if (target == null) return;

        Vector3 targetPos = new Vector3(cameraX, target.position.y, cameraZ);
        transform.position = targetPos;
    }   
}
