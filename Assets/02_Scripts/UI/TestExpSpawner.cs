using System;
using UnityEngine;

public class TestExpSpawner : MonoBehaviour
{
    ExperienceService es;

    private void Awake()
    {
        es = GameManager.Instance.GetService<ExperienceService>();
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            es.GetExp(10);     
        }
    }
}
