using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float inputX;
    public float inputY;

    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;

    Animator anim;
    private static readonly int moveHash = Animator.StringToHash("Speed");
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        GameManager.Instance.GetService<GameContextService>().RegisterPlayerObject(gameObject);
    }
    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
        
        rb.velocity = new Vector2 (inputX * moveSpeed, inputY * moveSpeed);

        if(inputX != 0)
        {
            if(inputX < 0)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }            
        }
        float speed = new Vector2(inputX, inputY).sqrMagnitude;
        anim.SetFloat(moveHash,speed);
    }
}
