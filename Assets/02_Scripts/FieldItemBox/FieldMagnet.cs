using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldMagnet : MonoBehaviour
{
    public static event Action<Transform> onMagnet;

    [SerializeField] private string playerTag = "Player";

    private CircleCollider2D circleCol;
    private void Awake()
    {
        circleCol = GetComponent<CircleCollider2D>();

        if (circleCol != null) return;

        if(circleCol == null)
        {
            circleCol = gameObject.AddComponent<CircleCollider2D>();
        }
        circleCol.isTrigger = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(playerTag)) return;

        onMagnet?.Invoke(collision.transform);
        gameObject.SetActive(false);
    }   
}
