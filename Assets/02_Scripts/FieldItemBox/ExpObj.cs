using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpObj : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10.0f;
    [SerializeField] private Transform target;
    private bool isMagnetOn = false;

    private CircleCollider2D circleCollider;

    private void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();

        if (circleCollider != null) return;

        if(circleCollider == null)
        {
            circleCollider.gameObject.AddComponent<CircleCollider2D>();
        }
        circleCollider.isTrigger = true;
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
        if(collision.CompareTag("Player"))
        {
            //TODO: 경험치 획득 로직 추가
            //TODO: 풀링할시 SetActive(false)로 변경
            Destroy(gameObject);
        }
    }
}
