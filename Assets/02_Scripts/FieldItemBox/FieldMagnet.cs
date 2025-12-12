using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldMagnet : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            GetExp(collision.transform);
            gameObject.SetActive(false);
        }
    }
    void GetExp(Transform player)
    {        
        foreach(ExpObj exp in ExpObj.expObjList)
        {
            if(exp != null)
            {
                exp.StartMagnet(player);
            }
        }
    }
}
