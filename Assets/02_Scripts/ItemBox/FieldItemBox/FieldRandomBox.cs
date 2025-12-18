using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FieldRandomBox : MonoBehaviour
{
    [SerializeField] GameObject[] randomItemPrefabs;
    [SerializeField] private int health = 1;
    [Range(0, 1)] public float drop = 0.5f; //아이템이 안나올확률 / 나올확률

    [SerializeField] private Transform player;
    [SerializeField] private float destroyDistance = 20f;

    [Header("Tag")]
    [SerializeField] private string playerTag = "PlayerProjectile";
    private int itemLayer;
    //두사체나 무기의 태그가 있으면 수정
    //[SerializeField] private string weaponTag = "WeaponTag";

    //스폰 포인트가 비어있는지 아닌지를 판별
    FieldSpawn fieldSpawn;
    private Transform mySpawnPoint;

    private BoxCollider2D boxCol;
    private void Awake()
    {
        if (boxCol != null) return;

        if (boxCol == null)
        {
            boxCol = gameObject.AddComponent<BoxCollider2D>();
        }
        boxCol.isTrigger = true;
        itemLayer = LayerMask.NameToLayer("PlayerProjectile");
    }

    private void Start()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag(playerTag);
            if (p != null) player = p.transform;
        }
    }
    private void Update()
    {
        if (player == null) return;
        //플레이어와 너무 멀어지면 파괴
        float dist = Vector2.Distance(player.position, transform.position);

        if(dist > destroyDistance)
        {            
            Destroy(gameObject);
        }
    }
    public void SetSpawnPointInfo(FieldSpawn spawnner, Transform point)
    {
        fieldSpawn = spawnner;
        mySpawnPoint = point;
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            DestroyBox();
        }
    }
    void DestroyBox()
    {
        if(randomItemPrefabs != null && randomItemPrefabs.Length > 0)
        {
            if (Random.value <= drop)
            {
                int index = Random.Range(0, randomItemPrefabs.Length);
                Instantiate(randomItemPrefabs[index], transform.position, Quaternion.identity);
            }
        }
        Destroy(gameObject);
    }   
    private void OnTriggerEnter2D(Collider2D collision)
    {

        int layer = collision.gameObject.layer;      
        
        if (layer == itemLayer)
        {
            //TODO: 투사체나 무기 데미지 불러오기 임시로 20데미지
            Debug.Log("데미지가 들어갔냐?");
            TakeDamage(1);
        }
    }
    private void OnDestroy()
    {
        if (fieldSpawn != null && mySpawnPoint != null)
        {
            fieldSpawn.ReleaseSpawnPoint(mySpawnPoint);
        }
    }
}
