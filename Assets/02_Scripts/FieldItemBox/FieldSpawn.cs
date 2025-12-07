using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FieldSpawn : MonoBehaviour
{
    [SerializeField] private Transform spawnRoot; //부모오브젝트
    public Transform[] spawnPoints; //spawnRoot안 스폰포인트들
    [SerializeField] private Transform itemBoxprefab; //랜덤박스 스크립트가 있는 프리펩
    
   
    public float spawnTime = 15.0f;
    
    private void Awake()
    {
        if(spawnRoot != null)
        {
            spawnPoints = spawnRoot.GetComponentsInChildren<Transform>();
        }           
    }
    private void Start()
    {
        StartCoroutine(spawn());
    }

    IEnumerator spawn()
    {
        while (spawnTime > 0) 
        {
            yield return new WaitForSeconds(spawnTime);
            SpawnBox();
        }
    }
    void SpawnBox()
    {
        if (spawnPoints == null) return;

        int index = Random.Range(1, spawnPoints.Length);
        Transform point = spawnPoints[index];
        Instantiate(itemBoxprefab, point.position, Quaternion.identity);
    }
}
