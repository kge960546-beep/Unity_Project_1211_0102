using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float inputX;
    public float inputY;
    Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
        
        rb.velocity = new Vector2 (inputX * moveSpeed, inputY * moveSpeed);

    }
}
