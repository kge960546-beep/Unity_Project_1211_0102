using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLockup : MonoBehaviour
{    
    public Transform target;

    [SerializeField] float cameraSpeed = 2.0f;

    [SerializeField] float cameraZ;
    [SerializeField] float cameraX;
    private void Awake()
    {       
        cameraX = transform.position.x;
        cameraZ = transform.position.z;
    }
    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPos = new Vector3(cameraX, target.position.y, cameraZ);
        transform.position = Vector3.Lerp(transform.position, targetPos, cameraSpeed * Time.deltaTime);
    }   
}
