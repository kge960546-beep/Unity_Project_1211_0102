using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpObj : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10.0f;
    [SerializeField] private Transform target;
    [SerializeField] private int expAmount = 10;
    private bool isMagnetOn = false;    

    private CircleCollider2D circleCollider;
    [SerializeField] private ExperienceService es;

    private void Awake()
    {
        es = GameManager.Instance.GetService<ExperienceService>();

        circleCollider = GetComponent<CircleCollider2D>();

        if (circleCollider != null) return;

        if(circleCollider == null)
        {
            circleCollider.gameObject.AddComponent<CircleCollider2D>();
        }
        circleCollider.isTrigger = true;
        if(es == null)
        {
            Debug.Log("아 없다고");
            return;
        }        
    }
    private void OnEnable()
    {       
        FieldMagnet.onMagnet += StartMagnet;
    }
    private void OnDisable()
    {
        FieldMagnet.onMagnet -= StartMagnet;
    }
    public void StartMagnet(Transform player)
    {
        if (isMagnetOn) return;

        target = player;
        isMagnetOn = true;
    }
    void Update()
    {
        if(isMagnetOn && target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Player")) return;

        es.GetExp(expAmount);
        //TODO: 경험치 획득 로직 추가
        //TODO: 풀링할시 SetActive(false)로 변경
        //ExperienceService GetExt(int)

        // pooling service based pooling
        GameManager.Instance.GetService<PoolingService>().ReturnOrDestroyGameObject(gameObject);
    }
}
