using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FieldSpawn : MonoBehaviour
{    
    //[SerializeField] private Transform spawnRoot; //부모오브젝트
    public Transform[] spawnPoints; //spawnRoot안 스폰포인트들
    [SerializeField] private Transform itemBoxprefab; //랜덤박스 스크립트가 있는 프리펩

    private HashSet<Transform> spawnPointCompleteHashSet = new HashSet<Transform>(); //중복 스폰 방지    
   
    public float spawnTime = 15.0f;    
   
    private void Start()
    {
        StartCoroutine(spawn());
    }
    IEnumerator spawn()
    {
        while (true) 
        {
            yield return new WaitForSeconds(spawnTime);
            SpawnBox();
        }
    }
    public void ReleaseSpawnPoint(Transform point)
    {
        if (point == null) return;

        spawnPointCompleteHashSet.Remove(point);
    }
    void SpawnBox()
    {
        if (spawnPoints == null || spawnPoints.Length == 0) return;
        if (spawnPointCompleteHashSet.Count >= spawnPoints.Length) return;

        //비어있는 스폰 포인트만 리스트 등록
        List<Transform> spawnList = new List<Transform>();
        foreach (Transform spPoint in spawnPoints)
        {
            if (spPoint == null) continue;

            if(!spawnPointCompleteHashSet.Contains(spPoint))
            {
                spawnList.Add(spPoint);
            }
        }
        if (spawnList.Count == 0) return;

        int index = Random.Range(0, spawnList.Count);        
        Transform point = spawnList[index];
        
        spawnPointCompleteHashSet.Add(point);
        
        Transform box = Instantiate(itemBoxprefab, point.position, Quaternion.identity);

        FieldRandomBox FieldBox = box.GetComponent<FieldRandomBox>();        
        if(FieldBox != null)
        {
            FieldBox.SetSpawnPointInfo(this, point);
        }
    }
}
