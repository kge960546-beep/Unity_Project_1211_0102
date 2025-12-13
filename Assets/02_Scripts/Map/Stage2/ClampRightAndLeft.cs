using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampRightAndLeft : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private Vector2 minLimit;
    [SerializeField] private Vector2 maxLimit;

    Rigidbody2D playerRb;
    private void Awake()
    {
        if (player != null)
            playerRb = player.GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (player == null) return;

        //Limit range calculation
        Vector2 playerPos = playerRb.position;

        float x = Mathf.Clamp(playerPos.x, minLimit.x, maxLimit.x);       

        Vector2 clamped = new Vector2(x, playerPos.y);

        //If you exceed the range, turn back and adjust the speed so that you don't exceed it again.
        // TODO: 떨림현상은 플레이어 컨트롤러 Update -> FixedUpdate 로 전환해야 수정 가능할것으로 예상
        if (Mathf.Abs(x - playerPos.x) > 0.0001f)
        {
            playerRb.MovePosition(clamped);

            Vector2 velocity = playerRb.velocity;

            if (playerPos.x < minLimit.x && velocity.x < 0.0f)
            {
                velocity.x = 0.0f;
            }
            if(playerPos.x > maxLimit.x && velocity.x > 0.0f)
            {
                velocity.x = 0.0f;
            }
            
            playerRb.velocity = velocity;
        }
    }   
}
