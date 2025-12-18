using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapUpDownScrolling : MonoBehaviour
{
    [SerializeField] Transform cam;
    
    [SerializeField] int tilemapCount = 3;
    
    private float tilemapHeight;
    private float loopLength;
    private float halfLength;

    void Start()
    {
        if(cam == null)
        {
            cam = Camera.main.transform;
        }        

        TilemapRenderer tilemapRenderer = GetComponent<TilemapRenderer>();
        if(tilemapRenderer != null)
        {
            tilemapHeight = tilemapRenderer.bounds.size.y;
            loopLength = tilemapHeight * tilemapCount;
            halfLength = loopLength * 0.5f;
        }
    }
    private void LateUpdate()
    {
        float distance = cam.position.y - transform.position.y;

        if(distance > halfLength)
        {
            transform.position += Vector3.up * loopLength;
        }
        else if(distance < -halfLength)
        {
            transform.position += Vector3.down * loopLength;
        }
    }   
}