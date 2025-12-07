using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpObj : MonoBehaviour
{
    public static List<ExpObj> expObjList = new List<ExpObj>();

    [SerializeField] private float moveSpeed = 10.0f;
    [SerializeField] private Transform target;
    private bool isMagnetOn = false;
    private void OnEnable()
    {
        expObjList.Add(this);
    }
    private void OnDisable()
    {
        expObjList.Remove(this);
    }
    public void StartMagnet(Transform player)
    {
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
            // °æÇèÄ¡ È¹µæ ·ÎÁ÷ Ãß°¡
            Destroy(gameObject);
        }
    }
}
