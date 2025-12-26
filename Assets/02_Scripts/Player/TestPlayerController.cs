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

    private PlayerFeedService pfs;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //TODO: 실행순서로 인한 null버그 있음 Start로 바꿔 볼 예정
        GameManager.Instance.GetService<GameContextService>().RegisterPlayerObject(gameObject); // TODO: remove
        pfs = GameManager.Instance.GetService<PlayerFeedService>();
        StartCoroutine(UpdateFeedAfterPhysicsUpdate());
    }

    IEnumerator UpdateFeedAfterPhysicsUpdate()
    {
        var wait = new WaitForFixedUpdate();
        while (true)
        {
            yield return wait;
            if (!gameObject.activeInHierarchy) continue;
            pfs.playerPosition = rb.position;
        }
    }

    private void FixedUpdate()
    {
        Vector2 dir = pfs.userInputDirection;
        rb.velocity = dir * moveSpeed;

        switch (System.Math.Sign(dir.x))
        {
            case -1:
                spriteRenderer.flipX = true;
                break;
            case 1:
                spriteRenderer.flipX = false;
                break;
        }
        anim.SetFloat(moveHash, rb.velocity.sqrMagnitude);
        MoveCrossHair();
    }
    private void MoveCrossHair()
    {
        Vector3 dir = new Vector3(inputX, inputY).normalized;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        indicator.transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
        indicator.transform.position = transform.position + (dir * 1.0f);
    }   
}