using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;

public class TestPlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float inputX;
    public float inputY;
    
    [SerializeField] private GameObject indicator;

    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;

    Animator anim;    
    private static readonly int moveHash = Animator.StringToHash("Speed");
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //TODO: 실행순서로 인한 null버그 있음 Start로 바꿔 볼 예정
        GameManager.Instance.GetService<GameContextService>().RegisterPlayerObject(gameObject);
    }
    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");        
        
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

        MoveCrossHair();
    }
    private void FixedUpdate()
    {
        Vector2 dir = new Vector2(inputX, inputY).normalized;
        rb.velocity = dir * moveSpeed;
    }
    private void MoveCrossHair()
    {
        Vector3 dir = new Vector3(inputX, inputY).normalized;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        indicator.transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
        indicator.transform.position = transform.position + (dir * 1.0f);
    }   
}