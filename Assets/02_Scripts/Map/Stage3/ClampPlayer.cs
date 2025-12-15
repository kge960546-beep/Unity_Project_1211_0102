using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampPlayer : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private Vector2 minLimit;
    [SerializeField] private Vector2 maxLimit;

    Rigidbody2D playerRb;
    private void Awake()
    {
        if(player != null)
            playerRb = player.GetComponent<Rigidbody2D>();
    }   
    private void FixedUpdate()
    {
        if (player == null) return;

        //Limit range calculation
        Vector2 playerPos = playerRb.position;        
        
        float x = Mathf.Clamp(playerPos.x, minLimit.x, maxLimit.x);
        float y = Mathf.Clamp(playerPos.y, minLimit.y, maxLimit.y);

        Vector2 clamped = new Vector2(x, y);

        //If you exceed the range, turn back and adjust the speed so that you don't exceed it again.
        // TODO: 떨림현상은 플레이어 컨트롤러 Update -> FixedUpdate 로 전환해야 수정 가능할것으로 예상
        if (clamped != playerPos)
        {
            playerRb.position = clamped;
        
            Vector2 velocity = playerRb.velocity;
        
            if(playerPos.x < minLimit.x || playerPos.x > maxLimit.x)
            {
                velocity.x = 0.0f;
            }
            if(playerPos.y < minLimit.y || playerPos.y > maxLimit.y)
            {
                velocity.y = 0.0f;
            }
        
            playerRb.velocity = velocity;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Vector3 p1 = new Vector3(minLimit.x, minLimit.y, 0);
        Vector3 p2 = new Vector3(maxLimit.x, minLimit.y, 0);
        Vector3 p3 = new Vector3(maxLimit.x, maxLimit.y, 0);
        Vector3 p4 = new Vector3(minLimit.x, maxLimit.y, 0);

        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawLine(p2, p3);
        Gizmos.DrawLine(p3, p4);
        Gizmos.DrawLine(p4, p1);
    }
}